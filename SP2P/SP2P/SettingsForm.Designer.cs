namespace SP2P
{
    partial class SettingsForm
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
            this.tab_control = new System.Windows.Forms.TabControl();
            this.tab_2 = new System.Windows.Forms.TabPage();
            this.bt_alap_beall = new System.Windows.Forms.Button();
            this.bt_elfogad = new System.Windows.Forms.Button();
            this.bt_megse = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_port = new System.Windows.Forms.TextBox();
            this.fbd = new System.Windows.Forms.FolderBrowserDialog();
            this.tab_control.SuspendLayout();
            this.tab_2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab_control
            // 
            this.tab_control.Controls.Add(this.tab_2);
            this.tab_control.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab_control.Location = new System.Drawing.Point(0, 0);
            this.tab_control.Margin = new System.Windows.Forms.Padding(0);
            this.tab_control.Name = "tab_control";
            this.tab_control.SelectedIndex = 0;
            this.tab_control.Size = new System.Drawing.Size(384, 211);
            this.tab_control.TabIndex = 0;
            // 
            // tab_2
            // 
            this.tab_2.Controls.Add(this.bt_alap_beall);
            this.tab_2.Controls.Add(this.bt_elfogad);
            this.tab_2.Controls.Add(this.bt_megse);
            this.tab_2.Controls.Add(this.label2);
            this.tab_2.Controls.Add(this.tb_port);
            this.tab_2.Location = new System.Drawing.Point(4, 22);
            this.tab_2.Name = "tab_2";
            this.tab_2.Padding = new System.Windows.Forms.Padding(3);
            this.tab_2.Size = new System.Drawing.Size(376, 185);
            this.tab_2.TabIndex = 1;
            this.tab_2.Text = "Net";
            this.tab_2.UseVisualStyleBackColor = true;
            // 
            // bt_alap_beall
            // 
            this.bt_alap_beall.Location = new System.Drawing.Point(8, 156);
            this.bt_alap_beall.Name = "bt_alap_beall";
            this.bt_alap_beall.Size = new System.Drawing.Size(148, 23);
            this.bt_alap_beall.TabIndex = 13;
            this.bt_alap_beall.Text = "Alapértelmezett Beállítások";
            this.bt_alap_beall.UseVisualStyleBackColor = true;
            this.bt_alap_beall.Click += new System.EventHandler(this.bt_alap_beall_Click);
            // 
            // bt_elfogad
            // 
            this.bt_elfogad.Location = new System.Drawing.Point(216, 156);
            this.bt_elfogad.Name = "bt_elfogad";
            this.bt_elfogad.Size = new System.Drawing.Size(73, 23);
            this.bt_elfogad.TabIndex = 12;
            this.bt_elfogad.Text = "Elfogad";
            this.bt_elfogad.UseVisualStyleBackColor = true;
            this.bt_elfogad.Click += new System.EventHandler(this.bt_elfogad_Click);
            // 
            // bt_megse
            // 
            this.bt_megse.Location = new System.Drawing.Point(295, 156);
            this.bt_megse.Name = "bt_megse";
            this.bt_megse.Size = new System.Drawing.Size(73, 23);
            this.bt_megse.TabIndex = 11;
            this.bt_megse.Text = "Mégse";
            this.bt_megse.UseVisualStyleBackColor = true;
            this.bt_megse.Click += new System.EventHandler(this.bt_megse_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port (50000-65535):";
            // 
            // tb_port
            // 
            this.tb_port.Location = new System.Drawing.Point(7, 30);
            this.tb_port.MaxLength = 5;
            this.tb_port.Name = "tb_port";
            this.tb_port.Size = new System.Drawing.Size(100, 20);
            this.tb_port.TabIndex = 0;
            this.tb_port.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 211);
            this.Controls.Add(this.tab_control);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Beállítások";
            this.tab_control.ResumeLayout(false);
            this.tab_2.ResumeLayout(false);
            this.tab_2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tab_control;
        private System.Windows.Forms.TabPage tab_2;
        private System.Windows.Forms.FolderBrowserDialog fbd;
        private System.Windows.Forms.Button bt_alap_beall;
        private System.Windows.Forms.Button bt_elfogad;
        private System.Windows.Forms.Button bt_megse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_port;
    }
}