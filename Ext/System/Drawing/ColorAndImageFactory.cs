using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Ext.System.Drawing {
    public class ColorAndImageFactory {
        public static double ToGrayscale(Color cl) {
            return cl.R * 0.2126 + cl.G * 0.7152 + cl.B * 0.0722;
        }

        public static double[] ConvertToArray(Bitmap bmp) {
            var Result = new double[bmp.Width * bmp.Height];
            var Min = double.MaxValue;
            var Max = double.MinValue;
            for(int i = 0; i < bmp.Height; i++) {
                for(int j = 0; j < bmp.Width; j++) {
                    var val = ToGrayscale(bmp.GetPixel(j, i));
                    Result[i * bmp.Width + j] = val;
                    if(val < Min)
                        Min = val;
                    if(val > Max)
                        Max = val;
                }
            }
            for(int i = 0; i < Result.Length; i++) {
                Result[i] = (Max - Result[i]) / (Max - Min);
            }
            return Result;
        }

        public static double GetColorRangeQ(Color c1, Color c2) {
            double res = 1;
            res *= Math.Abs(c1.R - c2.R);
            res *= Math.Abs(c1.G - c2.G);
            res *= Math.Abs(c1.B - c2.B);
            return Math.Pow(res, 1.0/3.0)/255.0;
        }

        public static ImageCodecInfo GetEncoderInfo(String mimeType) {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for(j = 0; j < encoders.Length; ++j) {
                if(encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        public static Color GetMedianFilter(params Color[] pixles) {
            Array.Sort(pixles, (a, b) => a.R + a.G + a.B - (b.R + b.G + b.B));
            return pixles[pixles.Length >> 1];
        }

    }
}
