using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ext.Data {
    public static class ExtBitConverter {
        public static int GetBitsCount(int Num) {
            int res = 1;
            Num >>= 1;
            while(Num != 0) {
                Num >>= 1;
                res++;
            }
            return res;
        }

        public static int GetBitsCount(long Num) {
            int res = 1;
            Num >>= 1;
            while(Num != 0) {
                Num >>= 1;
                res++;
            }
            return res;
        }

    }
}
