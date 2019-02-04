using System.Collections.Generic;

namespace Track
{
    public class Tracks
    {
        public int LengthTrack;
        public string TypeNode;
        public List<Plate> Plates;

        public Tracks(string typeNode, int lengthTrack)
        {
            TypeNode = typeNode;
            LengthTrack = lengthTrack;
            Plates = new List<Plate>();
        }
        public Tracks(int lengthTrack, string typeNode, List<Plate> plates)
        {
            LengthTrack = lengthTrack;
            TypeNode = typeNode;
            Plates = plates;
        }
    }
}
