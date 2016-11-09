#if (NET40 || NET45) && EXT_BIGINT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Web;
using System.Threading;
using System.Net;

using Ext.System.Core;
using Ext.Workers;

namespace Ext.System.Net {

    public class WebPagesDownloader {

        private Object _SyncObject = new Object();
        private IWorkerState _State = null;
        private DefaultSettings _Settings = null;
        private WebClient _WebClient = new WebClient();

        public int LastResponseDelay { get; private set; }
        public string LastState { get; private set; }

        public WebClient Client { get { return _WebClient; } }

        public WebPagesDownloader(IWorkerState State, DefaultSettings Settings) {
            _State = State;
            _Settings = Settings;
            _WebClient.Proxy = _Settings.Proxy;
            _WebClient.Encoding = Encoding.UTF8;
            LastState = "";
        }

        public string DownloadString(string URL) {
            string res = "";
            var s = DateTime.Now.Ticks;
            bool Success = false;
            int tries = 0;
            while((!Success) && (tries < _Settings.Repeats)) {
                tries++;
                try {
                    res = _WebClient.DownloadString(URL);
                    LastState = "OK";
                    Success = true;
                    Thread.Sleep(_Settings.Delay);
                } catch(WebException we) {
                    Thread.Sleep(_Settings.Delay);
                    if(tries < _Settings.Repeats)
                        continue;
                    if(we.Message.IndexOf("(404) Not Found") == -1) {
                        _State.Errors++;
                        LastState = we.Message;
                    } else {
                        _State.Error404++;
                        LastState = "404 Not Found";
                    }
                }
            }
            lock (_SyncObject) {
                _State.BytesDownloaded += res.Length;
                _State.PagesDownloaded++;
                LastResponseDelay = (int)((double)(DateTime.Now.Ticks - s) / (double)TimeSpan.TicksPerMillisecond);
            }
            return res;
        }
    }
}
#endif
