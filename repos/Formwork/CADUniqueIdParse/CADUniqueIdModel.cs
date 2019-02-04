using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CADUniqueIdParse
{
    public class CADUniqueIdModel
    {
        private string _name;
        private int _width;
        private int _height;
        private int _depth;
        private double _ratioWidth_2000;
        private double _ratioHeight_2000;
        private double _ratioWidth_1690;
        private double _ratioHeight_1690;
        private double _ratiokWidth_1400;
        private double _ratioHeight_1400;
        private double _ratioWidth_1000;
        private double _ratioHeight_1000;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                
            }
        }

        public int Width
        {
            get { return _width; }
            set
            {
                _width = value;
                
            }
        }

        public int Height
        {
            get { return _height; }
            set
            {
                _height = value;

            }
        }

        public int Depth
        {
            get { return _depth; }
            set
            {
                _depth = value;
                
            }
        }

        public double RatioWidth_2000
        {
            get { return _ratioWidth_2000; }
            set
            {
                _ratioWidth_2000 = value;

            }
        }

        public double RatioHeight_2000
        {
            get { return _ratioHeight_2000; }
            set
            {
                _ratioHeight_2000 = value;

            }
        }

        public double RatioWidth_1690
        {
            get { return _ratioWidth_1690; }
            set
            {
                _ratioWidth_1690 = value;

            }
        }

        public double RatioHeight_1690
        {
            get { return _ratioHeight_1690; }
            set
            {
                _ratioHeight_1690 = value;

            }
        }

        public double RatioWidth_1400
        {
            get { return _ratiokWidth_1400; }
            set
            {
                _ratiokWidth_1400 = value;

            }
        }

        public double RatioHeight_1400
        {
            get { return _ratioHeight_1400; }
            set
            {
                _ratioHeight_1400 = value;

            }
        }

        public double RatioWidth_1000
        {
            get { return _ratioWidth_1000; }
            set
            {
                _ratioWidth_1000 = value;

            }
        }

        public double RatioHeight_1000
        {
            get { return _ratioHeight_1000; }
            set
            {
                _ratioHeight_1000 = value;

            }
        }
    }
}
