using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
#if NET40
using System.Numerics;
using System.Threading.Tasks;
#endif
using System.IO;

using Ext.System.Core;
using Ext.System.Security;

namespace Ext.CommonForms {

    public partial class frmKeyDialog : Form {

#if NET40

        private DialogResult result = DialogResult.Cancel;

        public frmKeyDialog() {
            InitializeComponent();
        }

        public static DialogResult GetKeys(ref BigInteger E, ref BigInteger D, ref BigInteger N) {
            using(var frm = new frmKeyDialog()) {
                frm.ShowDialog();
                if(frm.result == DialogResult.OK) {
                    E = BigInteger.Parse(frm.txtE.Text);
                    D = BigInteger.Parse(frm.txtD.Text);
                    N = BigInteger.Parse(frm.txtN.Text);
                }
                return frm.result;
            }
        }

        public static DialogResult GetKeys(ref advRSA rsa) {
            using(var frm = new frmKeyDialog()) {
                frm.ShowDialog();
                if(frm.result == DialogResult.OK) {
                    rsa.E = BigInteger.Parse(frm.txtE.Text);
                    rsa.D = BigInteger.Parse(frm.txtD.Text);
                    rsa.N = BigInteger.Parse(frm.txtN.Text);
                }
                return frm.result;
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            result = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) {
            result = DialogResult.Cancel;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e) {
            using(OpenFileDialog ofd = new OpenFileDialog() {
                Filter = "xml|*.xml"
            }) {
                if(ofd.ShowDialog() == DialogResult.Cancel)
                    return;
                try {
                    var rsa = new advRSA();
                    using(var sr = new StreamReader(ofd.FileName))
                        rsa.FromXmlString(sr.ReadToEnd());
                    txtE.Text = rsa.E.ToString();
                    txtD.Text = rsa.D.ToString();
                    txtN.Text = rsa.N.ToString();
                } catch(FileNotFoundException) {
                    MessageBox.Show("File not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch(Exception ex) {
                    MessageBox.Show("File is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ex.Log();
                }
            }
        }

        private void KeyDialog_Load(object sender, EventArgs e) {

        }
#endif
    }
}
