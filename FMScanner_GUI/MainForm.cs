using System.Linq;
using System.Windows.Forms;

namespace FMScanner_GUI
{
    public sealed partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
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
    }
}