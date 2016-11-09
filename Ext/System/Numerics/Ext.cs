using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if (NET40 || NET45) && EXT_BIGINT
using System.Numerics;
#endif



namespace Ext.System.Numerics {
    public static class Ext {

#if (NET40 || NET45) && EXT_BIGINT

        public static int GetBitCount(this BigInteger src) {
            return src.ToByteArray().Length * 8;
        }

        public static int GetByteCount(this BigInteger src) {
            return src.ToByteArray().Length;
        }
#endif

    }

}
