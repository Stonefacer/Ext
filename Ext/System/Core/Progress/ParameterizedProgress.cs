using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ext.System.Core.Progress {
    public class ParameterizedProgress<T> : DefaultProgress, IParameterizedProgress<T> {
        public T Value { get; set; }
    }
}
