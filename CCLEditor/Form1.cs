using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCLEditor
{
    public partial class Form1 : Form
    {
        CCL ccl = new CCL();
        string fileName;
        public Form1()
        {
            InitializeComponent();
        }
        private void RefreshEnv()
        {
            if (ccl == null)
                return;

            cbEntries.Items.Clear();
            textBox1.Text = "";
            textBox2.Text = "";
            foreach (Entry entry in ccl.Entries)
            {
                cbEntries.Items.Add($"Character: {entry.shortName} \t Unk1: {entry.Unknown.ToString()} \t Unk2: {entry.Unknown2.ToString()}");
            }
        }
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = ".ccl files | *.ccl";
            ofd.Multiselect = false;
            ofd.Title = "Open CCL File";

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            fileName = ofd.FileName;
            ccl.Load(fileName);

            RefreshEnv();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ccl == null)
                return;
            Entry entry = new Entry();
            entry.Padding = 0;
            entry.shortName = "NEW";
            entry.nullterm = 0;
            entry.Padding1 = new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
            entry.Unknown = 999;
            entry.Unknown2 = -1;
            entry.Padding2 = new byte[] { 0x00, 0x00, 0x00, 0x00 };
            ccl.AddEntry(entry);

            RefreshEnv();

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (ccl == null || cbEntries.SelectedIndex == -1)
                return;

            ccl.RemoveEntry(cbEntries.SelectedIndex);

            RefreshEnv();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (ccl == null || cbEntries.SelectedIndex == -1)
                return;
            ccl.Entries[cbEntries.SelectedIndex].shortName = textBox1.Text;


        }

        private void cbEntries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ccl == null)
                return;
            textBox1.Text = ccl.Entries[cbEntries.SelectedIndex].shortName;
            textBox2.Text = ccl.Entries[cbEntries.SelectedIndex].Unknown.ToString();
            textBox3.Text = ccl.Entries[cbEntries.SelectedIndex].Unknown2.ToString();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (ccl == null || cbEntries.SelectedIndex == -1)
                return;
            ccl.Entries[cbEntries.SelectedIndex].Unknown = Int32.Parse(textBox2.Text);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ccl == null)
                return;
            ccl.Save(fileName);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (ccl == null || cbEntries.SelectedIndex == -1)
                return;
            ccl.Entries[cbEntries.SelectedIndex].Unknown2 = Int32.Parse(textBox3.Text);

        }

    }
}
