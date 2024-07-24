using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpiteSplitter
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.Icon = SpiteSplitter.Properties.Resources.Spite1;
        }

        private void roundButton1_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text, out int chunkSizeMB) || chunkSizeMB <= 0)
            {
                MessageBox.Show("Invalid MB size input.", "Invalid Input");
                return;
            }

            int chunkSize = chunkSizeMB * 1024 * 1024;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select file (.doomah)";
                openFileDialog.Filter = "All Files (*.*)|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                    string fileExtension = Path.GetExtension(filePath);
                    string fileDirectory = Path.GetDirectoryName(filePath);

                    try
                    {
                        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        {
                            int partNumber = 1;
                            byte[] buffer = new byte[chunkSize];
                            int bytesRead;

                            while ((bytesRead = fileStream.Read(buffer, 0, chunkSize)) > 0)
                            {
                                string chunkFileName = $"{fileNameWithoutExtension}{fileExtension}.part{partNumber}";
                                string chunkFilePath = Path.Combine(fileDirectory, chunkFileName);

                                using (FileStream chunkFileStream = new FileStream(chunkFilePath, FileMode.Create, FileAccess.Write))
                                {
                                    chunkFileStream.Write(buffer, 0, bytesRead);
                                }

                                partNumber++;
                            }

                            Process.Start(fileDirectory);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"EPIC FAIL: {ex.Message}", "Epic Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("You didn't even select a file.", "No File");
                }
            }
        }
    }
}
