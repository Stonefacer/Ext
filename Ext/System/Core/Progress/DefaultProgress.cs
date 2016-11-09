using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ext.System.Core.Progress {
    public class DefaultProgress : IProgressExt {

        protected int _done;
        protected int _total;

        public int OperationsDone {
            get{
                return _done;
            }
            set{
                if(_total == value)
                    return;
                _done = value;
                OnProgressChanged();
            }
        }

        public int OperationsTotal{
            get{
                return _total;
            }
            set{
                if(_total == value)
                    return;
                _total = value;
                OnProgressChanged();
            }
        }

        public int Percents {
            get{
                return (int)Math.Floor(PercentsAccurately);
            }
        }

        public float PercentsAccurately{
            get{
                if(_total == 0)
                    return 0f;
                return _done * 100.0f / _total;
            }
        }

        public event StateChanged ProgressChanged;

        protected virtual void OnProgressChanged() {
            ProgressChanged?.Invoke(this, new EventArgs());
        }
    }
}
