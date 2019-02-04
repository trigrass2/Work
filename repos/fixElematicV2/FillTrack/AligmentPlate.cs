using System;

namespace FillTrack
{
    [Serializable]
    public class AligmentPlate
    {
        private string _name;
        private string _typeNode;
        private string _listPlate;
        private int _countPlate;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string TypeNode
        {
            get { return _typeNode; }
            set { _typeNode = value; }
        }

        public int CountPlate
        {
            get { return _countPlate; }
            set { _countPlate = value; }
        }

        public string ListPlate
        {
            get { return _listPlate; }
            set { _listPlate = value; }
        }



        public AligmentPlate(string name, string typeNode, int countPlate, string listPlate)
        {
            _name = name;
            _typeNode = typeNode;
            _countPlate = countPlate;
            _listPlate = listPlate;
            
        }
        public AligmentPlate()
        {

        }


    }
}
