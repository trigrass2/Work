using System;
using System.Collections.Generic;

namespace FillTrack
{
    [Serializable]
    public class Plate
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Length { get; set; }
        public string TypeNode { get; set; }
        public string Concrete { get; set; }
        public string Loop { get; set; }
        public List<List<Rope>> AllRopes { get; set; }


        public Plate(string _name, int _length, int _width, int _height, string _typeNode, string _concrete, string _loop, List<List<Rope>> _allRopes)
        {
            Name = _name;
            Length = _length;
            Width = _width;
            TypeNode = _typeNode;
            AllRopes = _allRopes;
            Height = _height;
            Concrete = _concrete;
            Loop = _loop;
        }
        public Plate()
        {
            
        }

    }
}
