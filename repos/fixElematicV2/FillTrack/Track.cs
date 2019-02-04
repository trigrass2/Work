using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FillTrack
{
    public class Track
    {
        public int LengthTrack = 100000;
        public string TypeNode;
        public List<Plate> Plates;

        public Track( string typeNode, int lengthTrack)
        {
            TypeNode = typeNode;
            LengthTrack = lengthTrack;
            Plates = new List<Plate>();
        }
        public Track(int lengthTrack, string typeNode, List<Plate> plates)
        {
            LengthTrack = lengthTrack;
            TypeNode = typeNode;
            Plates = plates;
        }
    }
}
