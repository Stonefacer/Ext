using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ext.System.Core.Progress {
    public interface IProgressExt {
        int OperationsTotal { get; }
        int OperationsDone { get; }
        int Percents { get; }
        float PercentsAccurately { get; }
        event StateChanged ProgressChanged;
    }
}
