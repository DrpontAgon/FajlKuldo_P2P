namespace SP2P
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.bt_connect = new System.Windows.Forms.Button();
            this.tb_ip_1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_ip_2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_ip_4 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_ip_3 = new System.Windows.Forms.TextBox();
            this.tb_port = new System.Windows.Forms.TextBox();
            this.lb_wan_inf = new System.Windows.Forms.Label();
            this.lb_wan = new System.Windows.Forms.Label();
            this.lb_lan_inf = new System.Windows.Forms.Label();
            this.lb_lan = new System.Windows.Forms.Label();
            this.panel = new System.Windows.Forms.Panel();
            this.bt_send_info = new System.Windows.Forms.Button();
            this.lb_files = new System.Windows.Forms.ListBox();
            this.bt_send = new System.Windows.Forms.Button();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cms_item_ip = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_item_settings = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_listen = new System.Windows.Forms.Button();
            this.bt_port_openclose = new System.Windows.Forms.Button();
            this.panel.SuspendLayout();
            this.cms.SuspendLayout();
            this.SuspendLayout();
            // 
            // bt_connect
            // 
            this.bt_connect.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.bt_connect.Location = new System.Drawing.Point(42, 142);
            this.bt_connect.Name = "bt_connect";
            this.bt_connect.Size = new System.Drawing.Size(141, 30);
            this.bt_connect.TabIndex = 5;
            this.bt_connect.Text = "Csatlakozás";
            this.bt_connect.UseVisualStyleBackColor = true;
            this.bt_connect.Click += new System.EventHandler(this.bt_connect_Click);
            // 
            // tb_ip_1
            // 
            this.tb_ip_1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tb_ip_1.Location = new System.Drawing.Point(4, 113);
            this.tb_ip_1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.tb_ip_1.MaxLength = 3;
            this.tb_ip_1.Name = "tb_ip_1";
            this.tb_ip_1.Size = new System.Drawing.Size(30, 23);
            this.tb_ip_1.TabIndex = 0;
            this.tb_ip_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_ip_1.TextChanged += new System.EventHandler(this.tb_ip_1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(34, 116);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = ".";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(76, 116);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = ".";
            // 
            // tb_ip_2
            // 
            this.tb_ip_2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tb_ip_2.Location = new System.Drawing.Point(46, 113);
            this.tb_ip_2.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.tb_ip_2.MaxLength = 3;
            this.tb_ip_2.Name = "tb_ip_2";
            this.tb_ip_2.Size = new System.Drawing.Size(30, 23);
            this.tb_ip_2.TabIndex = 1;
            this.tb_ip_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_ip_2.TextChanged += new System.EventHandler(this.tb_ip_2_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(160, 116);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = ":";
            // 
            // tb_ip_4
            // 
            this.tb_ip_4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tb_ip_4.Location = new System.Drawing.Point(130, 113);
            this.tb_ip_4.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.tb_ip_4.MaxLength = 3;
            this.tb_ip_4.Name = "tb_ip_4";
            this.tb_ip_4.Size = new System.Drawing.Size(30, 23);
            this.tb_ip_4.TabIndex = 3;
            this.tb_ip_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_ip_4.TextChanged += new System.EventHandler(this.tb_ip_4_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(118, 116);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = ".";
            // 
            // tb_ip_3
            // 
            this.tb_ip_3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tb_ip_3.Location = new System.Drawing.Point(88, 113);
            this.tb_ip_3.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.tb_ip_3.MaxLength = 3;
            this.tb_ip_3.Name = "tb_ip_3";
            this.tb_ip_3.Size = new System.Drawing.Size(30, 23);
            this.tb_ip_3.TabIndex = 2;
            this.tb_ip_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_ip_3.TextChanged += new System.EventHandler(this.tb_ip_3_TextChanged);
            // 
            // tb_port
            // 
            this.tb_port.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tb_port.Location = new System.Drawing.Point(172, 113);
            this.tb_port.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.tb_port.MaxLength = 5;
            this.tb_port.Name = "tb_port";
            this.tb_port.Size = new System.Drawing.Size(50, 23);
            this.tb_port.TabIndex = 4;
            this.tb_port.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_port.TextChanged += new System.EventHandler(this.tb_port_TextChanged);
            // 
            // lb_wan_inf
            // 
            this.lb_wan_inf.BackColor = System.Drawing.Color.Yellow;
            this.lb_wan_inf.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lb_wan_inf.Location = new System.Drawing.Point(0, 0);
            this.lb_wan_inf.Margin = new System.Windows.Forms.Padding(0);
            this.lb_wan_inf.Name = "lb_wan_inf";
            this.lb_wan_inf.Size = new System.Drawing.Size(70, 20);
            this.lb_wan_inf.TabIndex = 0;
            this.lb_wan_inf.Text = "WAN IP:";
            this.lb_wan_inf.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lb_wan
            // 
            this.lb_wan.BackColor = System.Drawing.Color.Yellow;
            this.lb_wan.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lb_wan.Location = new System.Drawing.Point(70, 0);
            this.lb_wan.Margin = new System.Windows.Forms.Padding(0);
            this.lb_wan.Name = "lb_wan";
            this.lb_wan.Size = new System.Drawing.Size(156, 20);
            this.lb_wan.TabIndex = 0;
            this.lb_wan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lb_lan_inf
            // 
            this.lb_lan_inf.BackColor = System.Drawing.Color.Yellow;
            this.lb_lan_inf.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lb_lan_inf.Location = new System.Drawing.Point(0, 20);
            this.lb_lan_inf.Margin = new System.Windows.Forms.Padding(0);
            this.lb_lan_inf.Name = "lb_lan_inf";
            this.lb_lan_inf.Size = new System.Drawing.Size(70, 20);
            this.lb_lan_inf.TabIndex = 0;
            this.lb_lan_inf.Text = "LAN IP:";
            this.lb_lan_inf.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lb_lan
            // 
            this.lb_lan.BackColor = System.Drawing.Color.Yellow;
            this.lb_lan.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lb_lan.Location = new System.Drawing.Point(70, 20);
            this.lb_lan.Margin = new System.Windows.Forms.Padding(0);
            this.lb_lan.Name = "lb_lan";
            this.lb_lan.Size = new System.Drawing.Size(156, 20);
            this.lb_lan.TabIndex = 0;
            this.lb_lan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel
            // 
            this.panel.Controls.Add(this.bt_send_info);
            this.panel.Controls.Add(this.lb_files);
            this.panel.Controls.Add(this.bt_send);
            this.panel.Enabled = false;
            this.panel.Location = new System.Drawing.Point(0, 175);
            this.panel.Margin = new System.Windows.Forms.Padding(0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(226, 287);
            this.panel.TabIndex = 16;
            // 
            // bt_send_info
            // 
            this.bt_send_info.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.bt_send_info.Location = new System.Drawing.Point(185, 4);
            this.bt_send_info.Name = "bt_send_info";
            this.bt_send_info.Size = new System.Drawing.Size(26, 26);
            this.bt_send_info.TabIndex = 10;
            this.bt_send_info.Text = "?";
            this.bt_send_info.UseVisualStyleBackColor = true;
            this.bt_send_info.Click += new System.EventHandler(this.bt_send_info_Click);
            // 
            // lb_files
            // 
            this.lb_files.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lb_files.FormattingEnabled = true;
            this.lb_files.HorizontalScrollbar = true;
            this.lb_files.ItemHeight = 20;
            this.lb_files.Location = new System.Drawing.Point(12, 40);
            this.lb_files.Name = "lb_files";
            this.lb_files.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lb_files.Size = new System.Drawing.Size(201, 244);
            this.lb_files.TabIndex = 11;
            this.lb_files.DoubleClick += new System.EventHandler(this.lb_files_DoubleClick);
            this.lb_files.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lb_files_MouseDown);
            // 
            // bt_send
            // 
            this.bt_send.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.bt_send.Location = new System.Drawing.Point(42, 2);
            this.bt_send.Name = "bt_send";
            this.bt_send.Size = new System.Drawing.Size(141, 30);
            this.bt_send.TabIndex = 9;
            this.bt_send.Text = "Fájl küldése";
            this.bt_send.UseVisualStyleBackColor = true;
            this.bt_send.Click += new System.EventHandler(this.bt_send_Click);
            // 
            // ofd
            // 
            this.ofd.FileName = "openFileDialog1";
            // 
            // cms
            // 
            this.cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cms_item_ip,
            this.cms_item_settings});
            this.cms.Name = "cms";
            this.cms.Size = new System.Drawing.Size(168, 48);
            this.cms.Opening += new System.ComponentModel.CancelEventHandler(this.cms_Opening);
            // 
            // cms_item_ip
            // 
            this.cms_item_ip.Name = "cms_item_ip";
            this.cms_item_ip.Size = new System.Drawing.Size(167, 22);
            this.cms_item_ip.Text = "IP címek frissítése";
            this.cms_item_ip.Click += new System.EventHandler(this.cms_item_ip_Click);
            // 
            // cms_item_settings
            // 
            this.cms_item_settings.Name = "cms_item_settings";
            this.cms_item_settings.Size = new System.Drawing.Size(167, 22);
            this.cms_item_settings.Text = "Beállítások";
            this.cms_item_settings.Click += new System.EventHandler(this.cms_item_settings_Click);
            // 
            // bt_listen
            // 
            this.bt_listen.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.bt_listen.Location = new System.Drawing.Point(42, 77);
            this.bt_listen.Name = "bt_listen";
            this.bt_listen.Size = new System.Drawing.Size(141, 30);
            this.bt_listen.TabIndex = 18;
            this.bt_listen.Text = "Kapcsolatra várás";
            this.bt_listen.UseVisualStyleBackColor = true;
            this.bt_listen.Click += new System.EventHandler(this.bt_listen_Click);
            // 
            // bt_port_openclose
            // 
            this.bt_port_openclose.Enabled = false;
            this.bt_port_openclose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.bt_port_openclose.Location = new System.Drawing.Point(42, 41);
            this.bt_port_openclose.Name = "bt_port_openclose";
            this.bt_port_openclose.Size = new System.Drawing.Size(141, 30);
            this.bt_port_openclose.TabIndex = 17;
            this.bt_port_openclose.Text = "Port";
            this.bt_port_openclose.UseVisualStyleBackColor = true;
            this.bt_port_openclose.Click += new System.EventHandler(this.bt_port_openclose_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(226, 462);
            this.ContextMenuStrip = this.cms;
            this.Controls.Add(this.bt_port_openclose);
            this.Controls.Add(this.bt_listen);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.lb_lan);
            this.Controls.Add(this.lb_lan_inf);
            this.Controls.Add(this.lb_wan);
            this.Controls.Add(this.lb_wan_inf);
            this.Controls.Add(this.tb_port);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_ip_4);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb_ip_3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_ip_2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_ip_1);
            this.Controls.Add(this.bt_connect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SP2P";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel.ResumeLayout(false);
            this.cms.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_connect;
        private System.Windows.Forms.TextBox tb_ip_1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_ip_2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_ip_4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_ip_3;
        private System.Windows.Forms.TextBox tb_port;
        private System.Windows.Forms.Label lb_wan_inf;
        private System.Windows.Forms.Label lb_wan;
        private System.Windows.Forms.Label lb_lan_inf;
        private System.Windows.Forms.Label lb_lan;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Button bt_send_info;
        private System.Windows.Forms.ListBox lb_files;
        private System.Windows.Forms.Button bt_send;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.ContextMenuStrip cms;
        private System.Windows.Forms.ToolStripMenuItem cms_item_ip;
        private System.Windows.Forms.ToolStripMenuItem cms_item_settings;
        private System.Windows.Forms.Button bt_listen;
        private System.Windows.Forms.Button bt_port_openclose;
    }
}

