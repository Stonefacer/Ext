using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Ext.Data {
    public class PlainTextDataSaver : IDataSaver {

        private StreamWriter _Writer = null;
        private Object _SyncObject = new Object();

        public PlainTextDataSaver(string FileName) {
            _Writer = new StreamWriter(FileName, false, Encoding.UTF8);
        }

        public void AddData(string Format, params string[] args) {
            lock(_SyncObject)
                _Writer.WriteLine(string.Format(Format, args));
        }

        public void Dispose() {
            _Writer.Close();
            _Writer.Dispose();
            _Writer = null;
            GC.SuppressFinalize(this);
        }

        public void SetSettings(String Settings) {

        }

        public void AddData(long Num) {
            throw new NotImplementedException();
        }
    }
}
