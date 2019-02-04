using System;

namespace PlateStacker
{
    [Serializable]
    public class Rope
    {
        public int Diameter { get; set; }
        public string Location { get; set; }
        public int PositionNumber { get; set; }   
    }
}
