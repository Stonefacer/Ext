using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ext.Workers {

    public class UrlProcessedEventArgs: EventArgs {

        public string URL { get; private set; }
        public string State { get; private set; }
        public int Delay { get; private set; }

        public UrlProcessedEventArgs(string URL, string State, int Delay) {
            this.URL = URL;
            this.State = State;
            this.Delay = Delay;
        }

    }
}
