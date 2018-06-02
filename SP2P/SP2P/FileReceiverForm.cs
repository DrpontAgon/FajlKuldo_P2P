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
        enum FileSizeUnit
        {
            B, KB, MB, GB, TB, PB, EB, ZB, YB
        }
        public (string FileName, ulong Size)[] FilesToSave { get; private set; }
        public bool[] ReceiveFiles { get; }

        public FileReceiverForm((string,ulong)[] files)
        {
            InitializeComponent();
            FilesToSave = files;
            ReceiveFiles = new bool[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                (double size, FileSizeUnit fsu) = (Convert.ToDouble(files[i].Item2), FileSizeUnit.B);
                while (size >= 1024.0 && fsu < FileSizeUnit.YB)
                {
                    size /= 1024.0;
                    fsu++;
                }
                ReceiveFiles[i] = true;
                listView1.Items.Add(files[i].Item1).SubItems.Add($"{size:F2} {fsu}");
                listView1.Items[i].Checked = true;
            }
        }

        private void listView1_LabelEdited(object sender, LabelEditEventArgs e)
        {
            FilesToSave[e.Item].FileName = e.Label;
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            List<(string,ulong)> files = new List<(string, ulong)>();
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                ReceiveFiles[i] = listView1.Items[i].Checked;
                if (listView1.Items[i].Checked)
                {
                    files.Add((listView1.Items[i].Text, FilesToSave[i].Size));
                }
            }

            FilesToSave = files.ToArray();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = false;
            }

            for (int i = 0; i < ReceiveFiles.Length; i++)
            {
                ReceiveFiles[i] = false;
            }

            FilesToSave = null;
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
