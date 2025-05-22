using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Backup_Manager
{
    public partial class Form1 : Form
    {
        public static String basePath = @"C:\Users\Michael\AppData\Roaming\BackupManager\Storage";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //CHECK IF PATH EXISTS, IF NOT THAN CREATE
            createFolder(basePath);

            //REFRESH DATAGRIDVIEW
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            refreshDataGridView();

            //START TIMER
            timer1.Start();
        }

        public void refreshDataGridView() {
            dataGridView1.Rows.Clear();

            String[] files = Directory.GetFiles(basePath);

            for (int i = 0; i < files.Length; i++) {
                string[] lines = readFile(files[i]).Split(
                    new[] { "\r\n", "\r", "\n" },
                    StringSplitOptions.None
                );

                dataGridView1.Rows.Add(Path.GetFileName(files[i]), lines[0], lines[1], lines[2]);
            }
        }

        public String readFile(String path) {
            String text = "";

            using (StreamReader sr = File.OpenText(path))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    text += s + "\n";
                }
            }

            return text;
        }

        public void createFile(String path, String text)
        {
            using (var tw = new StreamWriter(path, false))
            {
                tw.WriteLine(text);
            }
        }

        public void createFolder(String path) {
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            Form2.form1 = this;

            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            String path = basePath + @"\" + dataGridView1.SelectedCells[0].Value;

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this?\t\t\t\t", "Delete", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                File.Delete(path);
                Debug.WriteLine(path);
                refreshDataGridView();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            String[] files = Directory.GetFiles(basePath);
            if (files.Length == 0)
            {
                updateButton.Enabled = false;
                deleteButton.Enabled = false;
            }
            else {
                updateButton.Enabled = true;
                deleteButton.Enabled = true;
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            String path = dataGridView1.SelectedCells[2].Value.ToString() + @"\" + Path.GetFileName(dataGridView1.SelectedCells[1].Value.ToString());
            createFolder(path);

            copyFolder(dataGridView1.SelectedCells[1].Value.ToString(), path);
        }

        public void copyFolder(String SourcePath, String DestinationPath) {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);
        }
    }
}
