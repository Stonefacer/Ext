using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
#if NET45
using System.Threading.Tasks;
#endif

namespace Ext.Workers {
    public interface IWorkerState {
        int PagesDownloaded { get; set; }
        int PagesTotal { get; set; }
        int PrevBytesDownloaded { get; set; }
        int BytesDownloaded { get; set; }
        int Errors { get; set; }
        int Error404 { get; set; }
        int Rows { get; set; }
        int Delay { get; set; }
        int Repeats { get; set; }
        WebProxy Proxy { get; set; }
        List<string> URLs { get; set; }

        void Reset();
        string GetNext();

    }
}
