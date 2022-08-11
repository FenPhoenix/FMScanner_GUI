using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FMScanner_GUI
{
    public sealed partial class MainForm : Form
    {
        private readonly string ConfigFile = Path.Combine(Application.StartupPath, "Config.ini");

        private void ReadConfig()
        {
            try
            {
                if (File.Exists(ConfigFile))
                {
                    string[] lines = File.ReadAllLines(ConfigFile);
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string lineT = lines[i].Trim();
                        int eqIndex = lineT.IndexOf('=');
                        if (eqIndex > -1)
                        {
                            string key = lineT.Substring(0, eqIndex);
                            string value = lineT.Substring(eqIndex + 1);

                            switch (key)
                            {
                                case "OutputFile":
                                    OutputFileTextBox.Text = value;
                                    break;
                            }
                        }
                    }
                }
            }
            catch
            {
                OutputFileTextBox.Text = "";
            }
        }

        private void WriteConfigFile()
        {
            using var sw = new StreamWriter(ConfigFile);
            sw.WriteLine("OutputFile=" + OutputFileTextBox.Text);
        }

        public MainForm()
        {
            InitializeComponent();

            ReadConfig();
        }

        private void InputFilesListBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true) e.Effect = DragDropEffects.Copy;
        }

        private void InputFilesListBox_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data?.GetData(DataFormats.FileDrop) is not string[] files) return;

            InputFilesListBox.Items.AddRange(files.Cast<object>().ToArray());
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            WriteConfigFile();
        }
    }
}