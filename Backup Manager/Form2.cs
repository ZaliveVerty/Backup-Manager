using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Backup_Manager
{
    public partial class Form2 : Form
    {
        public static Form1 form1;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void browseButton2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog2 = new FolderBrowserDialog();
            if (folderBrowserDialog2.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog2.SelectedPath;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bool ifEnabled = true;

            if (!string.IsNullOrEmpty(textBox1.Text))
            {
            }
            else {
                ifEnabled = false;
            }

            if (!string.IsNullOrEmpty(textBox2.Text))
            {
            }
            else {
                ifEnabled = false;
            }

            if (!string.IsNullOrEmpty(textBox3.Text))
            {
                String[] files = Directory.GetFiles(Form1.basePath);
                for (int i = 0; i < files.Length; i++) {
                    if (Path.GetFileName(files[i]).Equals(textBox3.Text)) {
                        ifEnabled = false;
                    }
                }
            }
            else
            {
                ifEnabled = false;
            }

            doneButton.Enabled = ifEnabled;
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            String text = textBox1.Text + "\r\n" + textBox2.Text + "\r\nNever";
            //DateTime.Now.ToString("yyyy-MM-dd hh:mm")
            form1.createFile(Form1.basePath + @"\" + textBox3.Text, text);

            form1.refreshDataGridView();

            Close();
        }
    }
}
