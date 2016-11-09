using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
#if NET45
using System.Threading.Tasks;
#endif
using System.Windows.Forms;

using Ext.System.Threading.Tasks;

namespace Ext.CommonForms {

    public partial class frmLoading<T> : Form where T : class {

#if !NET45
        public class LoadingTask { }
#else
        public class LoadingTask {
            public string Text { get; private set; }
            public string FiledText { get; private set; }
            public Task<T> Task { get; private set; }
            public T Result = null;

            public LoadingTask(Task<T> task, string text, string FiledText) {
                Task = task;
                Text = text;
                this.FiledText = FiledText;
            }

        }

        private static LoadingTask[] _TasksToExecute;

        public static LoadingTask Failed { get; private set; }

        public static void ShowDialog(Image image, params LoadingTask[] Tasks) {
            Failed = null;
            using(frmLoading<T> frm = new frmLoading<T>()) {
                frm.pictureBox1.Image = image;
                frm.label1.Text = Tasks[0].Text;
                _TasksToExecute = Tasks;
                frm.ShowDialog();
            }
        }

        private frmLoading() {
            InitializeComponent();
        }

        private async void frmLoading_Shown(Object sender, EventArgs e) {
            foreach(var ts in _TasksToExecute) {
                this.label1.Text = ts.Text;
                if(ts.Task.IsStarted())
                    continue;
                ts.Task.Start();
                ts.Result = await ts.Task;
                if(ts.Result != null) {
                    Failed = ts;
                    break;
                }
            }
            this.Close();
        }

        private void frmLoading_Load(object sender, EventArgs e) {

        }
#endif
    }
}
