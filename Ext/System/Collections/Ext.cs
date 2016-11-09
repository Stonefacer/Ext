using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ext.System.Collections {
    public static class Ext {
        public static void PushBack(this BitArray src, int Num, int BitsCount = 32) {
            while(BitsCount-- > 0) {
                src.Set(src.Count, (Num & 1) == 1);
                Num >>= 1;
            }
        }
    }
}
