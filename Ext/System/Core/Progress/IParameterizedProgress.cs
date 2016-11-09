using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ext.System.Core.Progress {
    public interface IParameterizedProgress<T> : IProgressExt {
        T Value { get; }
    }
}
