using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Ext.System.Drawing {
    class ColorComparator : IComparer<Color> {

        public int Compare(Color x, Color y) {
            var resX = (int)(ColorAndImageFactory.GetColorRangeQ(Color.White, x) * 1000.0);
            var resY = (int)(ColorAndImageFactory.GetColorRangeQ(Color.White, y) * 1000.0);
            return resX - resY;
        }

    }
}
