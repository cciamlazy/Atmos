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
        #region Variables
        private Point _location;
        public Point Location { get { return _location; } set { _location = value; } }

        private Size _size;
        public Size Size { get { return _size; } set { _size = value; } }

        private Sides _sides;
        public Sides Sides { get { return _sides; } set { _sides = value; } }

        private AmbietMode _mode;
        public AmbietMode Mode { get { return _mode; } set { _mode = value; } }

        private int _ledCount;
        public int LEDCount { get { return _ledCount; } }

        private LED[] _leds;
        public LED[] LEDs { get { return _leds; } set { if(_leds.Length == value.Length) _leds = value; } }
        #endregion

        public MonitorColor(Point location, Size size, Sides sides, int ledCount, AmbietMode mode = AmbietMode.Average)
        {
            _location = location;
            _size = size;
            _sides = sides;
            _ledCount = ledCount;
            _mode = mode;
            _leds = new LED[ledCount];
        }

        public async Task<bool> Update()
        {


            return true;
        }
    }
}
