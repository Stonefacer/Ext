
using Sys = System;

namespace Ext.CommonForms {
#if NET40
    partial class frmKeyDialog {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Sys.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

#region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.label1 = new Sys.Windows.Forms.Label();
            this.label2 = new Sys.Windows.Forms.Label();
            this.txtE = new Sys.Windows.Forms.TextBox();
            this.txtN = new Sys.Windows.Forms.TextBox();
            this.button1 = new Sys.Windows.Forms.Button();
            this.button2 = new Sys.Windows.Forms.Button();
            this.txtD = new Sys.Windows.Forms.TextBox();
            this.label3 = new Sys.Windows.Forms.Label();
            this.button3 = new Sys.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Sys.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new Sys.Drawing.Size(14, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "E";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Sys.Drawing.Point(12, 61);
            this.label2.Name = "label2";
            this.label2.Size = new Sys.Drawing.Size(15, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "N";
            // 
            // txtE
            // 
            this.txtE.Location = new Sys.Drawing.Point(33, 6);
            this.txtE.Name = "txtE";
            this.txtE.Size = new Sys.Drawing.Size(643, 20);
            this.txtE.TabIndex = 1;
            // 
            // txtN
            // 
            this.txtN.Location = new Sys.Drawing.Point(33, 58);
            this.txtN.Name = "txtN";
            this.txtN.Size = new Sys.Drawing.Size(643, 20);
            this.txtN.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new Sys.Drawing.Point(520, 84);
            this.button1.Name = "button1";
            this.button1.Size = new Sys.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new Sys.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new Sys.Drawing.Point(601, 84);
            this.button2.Name = "button2";
            this.button2.Size = new Sys.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new Sys.EventHandler(this.button2_Click);
            // 
            // txtD
            // 
            this.txtD.Location = new Sys.Drawing.Point(33, 32);
            this.txtD.Name = "txtD";
            this.txtD.Size = new Sys.Drawing.Size(643, 20);
            this.txtD.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new Sys.Drawing.Point(12, 35);
            this.label3.Name = "label3";
            this.label3.Size = new Sys.Drawing.Size(15, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "D";
            // 
            // button3
            // 
            this.button3.Location = new Sys.Drawing.Point(33, 84);
            this.button3.Name = "button3";
            this.button3.Size = new Sys.Drawing.Size(131, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Load from XML file";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new Sys.EventHandler(this.button3_Click);
            // 
            // KeyDialog
            // 
            this.AutoScaleDimensions = new Sys.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = Sys.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new Sys.Drawing.Size(688, 114);
            this.Controls.Add(this.txtD);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtN);
            this.Controls.Add(this.txtE);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = Sys.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeyDialog";
            this.StartPosition = Sys.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "KeyDialog";
            this.TopMost = true;
            this.Load += new Sys.EventHandler(this.KeyDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

#endregion

        private Sys.Windows.Forms.Label label1;
        private Sys.Windows.Forms.Label label2;
        private Sys.Windows.Forms.TextBox txtE;
        private Sys.Windows.Forms.TextBox txtN;
        private Sys.Windows.Forms.Button button1;
        private Sys.Windows.Forms.Button button2;
        private Sys.Windows.Forms.TextBox txtD;
        private Sys.Windows.Forms.Label label3;
        private Sys.Windows.Forms.Button button3;
    }
#endif
}