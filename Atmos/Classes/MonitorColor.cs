using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmos.Classes
{
    public enum AmbietMode
    {
        Average,
        Exact
    }

    public class MonitorColor
    {
        private Point _location;
        private Size _size;
        private AmbietMode _mode;
        private int _ledCount;

        public MonitorColor(Point location, Size size, int ledCount, AmbietMode mode = AmbietMode.Average)
        {
            _location = location;
            _size = size;
            _ledCount = ledCount;
            _mode = mode;
        }

        public Task<Color[]> GetColors()
        {

        }
    }
}
