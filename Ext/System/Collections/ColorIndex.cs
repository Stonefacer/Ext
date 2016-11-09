using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Ext.System.Drawing;

namespace Ext.System.Collections {
    public class ColorIndex {

        private List<Color> _Colors = new List<Color>();

        public int Count { get { return _Colors.Count; } }

        public ColorIndex() {

        }

        public void Add(Color cl) {
            if(!_Colors.Contains(cl))
                _Colors.Add(cl);
        }

        public void Trim(int maxCount) {
            _Colors.Sort(new ColorComparator());
            while(Count > maxCount)
                TrimSameColors();
        }

        public int GetId(Color cl) {
            return MinimalRange(cl);
        }

        public Color GetColor(int id) {
            if(id < 0 || id >= _Colors.Count)
                return Color.Azure;
            return _Colors[id];
        }

        private void TrimSameColors() {
            int c0 = 0;
            int c1 = 0;
            MinimalRange(out c0, out c1);
            int r = (_Colors[c0].R + _Colors[c1].R) >> 1;
            int g = (_Colors[c0].G + _Colors[c1].G) >> 1;
            int b = (_Colors[c0].B + _Colors[c1].B) >> 1;
            _Colors[c0] = Color.FromArgb(r, g, b);
            _Colors.RemoveAt(c1); // 
        }

        private int MinimalRange(Color cl) {
            double res = ColorAndImageFactory.GetColorRangeQ(cl, _Colors[0]);
            int resId = -1;
            for(int i = 1; i < _Colors.Count; i++) {
                var dif = ColorAndImageFactory.GetColorRangeQ(cl, _Colors[i]);
                if(dif < res) {
                    res = dif;
                    resId = i;
                }
            }
            return resId;
        }

        private double MinimalRange(out int color0, out int color1) {
            double res = double.MaxValue;
            color0 = 0;
            color1 = 0;
            for(int i = 0; i < _Colors.Count - 1; i++) {
                double range = ColorAndImageFactory.GetColorRangeQ(_Colors[i], _Colors[i+1]);
                if(range < res) {
                    res = range;
                    color0 = i;
                    color1 = i+1;
                }
            }
            return res;
        }

        public static ColorIndex Create(Bitmap bmp) {
            ColorIndex ci = new ColorIndex();
            for(int i = 0; i < bmp.Height; i++) {
                for(int j = 0; j < bmp.Width; j++) {
                    ci.Add(bmp.GetPixel(j, i));
                }
            }
            return ci;
        }

    }
}
