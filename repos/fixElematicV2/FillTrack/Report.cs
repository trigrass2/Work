using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FillTrack
{
    public class Report
    {
        public string NameTrack { get; set; }
        public string NamePlate { get; set; }
        public int OriginDiameterRope { get; set; }
        public int ThisDiameterRope { get; set; }

        public Report(string nameTrack, string namePlate, int originDiameterRope, int thisDiameterRope)
        {
            NamePlate = namePlate;
            NameTrack = nameTrack;
            OriginDiameterRope = originDiameterRope;
            ThisDiameterRope = thisDiameterRope;
        }
        public Report()
        {

        }
    }
}
