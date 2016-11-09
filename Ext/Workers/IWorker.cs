#if (NET40 || NET45) && EXT_BIGINT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Ext.System.Net;
using Ext.Data;

namespace Ext.Workers {
    public interface IWorker {
        string State { get; }
        string Version { get; set; }
        string Name { get; }
        event UrlProcessedInvoker UrlProcessed;
        Exception Prepare(IDataSaver DataSaver, IWorkerState State, DefaultSettings Settings);
        bool RunNext(WebPagesDownloader Client);
        Exception Finalize();
    }
}
#endif

