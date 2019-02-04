using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FillTrack
{
    public class Controller
    {
        public int bestCountRope = 0;
        public int lengthTrack = 100000;
        public List<Plate> bestPlate = null;

        public Controller(int lengthTrack)
        {
            this.lengthTrack = lengthTrack;
        }

        private bool RopeComparisonSame(Plate firstPlate, Plate lastPlate)
        {
            if(firstPlate.AllRopes.Count != lastPlate.AllRopes.Count)
            {
                return false;
            }
            int count = firstPlate.AllRopes.Count;
            

            for(int i = 0; i < count; i++)
            {
                if( firstPlate.AllRopes[i] == lastPlate.AllRopes[i])
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private bool RopeComparison(Plate firstPlate, Plate lastPlate)
        {
            if (firstPlate.AllRopes.Count != lastPlate.AllRopes.Count)
            {
                return false;
            }
            int count = firstPlate.AllRopes.Count;

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < firstPlate.AllRopes[i].Count; j++)
                {
                    if (firstPlate.AllRopes[i].Count >= lastPlate.AllRopes[i].Count && firstPlate.AllRopes[i][j].Location == lastPlate.AllRopes[i][j].Location && firstPlate.AllRopes[i][j].PositionNumber == lastPlate.AllRopes[i][j].PositionNumber)
                    {
                        if (firstPlate.AllRopes[i][j].Diameter >= lastPlate.AllRopes[i][j].Diameter) {
                            if (j == lastPlate.AllRopes[i].Count) { return true; }
                            continue;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public List<Plate> CheckSet(List<Plate> plates)
        {
            plates = SortPlatesByLength(plates);

            plates = SortPlatesByCountRope(plates);

            bestPlate = new List<Plate>();
            bestPlate.Add(plates[0]);

            if (CalcLengthPlate(bestPlate) < lengthTrack)
            {
                for (int i = 1; i < plates.Count; i++)
                {
                    if (RopeComparisonSame(bestPlate[bestPlate.Count-1], plates[i]) == true && plates[i].AllRopes.Count == bestPlate[bestPlate.Count - 1].AllRopes.Count && plates[i].Height == bestPlate[bestPlate.Count - 1].Height && plates[i].Width == bestPlate[bestPlate.Count - 1].Width && plates[i].Concrete == bestPlate[bestPlate.Count - 1].Concrete)
                    {
                        if (plates[i].Length <= (lengthTrack - CalcLengthPlate(bestPlate)))
                        {
                            bestPlate.Add(plates[i]);
                            
                        }
                    }
                }

                for (int i = 0; i < plates.Count; i++)
                {
                    if (RopeComparison(bestPlate[bestPlate.Count - 1], plates[i]) == true && plates[i].AllRopes.Count == bestPlate[bestPlate.Count - 1].AllRopes.Count && plates[i].Height == bestPlate[bestPlate.Count - 1].Height && plates[i].Width == bestPlate[bestPlate.Count - 1].Width && plates[i].Concrete == bestPlate[bestPlate.Count - 1].Concrete)
                    {
                        if (plates[i].Length <= (lengthTrack - CalcLengthPlate(bestPlate)))
                        {
                            if (!bestPlate.Contains(plates[i]))
                            {
                                bestPlate.Add(plates[i]);
                            }
                        }
                    }
                }
                
            }

            return bestPlate;
        }                
        public int CalcLengthPlate(List<Plate> plates)
        {
            int sumL = 0;

            for (int i = 0; i < plates.Count; i++)
            {
                sumL += plates[i].Length;
            }

            return sumL;
        }       
        public void GetSortTypeNodePlate(List<Plate> plates, List<Track> tracks)
        {   foreach (Track t in tracks)
            {
                t.Plates = plates.FindAll(x => x.TypeNode == t.TypeNode);
            }           
        }
        #region Сортировка плит
        private List<Plate> SortPlatesByLength(List<Plate> plates)
        {       
            var sortedPlates = from p in plates
                               orderby p.Length descending
                               select p;

            List<Plate> tempPlates = new List<Plate>(sortedPlates);

            return tempPlates;
        }
        private List<Plate> SortPlatesByCountRope(List<Plate> plates)
        {
            var sortedPlates = from p in plates
                               orderby p.AllRopes.Count descending
                               select p;

            List<Plate> tempPlates = new List<Plate>(sortedPlates);

            return tempPlates;
        }
        #endregion
    }
}
