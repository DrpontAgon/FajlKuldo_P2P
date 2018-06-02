using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SP2P
{
    public partial class FileReceiverForm : Form
    {
        public (string FileName, ulong Size)[] ReceiveFiles { get; private set; }
        public FileReceiverForm((string,ulong)[] files)
        {
            InitializeComponent();
            ReceiveFiles = files;
            for (int i = 0; i < files.Length; i++)
            {
                listView1.Items.Add(files[i].Item1).SubItems.Add(files[i].Item2+ " MB");
            }
        }

        private void listView1_LabelEdited(object sender, LabelEditEventArgs e)
        {
            ReceiveFiles[e.Item].FileName = e.Label;
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            List<(string,ulong)> files = new List<(string, ulong)>();
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Checked)
                {
                    files.Add((listView1.Items[i].Text, ReceiveFiles[i].Size));
                }
            }

            ReceiveFiles = files.ToArray();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = false;
            }

            ReceiveFiles = null;
        }

        private void CheckAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = true;
            }
        }

        private void CheckNone_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = false;
            }
        }
    }
}
