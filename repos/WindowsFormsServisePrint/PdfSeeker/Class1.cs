using System;
using System.IO;
using System.Security.Permissions;

namespace PdfSeeker
{
    public class Seeker
    {
        static Logger _logger;
        static string _printerName;
        static string _deleteYesNo;
        static string _path;
        FileSystemWatcher watcher;

        public Seeker(string path, string printer, string deleteYesNo)
        {
            _printerName = printer;
            _deleteYesNo = deleteYesNo;
            _path = path;
            watcher = new FileSystemWatcher();
        }

        public void Start()
        {
            Run(_path, _printerName, _deleteYesNo);
        }
        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
            
        }
        /// <summary>
        /// Запускает сервис
        /// </summary>
        /// <param name="path">папка для отслеживания файлов на печать</param>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void Run(string path, string printer, string deleteOrNotDelete)
        {
            _printerName = printer;
            _deleteYesNo = deleteOrNotDelete;
            _logger = new Logger();
            
            watcher.Path = path;
           
            watcher.NotifyFilter = NotifyFilters.LastAccess 
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.FileName 
                                 | NotifyFilters.DirectoryName;
           
            watcher.Filter = "*.pdf";

            watcher.Created += new FileSystemEventHandler(OnChanged);
            
            watcher.EnableRaisingEvents = true;

        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            
            SilentPrint(_printerName, e.FullPath);
            if(_deleteYesNo == "Yes") File.Delete(e.FullPath);
            _logger.Write($"Print {e.FullPath}");
        }
        
        private static void SilentPrint(string printerName, string filename)
        {
            #region spirepdf
            using (var pdfDoc = new Spire.Pdf.PdfDocument())
            {
                pdfDoc.LoadFromFile(filename);
                pdfDoc.PrinterName = printerName;
                pdfDoc.PrintDocument.Print();
            }
            #endregion
            
        }
    }
}
