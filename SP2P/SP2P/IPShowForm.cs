using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SP2P
{
    public partial class IPShowForm : Form
    {
        private IPAddress[] ips;

        public IPShowForm(IPAddress[] in_ips)
        {
            InitializeComponent();
            ips = in_ips;
        }

        private void IPShowForm_Load(object sender, EventArgs e)
        {
            foreach (var item in ips)
            {
                IPAddressBox.Items.Add(item.ToString());
            }
        }
    }
}
