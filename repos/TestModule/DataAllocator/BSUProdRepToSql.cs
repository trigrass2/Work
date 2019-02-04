using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DataAllocator
{
    public class BSUProdRepToSql
    {
        string path = "\\\\10.5.0.196\\Export\\";
        private readonly LOG_ZAVOD_NFEntities _dbContext;        
        private System.Threading.Timer timer;
        private static bool isRunning;
        public LastEntry lastEntry;
        private string messageBSUPrud = null;
        private List<BSU_Production> bSU_Productions;

        private delegate void SenderText(string lastEntry, System.Windows.Forms.Form form);
        SenderText senderText;
        SenderText senderState;
        public LastEntry stateModule = new LastEntry() { Message = "выключен" };        

        public BSUProdRepToSql()
        {            
            _dbContext = new LOG_ZAVOD_NFEntities();
            try
            {
                if (_dbContext.BSU_Production.Any())
                {
                    var bsu = _dbContext.BSU_Production.First();
                    messageBSUPrud = "BSU_Production. Время: " + bsu.Date.ToShortDateString() + " " + bsu.Time.ToString();
                }

            }
            catch (Exception)
            {
                Scope.WriteError("BSU/BSUProdRepToSql()");
            }

            lastEntry = new LastEntry() { Message = messageBSUPrud };
            senderText = new SenderText(SendTextOnActiveForm);
            senderState = new SenderText(SendStateOnActiveForm);
        }

        public void StartModule(object sender, DoWorkEventArgs e)
        {
            try
            {
                SendStateOnActiveForm("включен", Form.ActiveForm);
            }
            catch (Exception) { }

            List<string> files = new List<string>(Directory.GetFiles(path, "*Production report.csv"));

            if (!_dbContext.BSU_Production.Any())
            {
                DateTime defaultTime = new DateTime();
                foreach (string f in files)
                {
                    WriteData(f, defaultTime.Date, defaultTime.TimeOfDay);
                    SaveData(bSU_Productions);
                }
            }

            try
            {
                System.Threading.TimerCallback callback = new System.Threading.TimerCallback(UpdateTable);
                timer = new System.Threading.Timer(callback, null, 0, 60000);
            }
            catch (Exception ex)
            {
                Scope.WriteGlobalError("BSUProdRepToSql - " + ex.Message);
            }
        }

        public void StopModule()
        {
            try
            {
                SendStateOnActiveForm("выключен", Form.ActiveForm);
            }
            catch (Exception) { }
            if (timer != null)
            {
                timer.Change(Timeout.Infinite, 0);
                timer.Dispose();
            }
        }

        private void UpdateTable(object obj)
        {
            try
            {
                if (isRunning) return;
                isRunning = true;

                DateTime startDate = new DateTime();
                TimeSpan? startTime = new TimeSpan();
                List<string> files;

                try
                {

                    startDate = _dbContext.BSU_Production.Max(x => x.Date);
                    startTime = _dbContext.BSU_Production.Where(x => x.Date == startDate).Max(x => x.Time);

                    files = new List<string>(Directory.GetFiles(path, "Production report.csv"));
                }
                catch (Exception ex)
                {
                    Scope.WriteError(" Поиск в _dbContextMES.BSU_Production -> " + ex.Message);
                    return;
                }

                List<string> needFile = new List<string>();
                try
                {
                    needFile = new List<string>(Directory.GetFiles(path, "*Production report.csv").Where(x => File.GetLastWriteTime(x).Date >= startDate));
                }
                catch (Exception ex)
                {

                    Scope.WriteError("BSUProdRepToSql/needFile = new List<string>(Directory.GetFiles(path,  *.csv).Where(x => File.GetLastWriteTime(x) >= start)) -> " + ex.Message);
                }

                DateTime newFileDateTime;
                if (needFile.Any())
                {
                    newFileDateTime = File.GetLastWriteTime(needFile.ElementAt(needFile.Count() - 1));
                    if (newFileDateTime.Date >= startDate)
                    {
                        foreach (var f in needFile)
                        {
                            WriteData(f, startDate.Date, startTime);
                            if (bSU_Productions != null && bSU_Productions.Any())
                                SaveData(bSU_Productions);
                        }
                    }
                }

                isRunning = false;
            }
            catch (Exception ex)
            {
                Scope.WriteGlobalError("BSUProdRepToSql - " + ex.Message);
            }
            
        }
        private void WriteData(string path, DateTime lastDateInDB, TimeSpan? lastTimeInDB)
        {         

            string[] str = { ";" };
            List<string> csvStrings = new List<string>(File.ReadAllLines(path, Encoding.Default));
            string start = string.Empty;

            if (_dbContext.BSU_Production.Any())
            {
                start = lastDateInDB.ToString("yyyy.MM.dd").Replace(".", "-") + ";" + lastTimeInDB.ToString();
                csvStrings.RemoveRange(0, csvStrings.FindIndex(x => x.Contains(start)) + 1);
                if (csvStrings.Count == 0) return;
            }

            
            
            List<string> listCsv = new List<string>();
            bSU_Productions = new List<BSU_Production>();
            
            for (int i = 1; i < csvStrings.Count; i++)
            {
                try 
                {
                    listCsv = csvStrings[i].Split(str, StringSplitOptions.None).ToList();                    

                    bSU_Productions.Add(GetProduction(listCsv));                   
                }
                catch (FormatException)
                {
                    
                }
            }           

        }

        private BSU_Production GetProduction(List<string> data)
        {
            int columnNumber = 1;
            BSU_Production production = new BSU_Production();
            
            for(int i = 0; i < data.Count; i++) {               

                switch (columnNumber)
                {
                    case 1:   production.Date = Convert.ToDateTime(data[i]).Date; break;
                    case 2:   production.Time = Convert.ToDateTime(data[i]).TimeOfDay; break;
                    case 3:   production.Number = Int32.TryParse(data[i], out int k0) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 4:   production.ProductId = Int32.TryParse(data[i], out int j1) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 5:   production.ProductName = data[i]; break;
                    case 6:   production.Volume = float.Parse(data[i].Replace(".", ",")); break;
                    case 7:   production.SpecId = Int32.TryParse(data[i], out int j2) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 8:   production.SpecName = Int32.TryParse(data[i], out int j3) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 9:   production.Operator = data[i]; break;
                    case 10:  production.UnitId = Int32.TryParse(data[i], out int j4) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 11:  production.MixerID = Int32.TryParse(data[i], out int j5) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 12:  production.WCfactSet = float.Parse(data[i].Replace(".", ",")); break;
                    case 13:  production.WCfactPerm = Int32.TryParse(data[i], out int j6) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 14:  production.LastSet = Int32.TryParse(data[i], out int j7) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 15:  production.DosTimeDate = Convert.ToDateTime(data[i]).Date; break;
                    case 16:  production.DosTimeOrder = Convert.ToDateTime(data[i]).TimeOfDay; break;
                    case 17:  production.DosTimeDosing = Convert.ToDateTime(data[i]).TimeOfDay; break;
                    case 18:  production.DosTimeStart = Convert.ToDateTime(data[i]).TimeOfDay; break;
                    case 19:  production.DosTimeHygStart = Convert.ToDateTime(data[i]).TimeOfDay; break;
                    case 20:  production.DosTimeHygEnd = Convert.ToDateTime(data[i]).TimeOfDay; break;
                    case 21:  production.DosTimeHygDuration = Convert.ToDateTime(data[i]).TimeOfDay; break;
                    case 22:  production.DosTimeDischarge = Convert.ToDateTime(data[i]).TimeOfDay; break;
                    case 23:  production.DosTimeMixTime = Convert.ToDateTime(data[i]).TimeOfDay; break;
                    case 24:  production.WCwet = float.Parse(data[i].Replace(".", ",")); break;
                    case 25:  production.WCwater = float.Parse(data[i].Replace(".", ",")); break;
                    case 26:  production.WCabswater = Int32.TryParse(data[i], out int j8) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 27:  production.WCSolidsInWater = Int32.TryParse(data[i], out int j9) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 28:  production.WCprelWater = Int32.TryParse(data[i], out int j10) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 29:  production.WCWaterInAdds = float.Parse(data[i].Replace(".", ",")); break;
                    case 30:  production.WCWaterInPaint = Int32.TryParse(data[i], out int j11) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 31:  production.WCCement = float.Parse(data[i].Replace(".", ",")); break;
                    case 32:  production.WCPropOfFiller = Int32.TryParse(data[i], out int j12) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 33:  production.WC = float.Parse(data[i].Replace(".", ",")); break;
                    case 34:  production.HygDryMixSet =  Int32.TryParse(data[i], out int j13) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 35:  production.HygWetMixSet =  Int32.TryParse(data[i], out int j14) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 36:  production.HygDryMixFact = Int32.TryParse(data[i], out int j15) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 37:  production.HygWetMixFact = Int32.TryParse(data[i], out int j16) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 38:  production.HygSteamTime = Int32.TryParse(data[i], out int j17) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 39:  production.HygDrySet = float.Parse(data[i].Replace(".", ",")); break;
                    case 40:  production.HygWetSet = float.Parse(data[i].Replace(".", ",")); break;
                    case 41:  production.HygDryFact = Int32.TryParse(data[i], out int j18) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 42:  production.HygWetFact = Int32.TryParse(data[i], out int j19) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 43:  production.HygManualWater1 = Int32.TryParse(data[i], out int j21) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 44:  production.HygTemp = float.Parse(data[i].Replace(".", ",")); break;
                    case 45:  production.HygRecipe = Int32.TryParse(data[i], out int j20) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 46:  production.HygWaterSet = float.Parse(data[i].Replace(".", ",")); break;
                    case 47:  production.HygWaterFact = float.Parse(data[i].Replace(".", ",")); break;
                    case 48:  production.HygRecWaterSet = Int32.TryParse(data[i], out int q) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 49:  production.HygRecWaterFact = Int32.TryParse(data[i], out int w) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 50:  production.HygCleanWaterSet = Int32.TryParse(data[i], out int e) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 51:  production.HygCleanWaterFact = Int32.TryParse(data[i], out int r) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 52:  production.HygProbeSet = Int32.TryParse(data[i], out int t) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 53:  production.HygProbeFact = Int32.TryParse(data[i], out int y) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 54:  production.HygWaterTemp = Int32.TryParse(data[i], out int u) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 55:  production.HygRecWaterFill1 = Int32.TryParse(data[i], out int o) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 56:  production.HygRecWaterFill2 = Int32.TryParse(data[i], out int p) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 57:  production.HygWaterCorrection = Int32.TryParse(data[i], out int a) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 58:  production.HygManualWater2 = Int32.TryParse(data[i], out int s) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 59:  production.HygFlushWater = Int32.TryParse(data[i], out int d) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 60:  production.HygCleanWaterMax = Int32.TryParse(data[i], out int l) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 61:  production.HygRecWaterMax = Int32.TryParse(data[i], out int m) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 62:  production.HygSolidRecWater = Int32.TryParse(data[i], out int n) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 63:  production.HygMicrowave = float.Parse(data[i].Replace(".", ",")); break;
                    case 64:  production.HygSet = float.Parse(data[i].Replace(".", ",")); break;
                    case 65:  production.HygStart = Convert.ToDateTime(data[i]).TimeOfDay; break;
                    case 66:  production.HygStop = Convert.ToDateTime(data[i]).TimeOfDay; break;
                    case 67:  production.HygEnd = Convert.ToDateTime(data[i]).TimeOfDay; break;
                    case 68:  production.MaterialId1 = Int32.TryParse(data[i], out int v) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 69:  production.MaterialName1 = data[i]; break;
                    case 70:  production.MaterialAmount1 = float.Parse(data[i].Replace(".", ",")); break;
                    case 71:  production.MaterialSet1 = float.Parse(data[i].Replace(".", ",")); break;
                    case 72:  production.MaterialManual1 = float.Parse(data[i].Replace(".", ",")); break;
                    case 73:  production.MaterialWetPerc1 = float.Parse(data[i].Replace(".", ",")); break;
                    case 74:  production.MaterialWet1 = float.Parse(data[i].Replace(".", ",")); break;
                    case 75:  production.MaterialAbsPerc1 = float.Parse(data[i].Replace(".", ",")); break;
                    case 76:  production.MaterialAbs1 = float.Parse(data[i].Replace(".", ",")); break;
                    case 77:  production.MaterialCementPart1 = float.Parse(data[i].Replace(".", ",")); break;
                    case 78:  production.MaterialFiller1 = Int32.TryParse(data[i], out int b) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 79:  production.MaterialKValue1 = float.Parse(data[i].Replace(".", ",")); break;
                    case 80:  production.MaterialInhibitor1 = Int32.TryParse(data[i], out int x) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 81:  production.MaterialSolidPerc1 = float.Parse(data[i].Replace(".", ",")); break;
                    case 82:  production.MaterialLiquidPerc1 = float.Parse(data[i].Replace(".", ",")); break;
                    case 83:  production.MaterialRecValue1 = float.Parse(data[i].Replace(".", ",")); break;
                    case 84:  production.MaterialDen1 = float.Parse(data[i].Replace(".", ",")); break;
                    case 85:  production.MaterialVolume1 = float.Parse(data[i].Replace(".", ",")); break;
                    case 86:  production.MaterialDrySet1 = float.Parse(data[i].Replace(".", ",")); break;
                    case 87:  production.MaterialWetSet1 = float.Parse(data[i].Replace(".", ",")); break;
                    case 88:  production.MaterialNote1 = data[i];break;
                    case 89:  production.MaterialSetId1 = data[i];break;
                    case 90:  production.MaterialId2 = Int32.TryParse(data[i], out int z) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 91:  production.MaterialName2 = data[i];break;
                    case 92:  production.MaterialAmount2 = float.Parse(data[i].Replace(".", ",")); break;
                    case 93:  production.MaterialSet2 = float.Parse(data[i].Replace(".", ",")); break;
                    case 94:  production.MaterialManual2 = float.Parse(data[i].Replace(".", ",")); break;
                    case 95:  production.MaterialWetPerc2 = float.Parse(data[i].Replace(".", ",")); break;
                    case 96:  production.MaterialWet2 = float.Parse(data[i].Replace(".", ",")); break;
                    case 97:  production.MaterialAbsPerc2 = float.Parse(data[i].Replace(".", ",")); break;
                    case 98:  production.MaterialAbs2 = float.Parse(data[i].Replace(".", ",")); break;
                    case 99:  production.MaterialCementPart2 = float.Parse(data[i].Replace(".", ",")); break;
                    case 100: production.MaterialFiller2 = Int32.TryParse(data[i], out int k) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 101: production.MaterialKValue2 = float.Parse(data[i].Replace(".", ",")); break;
                    case 102: production.MaterialInhibitor2 = Int32.TryParse(data[i], out int k2) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 103: production.MaterialSolidPerc2 = float.Parse(data[i].Replace(".", ",")); break;
                    case 104: production.MaterialLiquidPerc2 = float.Parse(data[i].Replace(".", ",")); break;
                    case 105: production.MaterialRecValue2 = float.Parse(data[i].Replace(".", ",")); break;
                    case 106: production.MaterialDen2 = float.Parse(data[i].Replace(".", ",")); break;
                    case 107: production.MaterialVolume2 = float.Parse(data[i].Replace(".", ",")); break;
                    case 108: production.MaterialDrySet2 = float.Parse(data[i].Replace(".", ",")); break;
                    case 109: production.MaterialWetSet2 = float.Parse(data[i].Replace(".", ",")); break;
                    case 110: production.MaterialNote2 = data[i];break;
                    case 111: production.MaterialSetId2 = data[i];break;
                    case 112: production.MaterialId3 = Int32.TryParse(data[i], out int k3) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 113: production.MaterialName3 = data[i]; break;
                    case 114: production.MaterialAmount3 = float.Parse(data[i].Replace(".", ",")); break;
                    case 115: production.MaterialSet3 = float.Parse(data[i].Replace(".", ",")); break;
                    case 116: production.MaterialManual3 = float.Parse(data[i].Replace(".", ",")); break;
                    case 117: production.MaterialWetPerc3 = float.Parse(data[i].Replace(".", ",")); break;
                    case 118: production.MaterialWet3 = float.Parse(data[i].Replace(".", ",")); break;
                    case 119: production.MaterialAbsPerc3 = float.Parse(data[i].Replace(".", ",")); break;
                    case 120: production.MaterialAbs3 = float.Parse(data[i].Replace(".", ",")); break;
                    case 121: production.MaterialCementPart3 = float.Parse(data[i].Replace(".", ",")); break;
                    case 122: production.MaterialFiller3 = Int32.TryParse(data[i], out int k4) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 123: production.MaterialKValue3 = float.Parse(data[i].Replace(".", ",")); break;
                    case 124: production.MaterialInhibitor3 = Int32.TryParse(data[i], out int k5) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 125: production.MaterialSolidPerc3 = float.Parse(data[i].Replace(".", ",")); break;
                    case 126: production.MaterialLiquidPerc3 = float.Parse(data[i].Replace(".", ",")); break;
                    case 127: production.MaterialRecValue3 = float.Parse(data[i].Replace(".", ",")); break;
                    case 128: production.MaterialDen3 = float.Parse(data[i].Replace(".", ",")); break;
                    case 129: production.MaterialVolume3 = float.Parse(data[i].Replace(".", ",")); break;
                    case 130: production.MaterialDrySet3 = float.Parse(data[i].Replace(".", ",")); break;
                    case 131: production.MaterialWetSet3 = float.Parse(data[i].Replace(".", ",")); break;
                    case 132: production.MaterialNote3 = data[i];break;
                    case 133: production.MaterialSetId3 = data[i];break;
                    case 134: production.MaterialId4 = Int32.TryParse(data[i], out int k6) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 135: production.MaterialName4 = data[i];break;
                    case 136: production.MaterialAmount4 = float.Parse(data[i].Replace(".", ",")); break;
                    case 137: production.MaterialSet4 = float.Parse(data[i].Replace(".", ",")); break;
                    case 138: production.MaterialManual4 = float.Parse(data[i].Replace(".", ",")); break;
                    case 139: production.MaterialWetPerc4 = float.Parse(data[i].Replace(".", ",")); break;
                    case 140: production.MaterialWet4 = float.Parse(data[i].Replace(".", ",")); break;
                    case 141: production.MaterialAbsPerc4 = float.Parse(data[i].Replace(".", ",")); break;
                    case 142: production.MaterialAbs4 = float.Parse(data[i].Replace(".", ",")); break;
                    case 143: production.MaterialCementPart4 = float.Parse(data[i].Replace(".", ",")); break;
                    case 144: production.MaterialFiller4 = Int32.TryParse(data[i], out int k7) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 145: production.MaterialKValue4 = float.Parse(data[i].Replace(".", ",")); break;
                    case 146: production.MaterialInhibitor4 = Int32.TryParse(data[i], out int k8) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 147: production.MaterialSolidPerc4 = float.Parse(data[i].Replace(".", ",")); break;
                    case 148: production.MaterialLiquidPerc4 = float.Parse(data[i].Replace(".", ",")); break;
                    case 149: production.MaterialRecValue4 = float.Parse(data[i].Replace(".", ",")); break;
                    case 150: production.MaterialDen4 = float.Parse(data[i].Replace(".", ",")); break;
                    case 151: production.MaterialVolume4 = float.Parse(data[i].Replace(".", ",")); break;
                    case 152: production.MaterialDrySet4 = float.Parse(data[i].Replace(".", ",")); break;
                    case 153: production.MaterialWetSet4 = float.Parse(data[i].Replace(".", ",")); break;
                    case 154: production.MaterialNote4 = data[i];break;
                    case 155: production.MaterialSetId4 = data[i];break;
                    case 156: production.MaterialId5 = Int32.TryParse(data[i], out int k9) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 157: production.MaterialName5 = data[i];break;
                    case 158: production.MaterialAmount5 = float.Parse(data[i].Replace(".", ",")); break;
                    case 159: production.MaterialSet5 = float.Parse(data[i].Replace(".", ",")); break;
                    case 160: production.MaterialManual5 = float.Parse(data[i].Replace(".", ",")); break;
                    case 161: production.MaterialWetPerc5 = float.Parse(data[i].Replace(".", ",")); break;
                    case 162: production.MaterialWet5 = float.Parse(data[i].Replace(".", ",")); break;
                    case 163: production.MaterialAbsPerc5 = float.Parse(data[i].Replace(".", ",")); break;
                    case 164: production.MaterialAbs5 = float.Parse(data[i].Replace(".", ",")); break;
                    case 165: production.MaterialCementPart5 = float.Parse(data[i].Replace(".", ",")); break;
                    case 166: production.MaterialFiller5 = Int32.TryParse(data[i], out int k10) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 167: production.MaterialKValue5 = float.Parse(data[i].Replace(".", ",")); break;
                    case 168: production.MaterialInhibitor5 = Int32.TryParse(data[i], out int k11) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 169: production.MaterialSolidPerc5 = float.Parse(data[i].Replace(".", ",")); break;
                    case 170: production.MaterialLiquidPerc5 = float.Parse(data[i].Replace(".", ",")); break;
                    case 171: production.MaterialRecValue5 = float.Parse(data[i].Replace(".", ",")); break;
                    case 172: production.MaterialDen5 = float.Parse(data[i].Replace(".", ",")); break;
                    case 173: production.MaterialVolume5 = float.Parse(data[i].Replace(".", ",")); break;
                    case 174: production.MaterialDrySet5 = float.Parse(data[i].Replace(".", ",")); break;
                    case 175: production.MaterialWetSet5 = float.Parse(data[i].Replace(".", ",")); break;
                    case 176: production.MaterialNote5 = data[i];break;
                    case 177: production.MaterialSetId5 = data[i]; break;
                    case 178: production.MaterialId6 = Int32.TryParse(data[i], out int k12) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 179: production.MaterialName6 = data[i]; break;
                    case 180: production.MaterialAmount6 = float.Parse(data[i].Replace(".", ",")); break;
                    case 181: production.MaterialSet6 = float.Parse(data[i].Replace(".", ",")); break;
                    case 182: production.MaterialManual6 = float.Parse(data[i].Replace(".", ",")); break;
                    case 183: production.MaterialWetPerc6 = float.Parse(data[i].Replace(".", ",")); break;
                    case 184: production.MaterialWet6 = float.Parse(data[i].Replace(".", ",")); break;
                    case 185: production.MaterialAbsPerc6 = float.Parse(data[i].Replace(".", ",")); break;
                    case 186: production.MaterialAbs6 = float.Parse(data[i].Replace(".", ",")); break;
                    case 187: production.MaterialCementPart6 = float.Parse(data[i].Replace(".", ",")); break;
                    case 188: production.MaterialFiller6 = Int32.TryParse(data[i], out int k13) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 189: production.MaterialKValue6 = float.Parse(data[i].Replace(".", ",")); break;
                    case 190: production.MaterialInhibitor6 = Int32.TryParse(data[i], out int k14) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 191: production.MaterialSolidPerc6 = float.Parse(data[i].Replace(".", ",")); break;
                    case 192: production.MaterialLiquidPerc6 = float.Parse(data[i].Replace(".", ",")); break;
                    case 193: production.MaterialRecValue6 = float.Parse(data[i].Replace(".", ",")); break;
                    case 194: production.MaterialDen6 = float.Parse(data[i].Replace(".", ",")); break;
                    case 195: production.MaterialVolume6 = float.Parse(data[i].Replace(".", ",")); break; 
                    case 196: production.MaterialDrySet6 = float.Parse(data[i].Replace(".", ",")); break;
                    case 197: production.MaterialWetSet6 = float.Parse(data[i].Replace(".", ",")); break;
                    case 198: production.MaterialNote6 = data[i];break;
                    case 199: production.MaterialSetId6 = data[i];break;
                    case 200: production.MaterialId7 = Int32.TryParse(data[i], out int k15) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 201: production.MaterialName7 = data[i];break;
                    case 202: production.MaterialAmount7 = float.Parse(data[i].Replace(".", ",")); break;
                    case 203: production.MaterialSet7 = float.Parse(data[i].Replace(".", ",")); break;
                    case 204: production.MaterialManual7 = float.Parse(data[i].Replace(".", ",")); break;
                    case 205: production.MaterialWetPerc7 = float.Parse(data[i].Replace(".", ",")); break;
                    case 206: production.MaterialWet7 = float.Parse(data[i].Replace(".", ",")); break;
                    case 207: production.MaterialAbsPerc7 = float.Parse(data[i].Replace(".", ",")); break;
                    case 208: production.MaterialAbs7 = float.Parse(data[i].Replace(".", ",")); break;
                    case 209: production.MaterialCementPart7 = float.Parse(data[i].Replace(".", ",")); break;
                    case 210: production.MaterialFiller7 = Int32.TryParse(data[i], out int k16) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 211: production.MaterialKValue7 = float.Parse(data[i].Replace(".", ",")); break;
                    case 212: production.MaterialInhibitor7 = Int32.TryParse(data[i], out int k17) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 213: production.MaterialSolidPerc7 = float.Parse(data[i].Replace(".", ",")); break;
                    case 214: production.MaterialLiquidPerc7 = float.Parse(data[i].Replace(".", ",")); break;
                    case 215: production.MaterialRecValue7 = float.Parse(data[i].Replace(".", ",")); break;
                    case 216: production.MaterialDen7 = float.Parse(data[i].Replace(".", ",")); break;
                    case 217: production.MaterialVolume7 = float.Parse(data[i].Replace(".", ",")); break;
                    case 218: production.MaterialDrySet7 = float.Parse(data[i].Replace(".", ",")); break;
                    case 219: production.MaterialWetSet7 = float.Parse(data[i].Replace(".", ",")); break;
                    case 220: production.MaterialNote7 = data[i];break;
                    case 221: production.MaterialSetId7 = data[i]; break;
                    case 222: production.MaterialId8 = Int32.TryParse(data[i], out int c) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 223: production.MaterialName8 = data[i];break;
                    case 224: production.MaterialAmount8 = float.Parse(data[i].Replace(".", ",")); break;
                    case 225: production.MaterialSet8 = float.Parse(data[i].Replace(".", ",")); break;
                    case 226: production.MaterialManual8 = float.Parse(data[i].Replace(".", ",")); break;
                    case 227: production.MaterialWetPerc8 = float.Parse(data[i].Replace(".", ",")); break;
                    case 228: production.MaterialWet8 = float.Parse(data[i].Replace(".", ",")); break;
                    case 229: production.MaterialAbsPerc8 = float.Parse(data[i].Replace(".", ",")); break;
                    case 230: production.MaterialAbs8 = float.Parse(data[i].Replace(".", ",")); break;
                    case 231: production.MaterialCementPart8 = float.Parse(data[i].Replace(".", ",")); break;
                    case 232: production.MaterialFiller8 = Int32.TryParse(data[i], out int k18) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 233: production.MaterialKValue8 = float.Parse(data[i].Replace(".", ",")); break;
                    case 234: production.MaterialInhibitor8 = Int32.TryParse(data[i], out int k19) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 235: production.MaterialSolidPerc8 = float.Parse(data[i].Replace(".", ",")); break;
                    case 236: production.MaterialLiquidPerc8 = float.Parse(data[i].Replace(".", ",")); break;
                    case 237: production.MaterialRecValue8 = float.Parse(data[i].Replace(".", ",")); break;
                    case 238: production.MaterialDen8 = float.Parse(data[i].Replace(".", ",")); break;
                    case 239: production.MaterialVolume8 = float.Parse(data[i].Replace(".", ",")); break;
                    case 240: production.MaterialDrySet8 = float.Parse(data[i].Replace(".", ",")); break;
                    case 241: production.MaterialWetSet8 = float.Parse(data[i].Replace(".", ",")); break;
                    case 242: production.MaterialNote8 = data[i];break;
                    case 243: production.MaterialSetId8 = data[i]; break;
                    case 244: production.MaterialId9 = Int32.TryParse(data[i], out int k20) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 245: production.MaterialName9 = data[i];break;
                    case 246: production.MaterialAmount9 = float.Parse(data[i].Replace(".", ",")); break;
                    case 247: production.MaterialSet9 = float.Parse(data[i].Replace(".", ",")); break;
                    case 248: production.MaterialManual9 = float.Parse(data[i].Replace(".", ",")); break;
                    case 249: production.MaterialWetPerc9 = float.Parse(data[i].Replace(".", ",")); break;
                    case 250: production.MaterialWet9 = float.Parse(data[i].Replace(".", ",")); break;
                    case 251: production.MaterialAbsPerc9 = float.Parse(data[i].Replace(".", ",")); break;
                    case 252: production.MaterialAbs9 = float.Parse(data[i].Replace(".", ",")); break;
                    case 253: production.MaterialCementPart9 = float.Parse(data[i].Replace(".", ",")); break;
                    case 254: production.MaterialFiller9 = Int32.TryParse(data[i], out int k21) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 255: production.MaterialKValue9 = float.Parse(data[i].Replace(".", ",")); break;
                    case 256: production.MaterialInhibitor9 = Int32.TryParse(data[i], out int k22) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 257: production.MaterialSolidPerc9 = float.Parse(data[i].Replace(".", ",")); break;
                    case 258: production.MaterialLiquidPerc9 = float.Parse(data[i].Replace(".", ",")); break;
                    case 259: production.MaterialRecValue9 = float.Parse(data[i].Replace(".", ",")); break;
                    case 260: production.MaterialDen9 = float.Parse(data[i].Replace(".", ",")); break;
                    case 261: production.MaterialVolume9 = float.Parse(data[i].Replace(".", ",")); break;
                    case 262: production.MaterialDrySet9 = float.Parse(data[i].Replace(".", ",")); break;
                    case 263: production.MaterialWetSet9 = float.Parse(data[i].Replace(".", ",")); break;
                    case 264: production.MaterialNote9 = data[i];break;
                    case 265: production.MaterialSetId9 = data[i]; break;
                    case 266: production.MaterialId10 = Int32.TryParse(data[i], out int k23) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 267: production.MaterialName10 = data[i];break;
                    case 268: production.MaterialAmount10 = float.Parse(data[i].Replace(".", ",")); break;
                    case 269: production.MaterialSet10 = float.Parse(data[i].Replace(".", ",")); break;
                    case 270: production.MaterialManual10 = float.Parse(data[i].Replace(".", ",")); break;
                    case 271: production.MaterialWetPerc10 = float.Parse(data[i].Replace(".", ",")); break;
                    case 272: production.MaterialWet10 = float.Parse(data[i].Replace(".", ",")); break;
                    case 273: production.MaterialAbsPerc10 = float.Parse(data[i].Replace(".", ",")); break;
                    case 274: production.MaterialAbs10 = float.Parse(data[i].Replace(".", ",")); break;
                    case 275: production.MaterialCementPart10 = float.Parse(data[i].Replace(".", ",")); break;
                    case 276: production.MaterialFiller10 = Int32.TryParse(data[i], out int k24) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 277: production.MaterialKValue10 = float.Parse(data[i].Replace(".", ",")); break;
                    case 278: production.MaterialInhibitor10 = Int32.TryParse(data[i], out int k25) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 279: production.MaterialSolidPerc10 = float.Parse(data[i].Replace(".", ",")); break;
                    case 280: production.MaterialLiquidPerc10 = float.Parse(data[i].Replace(".", ",")); break;
                    case 281: production.MaterialRecValue10 = float.Parse(data[i].Replace(".", ",")); break;
                    case 282: production.MaterialDen10 = float.Parse(data[i].Replace(".", ",")); break;
                    case 283: production.MaterialVolume10 = float.Parse(data[i].Replace(".", ",")); break;
                    case 284: production.MaterialDrySet10 = float.Parse(data[i].Replace(".", ",")); break;
                    case 285: production.MaterialWetSet10 = float.Parse(data[i].Replace(".", ",")); break;
                    case 286: production.MaterialNote10 = data[i];break;
                    case 287: production.MaterialSetId10 = data[i];break;
                    case 288: production.MaterialId11 = Int32.TryParse(data[i], out int k26) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 289: production.MaterialName11 = data[i]; break;
                    case 290: production.MaterialAmount11 = float.Parse(data[i].Replace(".", ",")); break;
                    case 291: production.MaterialSet11 = float.Parse(data[i].Replace(".", ",")); break;
                    case 292: production.MaterialManual11 = float.Parse(data[i].Replace(".", ",")); break;
                    case 293: production.MaterialWetPerc11 = float.Parse(data[i].Replace(".", ",")); break;
                    case 294: production.MaterialWet11 = float.Parse(data[i].Replace(".", ",")); break;
                    case 295: production.MaterialAbsPerc11 = float.Parse(data[i].Replace(".", ",")); break;
                    case 296: production.MaterialAbs11 = float.Parse(data[i].Replace(".", ",")); break;
                    case 297: production.MaterialCementPart11 = float.Parse(data[i].Replace(".", ",")); break;
                    case 298: production.MaterialFiller11 = Int32.TryParse(data[i], out int k27) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 299: production.MaterialKValue11 = float.Parse(data[i].Replace(".", ",")); break;
                    case 300: production.MaterialInhibitor11 = Int32.TryParse(data[i], out int k28) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 301: production.MaterialSolidPerc11 = float.Parse(data[i].Replace(".", ",")); break;
                    case 302: production.MaterialLiquidPerc11 = float.Parse(data[i].Replace(".", ",")); break;
                    case 303: production.MaterialRecValue11 = float.Parse(data[i].Replace(".", ",")); break;
                    case 304: production.MaterialDen11 = float.Parse(data[i].Replace(".", ",")); break;
                    case 305: production.MaterialVolume11 = float.Parse(data[i].Replace(".", ",")); break;
                    case 306: production.MaterialDrySet11 = float.Parse(data[i].Replace(".", ",")); break;
                    case 307: production.MaterialWetSet11 = float.Parse(data[i].Replace(".", ",")); break;
                    case 308: production.MaterialNote11 = data[i];break;
                    case 309: production.MaterialSetId11 = data[i];break;
                    case 310: production.MaterialId12 = Int32.TryParse(data[i], out int k29) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 311: production.MaterialName12 = data[i];break;
                    case 312: production.MaterialAmount12 = float.Parse(data[i].Replace(".", ",")); break;
                    case 313: production.MaterialSet12 = float.Parse(data[i].Replace(".", ",")); break;
                    case 314: production.MaterialManual12 = float.Parse(data[i].Replace(".", ",")); break;
                    case 315: production.MaterialWetPerc12 = float.Parse(data[i].Replace(".", ",")); break;
                    case 316: production.MaterialWet12 = float.Parse(data[i].Replace(".", ",")); break;
                    case 317: production.MaterialAbsPerc12 = float.Parse(data[i].Replace(".", ",")); break;
                    case 318: production.MaterialAbs12 = float.Parse(data[i].Replace(".", ",")); break;
                    case 319: production.MaterialCementPart12 = float.Parse(data[i].Replace(".", ",")); break;
                    case 320: production.MaterialFiller12 = Int32.TryParse(data[i], out int k30) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 321: production.MaterialKValue12 = float.Parse(data[i].Replace(".", ",")); break;
                    case 322: production.MaterialInhibitor12 = Int32.TryParse(data[i], out int k31) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 323: production.MaterialSolidPerc12 = float.Parse(data[i].Replace(".", ",")); break;
                    case 324: production.MaterialLiquidPerc12 = float.Parse(data[i].Replace(".", ",")); break;
                    case 325: production.MaterialRecValue12 = float.Parse(data[i].Replace(".", ",")); break;
                    case 326: production.MaterialDen12 = float.Parse(data[i].Replace(".", ",")); break;
                    case 327: production.MaterialVolume12 = float.Parse(data[i].Replace(".", ",")); break;
                    case 328: production.MaterialDrySet12 = float.Parse(data[i].Replace(".", ",")); break;
                    case 329: production.MaterialWetSet12 = float.Parse(data[i].Replace(".", ",")); break;
                    case 330: production.MaterialNote12 = data[i];break;
                    case 331: production.MaterialSetId12 = data[i];break;
                    case 332: production.MaterialId13 = Int32.TryParse(data[i], out int k32) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 333: production.MaterialName13 = data[i];break;
                    case 334: production.MaterialAmount13 = float.Parse(data[i].Replace(".", ",")); break;
                    case 335: production.MaterialSet13 = float.Parse(data[i].Replace(".", ",")); break;
                    case 336: production.MaterialManual13 = float.Parse(data[i].Replace(".", ",")); break;
                    case 337: production.MaterialWetPerc13 = float.Parse(data[i].Replace(".", ",")); break;
                    case 338: production.MaterialWet13 = float.Parse(data[i].Replace(".", ",")); break;
                    case 339: production.MaterialAbsPerc13 = float.Parse(data[i].Replace(".", ",")); break;
                    case 340: production.MaterialAbs13 = float.Parse(data[i].Replace(".", ",")); break;
                    case 341: production.MaterialCementPart13 = float.Parse(data[i].Replace(".", ",")); break;
                    case 342: production.MaterialFiller13 = Int32.TryParse(data[i], out int k33) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 343: production.MaterialKValue13 = float.Parse(data[i].Replace(".", ",")); break;
                    case 344: production.MaterialInhibitor13 = Int32.TryParse(data[i], out int k34) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 345: production.MaterialSolidPerc13 = float.Parse(data[i].Replace(".", ",")); break;
                    case 346: production.MaterialLiquidPerc13 = float.Parse(data[i].Replace(".", ",")); break;
                    case 347: production.MaterialRecValue13 = float.Parse(data[i].Replace(".", ",")); break;
                    case 348: production.MaterialDen13 = float.Parse(data[i].Replace(".", ",")); break;
                    case 349: production.MaterialVolume13 = float.Parse(data[i].Replace(".", ",")); break;
                    case 350: production.MaterialDrySet13 = float.Parse(data[i].Replace(".", ",")); break;
                    case 351: production.MaterialWetSet13 = float.Parse(data[i].Replace(".", ",")); break;
                    case 352: production.MaterialNote13 = data[i];break;
                    case 353: production.MaterialSetId13 = data[i];break;
                    case 354: production.MaterialId14 = Int32.TryParse(data[i], out int k35) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 355: production.MaterialName14 = data[i];break;
                    case 356: production.MaterialAmount14 = float.Parse(data[i].Replace(".", ",")); break;
                    case 357: production.MaterialSet14 = float.Parse(data[i].Replace(".", ",")); break;
                    case 358: production.MaterialManual14 = float.Parse(data[i].Replace(".", ",")); break;
                    case 359: production.MaterialWetPerc14 = float.Parse(data[i].Replace(".", ",")); break;
                    case 360: production.MaterialWet14 = float.Parse(data[i].Replace(".", ",")); break;
                    case 361: production.MaterialAbsPerc14 = float.Parse(data[i].Replace(".", ",")); break;
                    case 362: production.MaterialAbs14 = float.Parse(data[i].Replace(".", ",")); break;
                    case 363: production.MaterialCementPart14 = float.Parse(data[i].Replace(".", ",")); break;
                    case 364: production.MaterialFiller14 = Int32.TryParse(data[i], out int k36) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 365: production.MaterialKValue14 = float.Parse(data[i].Replace(".", ",")); break;
                    case 366: production.MaterialInhibitor14 = Int32.TryParse(data[i], out int k37) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 367: production.MaterialSolidPerc14 = float.Parse(data[i].Replace(".", ",")); break;
                    case 368: production.MaterialLiquidPerc14 = float.Parse(data[i].Replace(".", ",")); break;
                    case 369: production.MaterialRecValue14 = float.Parse(data[i].Replace(".", ",")); break;
                    case 370: production.MaterialDen14 = float.Parse(data[i].Replace(".", ",")); break;
                    case 371: production.MaterialVolume14 = float.Parse(data[i].Replace(".", ",")); break;
                    case 372: production.MaterialDrySet14 = float.Parse(data[i].Replace(".", ",")); break;
                    case 373: production.MaterialWetSet14 = float.Parse(data[i].Replace(".", ",")); break;
                    case 374: production.MaterialNote14 = data[i];break;
                    case 375: production.MaterialSetId14 = data[i];break;
                    case 376: production.MaterialId15 = Int32.TryParse(data[i], out int k38) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 377: production.MaterialName15 = data[i]; break;
                    case 378: production.MaterialAmount15 = float.Parse(data[i].Replace(".", ",")); break;
                    case 379: production.MaterialSet15 = float.Parse(data[i].Replace(".", ",")); break;
                    case 380: production.MaterialManual15 = float.Parse(data[i].Replace(".", ",")); break;
                    case 381: production.MaterialWetPerc15 = float.Parse(data[i].Replace(".", ",")); break;
                    case 382: production.MaterialWet15 = float.Parse(data[i].Replace(".", ",")); break;
                    case 383: production.MaterialAbsPerc15 = float.Parse(data[i].Replace(".", ",")); break;
                    case 384: production.MaterialAbs15 = float.Parse(data[i].Replace(".", ",")); break;
                    case 385: production.MaterialCementPart15 = float.Parse(data[i].Replace(".", ",")); break;
                    case 386: production.MaterialFiller15 = Int32.TryParse(data[i], out int k39) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 387: production.MaterialKValue15 = float.Parse(data[i].Replace(".", ",")); break;
                    case 388: production.MaterialInhibitor15 = Int32.TryParse(data[i], out int k40) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 389: production.MaterialSolidPerc15 = float.Parse(data[i].Replace(".", ",")); break;
                    case 390: production.MaterialLiquidPerc15 = float.Parse(data[i].Replace(".", ",")); break;
                    case 391: production.MaterialRecValue15 = float.Parse(data[i].Replace(".", ",")); break;
                    case 392: production.MaterialDen15 = float.Parse(data[i].Replace(".", ",")); break;
                    case 393: production.MaterialVolume15 = float.Parse(data[i].Replace(".", ",")); break;
                    case 394: production.MaterialDrySet15 = float.Parse(data[i].Replace(".", ",")); break;
                    case 395: production.MaterialWetSet15 = float.Parse(data[i].Replace(".", ",")); break;
                    case 396: production.MaterialNote15 = data[i];break;
                    case 397: production.MaterialSetId15 = data[i];break;
                    case 398: production.MaterialId16 = Int32.TryParse(data[i], out int k41) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 399: production.MaterialName16 = data[i]; break;
                    case 400: production.MaterialAmount16 = float.Parse(data[i].Replace(".", ",")); break;
                    case 401: production.MaterialSet16 = float.Parse(data[i].Replace(".", ",")); break;
                    case 402: production.MaterialManual16 = float.Parse(data[i].Replace(".", ",")); break;
                    case 403: production.MaterialWetPerc16 = float.Parse(data[i].Replace(".", ",")); break;
                    case 404: production.MaterialWet16 = float.Parse(data[i].Replace(".", ",")); break;
                    case 405: production.MaterialAbsPerc16 = float.Parse(data[i].Replace(".", ",")); break;
                    case 406: production.MaterialAbs16 = float.Parse(data[i].Replace(".", ",")); break;
                    case 407: production.MaterialCementPart16 = float.Parse(data[i].Replace(".", ",")); break;
                    case 408: production.MaterialFiller16 = Int32.TryParse(data[i], out int k42) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 409: production.MaterialKValue16 = float.Parse(data[i].Replace(".", ",")); break;
                    case 410: production.MaterialInhibitor16 = Int32.TryParse(data[i], out int k43) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 411: production.MaterialSolidPerc16 = float.Parse(data[i].Replace(".", ",")); break;
                    case 412: production.MaterialLiquidPerc16 = float.Parse(data[i].Replace(".", ",")); break;
                    case 413: production.MaterialRecValue16 = float.Parse(data[i].Replace(".", ",")); break;
                    case 414: production.MaterialDen16 = float.Parse(data[i].Replace(".", ",")); break;
                    case 415: production.MaterialVolume16 = float.Parse(data[i].Replace(".", ",")); break;
                    case 416: production.MaterialDrySet16 = float.Parse(data[i].Replace(".", ",")); break;
                    case 417: production.MaterialWetSet16 = float.Parse(data[i].Replace(".", ",")); break;
                    case 418: production.MaterialNote16 = data[i];break;
                    case 419: production.MaterialSetId16 = data[i];break;
                    case 420: production.MaterialId17 = Int32.TryParse(data[i], out int k44) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 421: production.MaterialName17 = data[i]; break;
                    case 422: production.MaterialAmount17 = float.Parse(data[i].Replace(".", ",")); break;
                    case 423: production.MaterialSet17 = float.Parse(data[i].Replace(".", ",")); break;
                    case 424: production.MaterialManual17 = float.Parse(data[i].Replace(".", ",")); break;
                    case 425: production.MaterialWetPerc17 = float.Parse(data[i].Replace(".", ",")); break;
                    case 426: production.MaterialWet17 = float.Parse(data[i].Replace(".", ",")); break;
                    case 427: production.MaterialAbsPerc17 = float.Parse(data[i].Replace(".", ",")); break;
                    case 428: production.MaterialAbs17 = float.Parse(data[i].Replace(".", ",")); break;
                    case 429: production.MaterialCementPart17 = float.Parse(data[i].Replace(".", ",")); break;
                    case 430: production.MaterialFiller17 = Int32.TryParse(data[i], out int k45) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 431: production.MaterialKValue17 = float.Parse(data[i].Replace(".", ",")); break;
                    case 432: production.MaterialInhibitor17 = Int32.TryParse(data[i], out int k46) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 433: production.MaterialSolidPerc17 = float.Parse(data[i].Replace(".", ",")); break;
                    case 434: production.MaterialLiquidPerc17 = float.Parse(data[i].Replace(".", ",")); break;
                    case 435: production.MaterialRecValue17 = float.Parse(data[i].Replace(".", ",")); break;
                    case 436: production.MaterialDen17 = float.Parse(data[i].Replace(".", ",")); break;
                    case 437: production.MaterialVolume17 = float.Parse(data[i].Replace(".", ",")); break;
                    case 438: production.MaterialDrySet17 = float.Parse(data[i].Replace(".", ",")); break;
                    case 439: production.MaterialWetSet17 = float.Parse(data[i].Replace(".", ",")); break;
                    case 440: production.MaterialNote17 = data[i];break;
                    case 441: production.MaterialSetId17 = data[i]; break;
                    case 442: production.MaterialId18 = Int32.TryParse(data[i], out int k47) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 443: production.MaterialName18 = data[i]; break;
                    case 444: production.MaterialAmount18 = float.Parse(data[i].Replace(".", ",")); break;
                    case 445: production.MaterialSet18 = float.Parse(data[i].Replace(".", ",")); break;
                    case 446: production.MaterialManual18 = float.Parse(data[i].Replace(".", ",")); break;
                    case 447: production.MaterialWetPerc18 = float.Parse(data[i].Replace(".", ",")); break;
                    case 448: production.MaterialWet18 = float.Parse(data[i].Replace(".", ",")); break;
                    case 449: production.MaterialAbsPerc18 = float.Parse(data[i].Replace(".", ",")); break;
                    case 450: production.MaterialAbs18 = float.Parse(data[i].Replace(".", ",")); break;
                    case 451: production.MaterialCementPart18 = float.Parse(data[i].Replace(".", ",")); break;
                    case 452: production.MaterialFiller18 = Int32.TryParse(data[i], out int k48) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 453: production.MaterialKValue18 = float.Parse(data[i].Replace(".", ",")); break;
                    case 454: production.MaterialInhibitor18 = Int32.TryParse(data[i], out int k49) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 455: production.MaterialSolidPerc18 = float.Parse(data[i].Replace(".", ",")); break;
                    case 456: production.MaterialLiquidPerc18 = float.Parse(data[i].Replace(".", ",")); break;
                    case 457: production.MaterialRecValue18 = float.Parse(data[i].Replace(".", ",")); break;
                    case 458: production.MaterialDen18 = float.Parse(data[i].Replace(".", ",")); break;
                    case 459: production.MaterialVolume18 = float.Parse(data[i].Replace(".", ",")); break;
                    case 460: production.MaterialDrySet18 = float.Parse(data[i].Replace(".", ",")); break;
                    case 461: production.MaterialWetSet18 = float.Parse(data[i].Replace(".", ",")); break;
                    case 462: production.MaterialNote18 = data[i];break;
                    case 463: production.MaterialSetId18 = data[i];break;
                    case 464: production.MaterialId19 = Int32.TryParse(data[i], out int k50) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 465: production.MaterialName19 = data[i];break;
                    case 466: production.MaterialAmount19 = float.Parse(data[i].Replace(".", ",")); break;
                    case 467: production.MaterialSet19 = float.Parse(data[i].Replace(".", ",")); break;
                    case 468: production.MaterialManual19 = float.Parse(data[i].Replace(".", ",")); break;
                    case 469: production.MaterialWetPerc19 = float.Parse(data[i].Replace(".", ",")); break;
                    case 470: production.MaterialWet19 = float.Parse(data[i].Replace(".", ",")); break;
                    case 471: production.MaterialAbsPerc19 = float.Parse(data[i].Replace(".", ",")); break;
                    case 472: production.MaterialAbs19 = float.Parse(data[i].Replace(".", ",")); break;
                    case 473: production.MaterialCementPart19 = float.Parse(data[i].Replace(".", ",")); break;
                    case 474: production.MaterialFiller19 = Int32.TryParse(data[i], out int k51) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 475: production.MaterialKValue19 = float.Parse(data[i].Replace(".", ",")); break;
                    case 476: production.MaterialInhibitor19 = Int32.TryParse(data[i], out int k52) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 477: production.MaterialSolidPerc19 = float.Parse(data[i].Replace(".", ",")); break;
                    case 478: production.MaterialLiquidPerc19 = float.Parse(data[i].Replace(".", ",")); break;
                    case 479: production.MaterialRecValue19 = float.Parse(data[i].Replace(".", ",")); break;
                    case 480: production.MaterialDen19 = float.Parse(data[i].Replace(".", ",")); break;
                    case 481: production.MaterialVolume19 = float.Parse(data[i].Replace(".", ",")); break;
                    case 482: production.MaterialDrySet19 = float.Parse(data[i].Replace(".", ",")); break;
                    case 483: production.MaterialWetSet19 = float.Parse(data[i].Replace(".", ",")); break;
                    case 484: production.MaterialNote19 = data[i];break;
                    case 485: production.MaterialSetId19 = data[i];break;
                    case 486: production.MaterialId20 = Int32.TryParse(data[i], out int k53) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 487: production.MaterialName20 = data[i]; break;
                    case 488: production.MaterialAmount20 = float.Parse(data[i].Replace(".", ",")); break;
                    case 489: production.MaterialSet20 = float.Parse(data[i].Replace(".", ",")); break;
                    case 490: production.MaterialManual20 = float.Parse(data[i].Replace(".", ",")); break;
                    case 491: production.MaterialWetPerc20 = float.Parse(data[i].Replace(".", ",")); break;
                    case 492: production.MaterialWet20 = float.Parse(data[i].Replace(".", ",")); break;
                    case 493: production.MaterialAbsPerc20 = float.Parse(data[i].Replace(".", ",")); break;
                    case 494: production.MaterialAbs20 = float.Parse(data[i].Replace(".", ",")); break;
                    case 495: production.MaterialCementPart20 = float.Parse(data[i].Replace(".", ",")); break;
                    case 496: production.MaterialFiller20 = Int32.TryParse(data[i], out int k54) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 497: production.MaterialKValue20 = float.Parse(data[i].Replace(".", ",")); break;
                    case 498: production.MaterialInhibitor20 = Int32.TryParse(data[i], out int k55) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 499: production.MaterialSolidPerc20 = float.Parse(data[i].Replace(".", ",")); break;
                    case 500: production.MaterialLiquidPerc20 = float.Parse(data[i].Replace(".", ",")); break;
                    case 501: production.MaterialRecValue20 = float.Parse(data[i].Replace(".", ",")); break;
                    case 502: production.MaterialDen20 = float.Parse(data[i].Replace(".", ",")); break;
                    case 503: production.MaterialVolume20 = float.Parse(data[i].Replace(".", ",")); break;
                    case 504: production.MaterialDrySet20 = float.Parse(data[i].Replace(".", ",")); break;
                    case 505: production.MaterialWetSet20 = float.Parse(data[i].Replace(".", ",")); break;
                    case 506: production.MaterialNote20 = data[i];break;
                    case 507: production.MaterialSetId20 = data[i]; break;
                    case 508: production.MaterialId21 = Int32.TryParse(data[i], out int k56) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 509: production.MaterialName21 = data[i]; break;
                    case 510: production.MaterialAmount21 = float.Parse(data[i].Replace(".", ",")); break;
                    case 511: production.MaterialSet21 = float.Parse(data[i].Replace(".", ",")); break;
                    case 512: production.MaterialManual21 = float.Parse(data[i].Replace(".", ",")); break;
                    case 513: production.MaterialWetPerc21 = float.Parse(data[i].Replace(".", ",")); break;
                    case 514: production.MaterialWet21 = float.Parse(data[i].Replace(".", ",")); break;
                    case 515: production.MaterialAbsPerc21 = float.Parse(data[i].Replace(".", ",")); break;
                    case 516: production.MaterialAbs21 = float.Parse(data[i].Replace(".", ",")); break;
                    case 517: production.MaterialCementPart21 = float.Parse(data[i].Replace(".", ",")); break;
                    case 518: production.MaterialFiller21 = Int32.TryParse(data[i], out int k57) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 519: production.MaterialKValue21 = float.Parse(data[i].Replace(".", ",")); break;
                    case 520: production.MaterialInhibitor21 = Int32.TryParse(data[i], out int k58) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 521: production.MaterialSolidPerc21 = float.Parse(data[i].Replace(".", ",")); break;
                    case 522: production.MaterialLiquidPerc21 = float.Parse(data[i].Replace(".", ",")); break;
                    case 523: production.MaterialRecValue21 = float.Parse(data[i].Replace(".", ",")); break;
                    case 524: production.MaterialDen21 = float.Parse(data[i].Replace(".", ",")); break;
                    case 525: production.MaterialVolume21 = float.Parse(data[i].Replace(".", ",")); break;
                    case 526: production.MaterialDrySet21 = float.Parse(data[i].Replace(".", ",")); break;
                    case 527: production.MaterialWetSet21 = float.Parse(data[i].Replace(".", ",")); break;
                    case 528: production.MaterialNote21 = data[i];break;
                    case 529: production.MaterialSetId21 = data[i];break;
                    case 530: production.MaterialId22 = Int32.TryParse(data[i], out int k59) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 531: production.MaterialName22 = data[i]; break;
                    case 532: production.MaterialAmount22 = float.Parse(data[i].Replace(".", ",")); break;
                    case 533: production.MaterialSet22 = float.Parse(data[i].Replace(".", ",")); break;
                    case 534: production.MaterialManual22 = float.Parse(data[i].Replace(".", ",")); break;
                    case 535: production.MaterialWetPerc22 = float.Parse(data[i].Replace(".", ",")); break;
                    case 536: production.MaterialWet22 = float.Parse(data[i].Replace(".", ",")); break;
                    case 537: production.MaterialAbsPerc22 = float.Parse(data[i].Replace(".", ",")); break;
                    case 538: production.MaterialAbs22 = float.Parse(data[i].Replace(".", ",")); break;
                    case 539: production.MaterialCementPart22 = float.Parse(data[i].Replace(".", ",")); break;
                    case 540: production.MaterialFiller22 = Int32.TryParse(data[i], out int k60) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 541: production.MaterialKValue22 = float.Parse(data[i].Replace(".", ",")); break;
                    case 542: production.MaterialInhibitor22 = Int32.TryParse(data[i], out int k61) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 543: production.MaterialSolidPerc22 = float.Parse(data[i].Replace(".", ",")); break;
                    case 544: production.MaterialLiquidPerc22 = float.Parse(data[i].Replace(".", ",")); break;
                    case 545: production.MaterialRecValue22 = float.Parse(data[i].Replace(".", ",")); break;
                    case 546: production.MaterialDen22 = float.Parse(data[i].Replace(".", ",")); break;
                    case 547: production.MaterialVolume22 = float.Parse(data[i].Replace(".", ",")); break;
                    case 548: production.MaterialDrySet22 = float.Parse(data[i].Replace(".", ",")); break;
                    case 549: production.MaterialWetSet22 = float.Parse(data[i].Replace(".", ",")); break;
                    case 550: production.MaterialNote22 = data[i];break;
                    case 551: production.MaterialSetId22 = data[i]; break;
                    case 552: production.MaterialId23 = Int32.TryParse(data[i], out int k62) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 553: production.MaterialName23 = data[i]; break;
                    case 554: production.MaterialAmount23 = float.Parse(data[i].Replace(".", ",")); break;
                    case 555: production.MaterialSet23 = float.Parse(data[i].Replace(".", ",")); break;
                    case 556: production.MaterialManual23 = float.Parse(data[i].Replace(".", ",")); break;
                    case 557: production.MaterialWetPerc23 = float.Parse(data[i].Replace(".", ",")); break;
                    case 558: production.MaterialWet23 = float.Parse(data[i].Replace(".", ",")); break;
                    case 559: production.MaterialAbsPerc23 = float.Parse(data[i].Replace(".", ",")); break;
                    case 560: production.MaterialAbs23 = float.Parse(data[i].Replace(".", ",")); break;
                    case 561: production.MaterialCementPart23 = float.Parse(data[i].Replace(".", ",")); break;
                    case 562: production.MaterialFiller23 = Int32.TryParse(data[i], out int k63) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 563: production.MaterialKValue23 = float.Parse(data[i].Replace(".", ",")); break;
                    case 564: production.MaterialInhibitor23 = Int32.TryParse(data[i], out int k64) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 565: production.MaterialSolidPerc23 = float.Parse(data[i].Replace(".", ",")); break;
                    case 566: production.MaterialLiquidPerc23 = float.Parse(data[i].Replace(".", ",")); break;
                    case 567: production.MaterialRecValue23 = float.Parse(data[i].Replace(".", ",")); break;
                    case 568: production.MaterialDen23 = float.Parse(data[i].Replace(".", ",")); break;
                    case 569: production.MaterialVolume23 = float.Parse(data[i].Replace(".", ",")); break;
                    case 570: production.MaterialDrySet23 = float.Parse(data[i].Replace(".", ",")); break;
                    case 571: production.MaterialWetSet23 = float.Parse(data[i].Replace(".", ",")); break;
                    case 572: production.MaterialNote23 = data[i];break;
                    case 573: production.MaterialSetId23 = data[i];break;
                    case 574: production.MaterialId24 = Int32.TryParse(data[i], out int k65) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 575: production.MaterialName24 = data[i]; break;
                    case 576: production.MaterialAmount24 = float.Parse(data[i].Replace(".", ",")); break;
                    case 577: production.MaterialSet24 = float.Parse(data[i].Replace(".", ",")); break;
                    case 578: production.MaterialManual24 = float.Parse(data[i].Replace(".", ",")); break;
                    case 579: production.MaterialWetPerc24 = float.Parse(data[i].Replace(".", ",")); break;
                    case 580: production.MaterialWet24 = float.Parse(data[i].Replace(".", ",")); break;
                    case 581: production.MaterialAbsPerc24 = float.Parse(data[i].Replace(".", ",")); break;
                    case 582: production.MaterialAbs24 = float.Parse(data[i].Replace(".", ",")); break;
                    case 583: production.MaterialCementPart24 = float.Parse(data[i].Replace(".", ",")); break;
                    case 584: production.MaterialFiller24 = Int32.TryParse(data[i], out int k66) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 585: production.MaterialKValue24 = float.Parse(data[i].Replace(".", ",")); break;
                    case 586: production.MaterialInhibitor24 = Int32.TryParse(data[i], out int k67) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 587: production.MaterialSolidPerc24 = float.Parse(data[i].Replace(".", ",")); break;
                    case 588: production.MaterialLiquidPerc24 = float.Parse(data[i].Replace(".", ",")); break;
                    case 589: production.MaterialRecValue24 = float.Parse(data[i].Replace(".", ",")); break;
                    case 590: production.MaterialDen24 = float.Parse(data[i].Replace(".", ",")); break;
                    case 591: production.MaterialVolume24 = float.Parse(data[i].Replace(".", ",")); break;
                    case 592: production.MaterialDrySet24 = float.Parse(data[i].Replace(".", ",")); break;
                    case 593: production.MaterialWetSet24 = float.Parse(data[i].Replace(".", ",")); break;
                    case 594: production.MaterialNote24 = data[i];break;
                    case 595: production.MaterialSetId24 = data[i];break;
                    case 596: production.MaterialId25 = Int32.TryParse(data[i], out int k68) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 597: production.MaterialName25 = data[i]; break;
                    case 598: production.MaterialAmount25 = float.Parse(data[i].Replace(".", ",")); break;
                    case 599: production.MaterialSet25 = float.Parse(data[i].Replace(".", ",")); break;
                    case 600: production.MaterialManual25 = float.Parse(data[i].Replace(".", ",")); break;
                    case 601: production.MaterialWetPerc25 = float.Parse(data[i].Replace(".", ",")); break;
                    case 602: production.MaterialWet25 = float.Parse(data[i].Replace(".", ",")); break;
                    case 603: production.MaterialAbsPerc25 = float.Parse(data[i].Replace(".", ",")); break;
                    case 604: production.MaterialAbs25 = float.Parse(data[i].Replace(".", ",")); break;
                    case 605: production.MaterialCementPart25 = float.Parse(data[i].Replace(".", ",")); break;
                    case 606: production.MaterialFiller25 = Int32.TryParse(data[i], out int k69) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 607: production.MaterialKValue25 = float.Parse(data[i].Replace(".", ",")); break;
                    case 608: production.MaterialInhibitor25 = Int32.TryParse(data[i], out int k70) ? Convert.ToInt32(data[i]) : (int?)null; break;
                    case 609: production.MaterialSolidPerc25 = float.Parse(data[i].Replace(".", ",")); break;
                    case 610: production.MaterialLiquidPerc25 = float.Parse(data[i].Replace(".", ",")); break;
                    case 611: production.MaterialRecValue25 = float.Parse(data[i].Replace(".", ",")); break;
                    case 612: production.MaterialDen25 = float.Parse(data[i].Replace(".", ",")); break;
                    case 613: production.MaterialVolume25 = float.Parse(data[i].Replace(".", ",")); break;
                    case 614: production.MaterialDrySet25 = float.Parse(data[i].Replace(".", ",")); break;
                    case 615: production.MaterialWetSet25 = float.Parse(data[i].Replace(".", ",")); break;
                    case 616: production.MaterialNote25 = data[i]; break;
                    case 617: production.MaterialSetId25 = data[i]; break;

                }
                
                columnNumber++;
            }
            return production;
        }

        private void SaveData(List<BSU_Production> insertData)
        {
            try
            {
                LOG_ZAVOD_NFEntities _context = new LOG_ZAVOD_NFEntities();
                Scope.BulkInsert(insertData, new SqlConnection(_context.Database.Connection.ConnectionString), "BSU_Production");


                messageBSUPrud = "BSU_Production. Время: " + Convert.ToString(insertData[insertData.Count - 1].Date.ToShortDateString() + " " + insertData[insertData.Count - 1].Time.ToString());


            }
            catch (Exception ex)
            {
                Scope.WriteError(" SaveChange in BSU_Productions -> " + ex.Message);
            }

            try
            {
                SendTextOnActiveForm(messageBSUPrud, Form.ActiveForm);                
            }
            catch (Exception)
            {

            }

            bSU_Productions.Clear();
        }

        private void SendTextOnActiveForm(string msg, Form form)
        {
            Form activeForm = form;
            if (activeForm.InvokeRequired)
            {
                activeForm.Invoke(senderText, msg, form);
                return;
            }
            if (msg != null)
            {
                lastEntry.Message = msg;
            }
        }
        private void SendStateOnActiveForm(string state, Form form)
        {
            Form activeForm = form;
            if (activeForm.InvokeRequired)
            {
                activeForm.Invoke(senderState, state, form);
                activeForm.Invoke(new Action(() => { }));
                return;
            }
            if (state != null)
            {
                stateModule.Message = state;
            }
        }
    }
}

