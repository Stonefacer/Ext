using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Ext.Data {
    public class BitSaver : IDataSaver {

        private FileStream _Writer = null;
        private Object _SyncObject = new Object();

        public FileStream OutputStream { get { return _Writer; } }

        public BitSaver(string FileName) {
            _Writer = new FileStream(FileName, FileMode.Create);
        }

        public void AddData(long Num) {
            lock (_SyncObject)
                _Writer.WriteByte((byte)(Num & 0xff));
        }

        public void AddData(string Format, params string[] args) {
            throw new NotImplementedException();
        }

        public void Dispose() {
            _Writer.Close();
            _Writer.Dispose();
            _Writer = null;
            GC.SuppressFinalize(this);
        }

        public void SetSettings(string Settings) {
            
        }
    }
}
