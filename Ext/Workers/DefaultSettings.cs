#if (NET40 || NET45) && EXT_BIGINT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading.Tasks;

using Ext.System.Core;

namespace Ext.Workers {
    public class DefaultSettings:SignedFileBase {
        public int Delay { get; set; }
        public int Repeats { get; set; }
        public int PagesTotal { get { return URLs.Count; } }
        public WebProxy Proxy { get; set; }
        public string OutputFilePath { get; set; }
        public string OutputFormat { get; set; }
        public string Filter { get; set; }
        public string Format { get; set; }
        public ushort Threads { get; set; }
        public List<string> URLs { get; set; } = new List<string>();
        public Dictionary<string, string> MoreSettings = new Dictionary<string, string>();

        protected override void LoadData(MemoryStream MemStream) {
            using(var Reader = new StreamReader(MemStream, Encoding.UTF8)) {
                Delay = Reader.ReadLine().ToInt(1000);
                Repeats = Reader.ReadLine().ToInt(2);
                Proxy = Reader.ReadLine().ToWebProxy();
                OutputFilePath = Reader.ReadLine();
                OutputFormat = Reader.ReadLine();
                Filter = Reader.ReadLine();
                Format = Reader.ReadLine();
                Threads = (ushort)Reader.ReadLine().ToInt();
                var MoreStg = Reader.ReadLine().ToInt();
                while(MoreStg-- > 0) {
                    var data = Reader.ReadLine().Split('|');
                    MoreSettings[data[0]] = data[1];
                }
                int urls = Reader.ReadLine().ToInt(0);
                URLs.Clear();
                for(int i = 0; i < urls && !Reader.EndOfStream; i++)
                    URLs.Add(Reader.ReadLine());
            }
        }

        protected override byte[] GetSaveData() {
            StringBuilder Result = new StringBuilder();
            Result.AppendLine(Delay.ToString());
            Result.AppendLine(Repeats.ToString());
            Result.AppendLine(Proxy?.ToString() ?? "");
            Result.AppendLine(OutputFilePath);
            Result.AppendLine(OutputFormat);
            Result.AppendLine(Filter);
            Result.AppendLine(Format);
            Result.AppendLine(Threads.ToString());
            Result.AppendLine(MoreSettings.Count.ToString());
            foreach(var v in MoreSettings)
                Result.AppendLine(string.Format("{0}|{1}", v.Key, v.Value));
            Result.AppendLine(URLs.Count.ToString());
            foreach(var v in URLs)
                Result.AppendLine(v);
            return Encoding.UTF8.GetBytes(Result.ToString());
        }
    }
}
#endif
