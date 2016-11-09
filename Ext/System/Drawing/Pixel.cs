using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Ext.System.Drawing {
    public struct Pixel {
        public Color Color;
        public int X;
        public int Y;
        public Pixel(Color color, int x, int y) {
            Color = color;
            X = x;
            Y = y;
        }
    }
}
