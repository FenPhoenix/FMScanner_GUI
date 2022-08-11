using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AL_Common;
using FMScanner;
using FMScanner_GUI.Dialogs;
using JetBrains.Annotations;
using static AL_Common.Common;

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

        private void OutputFileBrowseButton_Click(object sender, EventArgs e)
        {
#if false
            using var d = new VistaFolderBrowserDialog { InitialDirectory = OutputFileTextBox.Text };
            if (d.ShowDialog(this) != DialogResult.OK) return;
            OutputFileTextBox.Text = d.DirectoryName;
#else
            using var d = new OpenFileDialog { InitialDirectory = OutputFileTextBox.Text };
            if (d.ShowDialog(this) != DialogResult.OK) return;
            OutputFileTextBox.Text = d.FileName;
#endif
        }

        private static readonly string _baseTemp = Path.Combine(Path.GetTempPath(), "FMScanner_GUI");


        private static string? _fmScannerTemp;
        internal static string FMScannerTemp => _fmScannerTemp ??= Path.Combine(_baseTemp, "FMScan");

        private static string? _sevenZipListTemp;
        internal static string SevenZipListTemp => _sevenZipListTemp ??= Path.Combine(_baseTemp, "7zl");

        [PublicAPI]
        [AssertionMethod]
        public static void AssertR(
            [AssertionCondition(AssertionConditionType.IS_TRUE)]
            bool condition,
            string message,
            string detailedMessage = "")
            => Trace.Assert(condition, message, detailedMessage);

        internal static readonly string SevenZipPath = Environment.Is64BitOperatingSystem
            ? Path.Combine(Application.StartupPath, "7z64")
            : Path.Combine(Application.StartupPath, "7z32");

        internal static readonly string SevenZipExe = Path.Combine(SevenZipPath, "7z.exe");

        internal void CreateOrClearTempPath(string path)
        {
            #region Safety check

            // Make sure we never delete any paths that are not safely tucked in our temp folder
            string baseTemp = _baseTemp.TrimEnd('/', '\\', ' ');

            // @DIRSEP: getting rid of this concat is more trouble than it's worth
            // This method is called rarely and only once in a row
            bool pathIsInTempDir = path.PathStartsWithI(baseTemp + "\\");

            AssertR(pathIsInTempDir, "Path '" + path + "' is not in temp dir '" + baseTemp + "'");

            if (!pathIsInTempDir) return;

            #endregion

            if (Directory.Exists(path))
            {
                try
                {
                    DirAndFileTree_UnSetReadOnly(path, throwException: true);
                }
                catch
                {
                    // ignore
                }

                try
                {
                    foreach (string f in Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly))
                    {
                        File.Delete(f);
                    }
                    foreach (string d in Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly))
                    {
                        Directory.Delete(d, recursive: true);
                    }
                }
                catch
                {
                    // ignore
                }
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch
                {
                    // ignore
                }
            }
        }

        private static CancellationTokenSource _scanCts = new();
        private static void CancelToken() => _scanCts.CancelIfNotDisposed();

        private async Task WriteJson()
        {
            try
            {
                await WriteJsonInternal();
            }
            finally
            {
                ScanProgressBar.Value = 0;
                _scanCts.Dispose();
            }
        }

        private async Task WriteJsonInternal()
        {
            _scanCts = _scanCts.Recreate();
            ScanProgressBar.Value = 0;

            var scanOptions = ScanOptions.FalseDefault(
                scanTitle: TitleCheckBox.Checked,
                scanAuthor: AuthorCheckBox.Checked,
                scanGameType: GameCheckBox.Checked,
                scanCustomResources: CustomResourcesCheckBox.Checked,
                scanSize: SizeCheckBox.Checked,
                scanReleaseDate: LastUpdatedDateCheckBox.Checked,
                scanTags: TagsCheckBox.Checked,
                scanMissionCount: MissionCountCheckBox.Checked,
                scanCampaignMissionNames: MissionNamesCheckBox.Checked,
                scanVersion: VersionCheckBox.Checked,
                scanLanguages: LanguagesCheckBox.Checked,
                scanNewDarkRequired: NewDarkRequiredCheckBox.Checked,
                scanNewDarkMinimumVersion: NewDarkMinVerCheckBox.Checked,
                scanDescription: DescriptionCheckBox.Checked
            );

            var fmsToScan = new List<FMToScan>();
            foreach (string item in InputFilesListBox.Items)
            {
                fmsToScan.Add(new FMToScan
                {
                    Path = item
                });
            }

            //void ReportProgress(ProgressReport pr) => Core.View.SetProgressBoxState_Single(
            //    message1: scanMessage ?? LText.ProgressBox.ReportScanningFirst +
            //    pr.FMNumber +
            //    LText.ProgressBox.ReportScanningBetweenNumAndTotal +
            //    pr.FMsTotal +
            //    LText.ProgressBox.ReportScanningLast,
            //    message2: pr.FMName.ExtIsArchive()
            //        ? pr.FMName.GetFileNameFast()
            //        : pr.FMName.GetDirNameFast(),
            //    percent: pr.Percent
            //);

            void ReportProgress(ProgressReport pr)
            {
                Invoke(() =>
                {
                    ScanProgressBar.Value = pr.Percent.Clamp(0, 100);
                });
            }

            List<ScannedFMDataAndError>? fmDataList;
            try
            {
                var progress = new Progress<ProgressReport>(ReportProgress);
                CreateOrClearTempPath(FMScannerTemp);
                CreateOrClearTempPath(SevenZipListTemp);
                if (_scanCts.IsCancellationRequested) return;

                using var scanner = new Scanner(SevenZipExe);
                fmDataList = await scanner.ScanAsync(fmsToScan, FMScannerTemp, scanOptions, progress,
                    _scanCts.Token);
            }
            catch (OperationCanceledException)
            {
                CreateOrClearTempPath(FMScannerTemp);
                CreateOrClearTempPath(SevenZipListTemp);
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("There were errors in the scan.");
                return;
            }

            var lines = new List<string>();

            static string NullableBool(bool? value)
            {
                return value switch
                {
                    true => "true",
                    false => "false",
                    _ => "null"
                };
            }

            static string NullableDateTimeToUTC(DateTime? value)
            {
                if (value == null)
                {
                    return "";
                }
                else
                {
                    var dt = (DateTime)value;
                    return dt.ToUniversalTime().ToString("O");
                }
            }

            try
            {
                for (int scannedI = 0; scannedI < fmDataList.Count; scannedI++)
                {
                    ScannedFMDataAndError item = fmDataList[scannedI];
                    if (item.ScannedFMData == null) continue;

                    ScannedFMData fmd = item.ScannedFMData;

                    lines.Add("{");
                    lines.Add("  \"name\": \"" + fmd.Title + "\",");
                    lines.Add("  \"author\": {");
                    lines.Add("    \"name\": \"" + fmd.Author + "\"");
                    lines.Add("  },");
                    lines.Add("  \"included_missions\": " + (fmd.MissionCount != null ? ((int)fmd.MissionCount).ToString(CultureInfo.InvariantCulture) : "") + ",");
                    lines.Add("  \"details\": {");
                    lines.Add("    \"game\": \"" + fmd.Game + "\",");
                    // TODO: What is this field? We don't scan for anything called "category"
                    lines.Add("    \"category\": null,");
                    lines.Add("    \"languages\": [");
                    for (int langsI = 0; langsI < fmd.Languages.Length; langsI++)
                    {
                        string lang = fmd.Languages[langsI];
                        lines.Add("      \"" + lang + "\"" + (langsI == fmd.Languages.Length - 1 ? "" : ","));
                    }
                    lines.Add("    ],");
                    lines.Add("    \"version\": \"" + fmd.Version + "\",");
                    lines.Add("    \"newdark\": {");
                    lines.Add("      \"is_required\": " + NullableBool(fmd.NewDarkRequired) + ",");
                    lines.Add("      \"minimum_required_version\": \"" + fmd.NewDarkMinRequiredVersion + "\"");
                    lines.Add("    },");
                    lines.Add("    \"original_release_date\": null,");
                    lines.Add("    \"last_update_date\": \"" + NullableDateTimeToUTC(fmd.LastUpdateDate) + "\",");
                    lines.Add("    \"characteristics\": {");
                    lines.Add("      \"has_custom_scripts\": " + NullableBool(fmd.HasCustomScripts) + ",");
                    lines.Add("      \"has_custom_textures\": " + NullableBool(fmd.HasCustomTextures) + ",");
                    lines.Add("      \"has_custom_sounds\": " + NullableBool(fmd.HasCustomSounds) + ",");
                    lines.Add("      \"has_custom_objects\": " + NullableBool(fmd.HasCustomObjects) + ",");
                    lines.Add("      \"has_custom_creatures\": " + NullableBool(fmd.HasCustomCreatures) + ",");
                    lines.Add("      \"has_custom_motions\": " + NullableBool(fmd.HasCustomMotions) + ",");
                    lines.Add("      \"has_custom_subtitles\": " + NullableBool(fmd.HasCustomSubtitles) + ",");
                    lines.Add("      \"has_automap\": " + NullableBool(fmd.HasAutomap) + ",");
                    lines.Add("      \"has_movies\": " + NullableBool(fmd.HasMovies) + ",");
                    lines.Add("      \"has_map\": " + NullableBool(fmd.HasMap) + ",");
                    lines.Add("    }");
                    lines.Add("  }");
                    lines.Add("}" + (scannedI == fmDataList.Count - 1 ? "" : ","));
                }

                File.WriteAllLines(OutputFileTextBox.Text, lines, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error trying to write file '" + OutputFileTextBox.Text + "':\r\n" + ex,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void ScanButton_Click(object sender, EventArgs e)
        {
            await WriteJson();
        }
    }
    /*
    {
      "name": "tempore minus modi qui natus",
      "author": {
        "name": "Louise Carter"
      },
      "included_missions": 5,
      "details": {
        "game": "libero",
        "category": "modi",
        "languages": [
          "en_IE"
        ],
        "version": "6.0.0",
        "newdark": {
          "is_required": true,
          "minimum_required_version": "6.9.1"
        },
        "original_release_date": "2022-05-23T11:59:01.309Z",
        "last_update_date": "2021-08-13T03:00:17.656Z",
        "characteristics": {
          "has_custom_scripts": true,
          "has_custom_textures": false,
          "has_custom_sounds": false,
          "has_custom_objects": true,
          "has_custom_creatures": true,
          "has_custom_motions": false,
          "has_custom_subtitles": false,
          "has_automap": true,
          "has_movies": true,
          "has_map": true
        }
      }
    }
    */
}