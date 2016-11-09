using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
#if NET45
using System.Threading.Tasks;
#endif

namespace Ext.Workers {
    public class DefaultWorkerState : IWorkerState {

        private Object _SyncObj = new Object();
        private int _LastURLIndex = 0;

        public int BytesDownloaded { get; set; }
        public int Delay { get; set; }
        public int Error404 { get; set; }
        public int Errors { get; set; }
        public int PagesDownloaded { get; set; }
        public int PagesTotal { get; set; }
        public int PrevBytesDownloaded { get; set; }
        public WebProxy Proxy { get; set; }
        public int Repeats { get; set; }
        public int Rows { get; set; }
        public List<string> URLs { get; set; }

        public void Reset() {
            PagesDownloaded = 0;
            BytesDownloaded = 0;
            Errors = 0;
            Rows = 0;
            Error404 = 0;
            _LastURLIndex = 0;
        }

        public string GetNext() {
            lock (_SyncObj) {
                if(_LastURLIndex >= URLs.Count)
                    return null;
                var res = URLs[_LastURLIndex++];
                return res;
            }
        }
    }
}
