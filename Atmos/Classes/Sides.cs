using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmos.Classes
{
    public class Sides
    {
        public bool Top { get; set; } = true;
        public int TopLEDCount { get; set; }
        public bool Right { get; set; } = true;
        public int RightLEDCount { get; set; }
        public bool Bottom { get; set; } = true;
        public int BottomLEDCount { get; set; }
        public bool Left { get; set; } = true;
        public int LeftLEDCount { get; set; }
    }
}
