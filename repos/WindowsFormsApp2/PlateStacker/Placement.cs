using System.Collections.Generic;

namespace PlateStacker
{
    public static class Placement
    {
        public static List<Track> GetResult(List<Plate> plates, int lengthTrack)
        {
            Controller cntr = new Controller(lengthTrack);
            List<string> countTypeNode = new List<string>();
            List<Track> sortTracks = new List<Track>();
            List<Track> listFinishTracks = new List<Track>();

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
                sortTracks.Add(new Track(countTypeNode[i], lengthTrack));
            }

            cntr.GetSortTypeNodePlate(plates, sortTracks);

            cntr.PlatesHandler(sortTracks, listFinishTracks);

            return listFinishTracks;
        }
    }
}
