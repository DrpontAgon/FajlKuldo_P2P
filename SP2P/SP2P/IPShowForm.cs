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
        IPAddress[] ips;
        public bool Accepted = true;

        public IPShowForm(IPAddress[] in_ips)
        {
            InitializeComponent();
            ips = in_ips;
        }
    }
}
