using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if NET45
using System.Threading.Tasks;
#endif

namespace Ext.System.Threading.Tasks {
    public static class Extentions {
#if NET45
        public static bool IsStarted(this Task ts) {
            return ts.Status != TaskStatus.Created;
        }
#endif
    }
}
