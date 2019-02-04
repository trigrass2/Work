using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Reports
{
    public class PlanFactPlantReportsView
    {
        public DateTime ReportDateTime { get; set; }
        public int ReportPlan1 { get; set; }
        public int ReportFact2 { get; set; }
        public int ReportPlan3 { get; set; }
        public int ReportFact4 { get; set; }
        public int ReportPlan5 { get; set; }

        public int Plant_id { get; set; }
        public string Plant_name { get; set; }
        public int Factory_id { get; set; }
        public int? Problem_id { get; set; }
        public string Problem_name { get; set; }
    }
}
