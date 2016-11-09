#if (NET40 || NET45) && EXT_BIGINT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ext.Data;
using Ext.System.Net;

namespace Ext.Workers {

    public abstract class WorkerBase : IWorker {

        protected IWorkerState _State = null;
        protected DefaultSettings _Settings = null;
        protected IDataSaver _DataSaver = null;

        public String Name { get; protected set; }
        public String State { get; protected set; }
        public String Version { get; set; }

        public event UrlProcessedInvoker UrlProcessed;

        public virtual Exception Finalize() {
            return null;
        }
        public virtual Exception Prepare(IDataSaver DataSaver, IWorkerState State, DefaultSettings Settings) {
            try {
                _Settings = Settings;
                _DataSaver = DataSaver;
                _State = State;
                return null;
            } catch(Exception ex) {
                return ex;
            }
        }

        public abstract bool RunNext(WebPagesDownloader Client);

        protected virtual void CallUrlProcessed(string Url, string State, int Delay) {
            if(UrlProcessed != null)
                UrlProcessed(this, new UrlProcessedEventArgs(Url, State, Delay));
        }
    }
}
#endif
