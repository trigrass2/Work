using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Track
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }

        private List<Tracks> GetResult(List<Plate> plates, int lengthTrack) // возвращает заполненные дорожки
        {
            
            List<string> countTypeNode = new List<string>();
            Controller cntr = new Controller(lengthTrack);
            List<Tracks> sortTracks = new List<Tracks>();
            List<Tracks> listFinishTracks = new List<Tracks>();

            foreach (Plate plate in plates)
            {
                if (countTypeNode.Count == 0)
                {
                    countTypeNode.Add(plate.TypeNode);
                }
                else
                {
                    if (!countTypeNode.Contains(plate.TypeNode))
                    {
                        countTypeNode.Add(plate.TypeNode);
                    }
                }
            }
            for (int i = 0; i < countTypeNode.Count; i++)
            {
                sortTracks.Add(new Tracks(countTypeNode[i], lengthTrack));
            }

            cntr.GetSortTypeNodePlate(plates, sortTracks);

            cntr.PlatesHandler(sortTracks, listFinishTracks);

            return listFinishTracks;
        }
        
    }
}
