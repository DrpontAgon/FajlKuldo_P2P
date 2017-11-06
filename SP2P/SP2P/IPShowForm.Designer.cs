namespace SP2P
{
    partial class IPShowForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Yes = new System.Windows.Forms.Button();
            this.No = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Yes
            // 
            this.Yes.Location = new System.Drawing.Point(116, 227);
            this.Yes.Name = "Yes";
            this.Yes.Size = new System.Drawing.Size(75, 23);
            this.Yes.TabIndex = 0;
            this.Yes.Text = "Igen";
            this.Yes.UseVisualStyleBackColor = true;
            this.Yes.Click += new System.EventHandler(this.Yes_Click);
            // 
            // No
            // 
            this.No.Location = new System.Drawing.Point(197, 227);
            this.No.Name = "No";
            this.No.Size = new System.Drawing.Size(75, 23);
            this.No.TabIndex = 0;
            this.No.Text = "Nem";
            this.No.UseVisualStyleBackColor = true;
            this.No.Click += new System.EventHandler(this.No_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(284, 262);
            this.label1.TabIndex = 1;
            this.label1.Text = "El kívánja fogadni a kapcsolatot tőle?";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // IPShowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.ControlBox = false;
            this.Controls.Add(this.No);
            this.Controls.Add(this.Yes);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IPShowForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bejövő kapcsolat";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.IPShowForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Yes;
        private System.Windows.Forms.Button No;
        private System.Windows.Forms.Label label1;
    }
}