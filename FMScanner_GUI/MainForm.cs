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
using Newtonsoft.Json;
using static AL_Common.Common;
using static AL_Common.Logger;

namespace FMScanner_GUI
{
    public sealed partial class MainForm : Form
    {
        private readonly string ConfigFile = Path.Combine(Application.StartupPath, "Config.ini");

        private const string JsonFile = "FMScanner_output.json";

        private readonly string LogFile = Path.Combine(Application.StartupPath, "log.txt");

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
                                    OutputDirTextBox.Text = value;
                                    break;
                            }
                        }
                    }
                }
            }
            catch
            {
                OutputDirTextBox.Text = "";
            }
        }

        private void WriteConfigFile()
        {
            using var sw = new StreamWriter(ConfigFile);
            sw.WriteLine("OutputFile=" + OutputDirTextBox.Text);
        }

        public MainForm()
        {
            InitializeComponent();
            ScanInfoLabel.Text = "";
            OutputFileNoteLabel.Text = "(output file will be '" + JsonFile + "')";

            Text = "FMScanner GUI " + Application.ProductVersion;

            SetLogFile(LogFile);

            ReadConfig();
        }

        private void InputFilesListBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true) e.Effect = DragDropEffects.Copy;
        }

        private void AddToListBox(List<string> items)
        {
            try
            {
                InputFilesListBox.BeginUpdate();

                var listBoxItemsHash = InputFilesListBox.Items.Cast<string>().ToHashSetPathI();
                foreach (string item in items)
                {
                    if (!listBoxItemsHash.Contains(item))
                    {
                        listBoxItemsHash.Add(item);
                        InputFilesListBox.Items.Add(item);
                    }
                }
            }
            finally
            {
                InputFilesListBox.EndUpdate();
            }
        }

        private void InputFilesListBox_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data?.GetData(DataFormats.FileDrop) is not string[] files) return;

            var filesList = files.ToList();

            for (int i = 0; i < filesList.Count; i++)
            {
                var file = filesList[i];

                if (!file.EndsWithI(".zip") && !file.EndsWithI(".7z"))
                {
                    filesList.RemoveAt(i);
                    i--;
                }
            }

            AddToListBox(filesList);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            WriteConfigFile();
        }

        private void OutputFileBrowseButton_Click(object sender, EventArgs e)
        {
#if true
            using var d = new VistaFolderBrowserDialog { InitialDirectory = OutputDirTextBox.Text };
            if (d.ShowDialog(this) != DialogResult.OK) return;
            OutputDirTextBox.Text = d.DirectoryName;
#else
            using var d = new OpenFileDialog { InitialDirectory = OutputDirTextBox.Text };
            if (d.ShowDialog(this) != DialogResult.OK) return;
            OutputDirTextBox.Text = d.FileName;
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

        private async Task Scan()
        {
            if (InputFilesListBox.Items.Count == 0) return;

            try
            {
                _scanCts = _scanCts.Recreate();
                ScanProgressBar.Value = 0;

                foreach (Control c in Controls)
                {
                    if (c != CancelScanButton && c != ScanProgressBar && c != ScanInfoLabel)
                    {
                        c.Enabled = false;
                    }
                }

                await ScanInternal();
            }
            finally
            {
                ScanProgressBar.Value = 0;
                _scanCts.Dispose();
                ScanInfoLabel.Text = "";

                foreach (Control c in Controls)
                {
                    c.Enabled = true;
                }
            }
        }

        private static string EscapeForJson(string str)
        {
            return str
                .Replace(@"\", @"\\")
                .Replace(@"""", @"\""")
                .Replace("/", @"\/")
                .Replace("\b", @"\b")
                .Replace("\f", @"\f")
                .Replace("\n", @"\n")
                .Replace("\r", @"\r")
                .Replace("\t", @"\t");
        }

        private async Task ScanInternal()
        {
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
                if (!item.EndsWithI(".zip") && !item.EndsWithI(".7z")) continue;
                if (IgnoreFMSelBakCheckBox.Checked && item.EndsWithI(".FMSelBak.zip")) continue;

                fmsToScan.Add(new FMToScan
                {
                    Path = item
                });
            }

            if (fmsToScan.Count == 0) return;

            void ReportProgress(ProgressReport pr)
            {
                Invoke(() =>
                {
                    ScanProgressBar.Value = pr.Percent.Clamp(0, 100);
                    ScanInfoLabel.Text =
                        pr.FMName + "\r\n" +
                        pr.Percent + "%";
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

            try
            {
                bool errors = false;
                bool otherErrors = false;
                var unsupportedCompressionErrors = new List<(FMToScan FM, ScannedFMDataAndError ScannedFMDataAndError)>();

                for (int i = 0; i < fmsToScan.Count; i++)
                {
                    ScannedFMDataAndError item = fmDataList[i];
                    if (item.Fen7zResult != null ||
                        item.Exception != null ||
                        !item.ErrorInfo.IsEmpty())
                    {
                        if (item.Exception is FMScanner.FastZipReader.ZipCompressionMethodException)
                        {
                            unsupportedCompressionErrors.Add((fmsToScan[i], item));
                        }
                        else
                        {
                            otherErrors = true;
                        }

                        errors = true;
                    }
                }

                if (errors)
                {
                    if (unsupportedCompressionErrors.Count > 0)
                    {
                        if (unsupportedCompressionErrors.Count == 1)
                        {
                            MessageBox.Show(
                                "The zip archive '"
                                + unsupportedCompressionErrors[0].FM.Path +
                                "' contains one or more files compressed with an unsupported compression method. " +
                                "Only the DEFLATE method is supported. Try manually extracting and re-creating the zip archive.");
                        }
                        else
                        {
                            string msg =
                                "One or more zip archives contain files compressed with unsupported compression methods. " +
                                "Only the DEFLATE method is supported. Try manually extracting and re-creating the zip archives.\r\n\r\n" +
                                "The following zip archives produced this error:\r\n\r\n";

                            for (int errorI = 0; errorI < Math.Min(unsupportedCompressionErrors.Count, 10); errorI++)
                            {
                                msg += unsupportedCompressionErrors[errorI].FM.Path + "\r\n";
                            }

                            if (unsupportedCompressionErrors.Count > 10)
                            {
                                msg += "[See log.txt for the rest]";
                            }

                            if (otherErrors)
                            {
                                msg += "\r\n\r\nIn addition, one or more other errors occurred. See log.txt for details.";
                            }

                            MessageBox.Show(msg);
                        }
                    }
                    else
                    {
                        MessageBox.Show(
                            "One or more errors occurred while scanning. See log.txt for details.");
                    }

                    return;
                }

#if false
                WriteJson_Manual(lines, fmDataList);
#else
                WriteJson_Lib(fmDataList);
#endif
            }
            catch (Exception ex)
            {
                Log("Error trying to write file '" + JsonFile + "' in '" + OutputDirTextBox.Text + "'.", ex);
                MessageBox.Show(
                    "Error trying to write file '" + JsonFile + "' in '" + OutputDirTextBox.Text + "'. See log.txt for details." + ex,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private static string NullableDateTimeToUTC(DateTime? value)
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

        private void WriteJson_Manual(List<string> lines, List<ScannedFMDataAndError> fmDataList)
        {
            static string NullableBool(bool? value)
            {
                return value switch
                {
                    true => "true",
                    false => "false",
                    _ => "null"
                };
            }

            for (int scannedI = 0; scannedI < fmDataList.Count; scannedI++)
            {
                ScannedFMDataAndError item = fmDataList[scannedI];

                if (item.ScannedFMData == null) continue;

                ScannedFMData fmd = item.ScannedFMData;

                // TODO: We should probably use a proper JSON library to avoid problems but meh
                lines.Add("{");
                lines.Add("  \"name\": \"" + EscapeForJson(fmd.Title) + "\",");
                lines.Add("  \"author\": {");
                lines.Add("    \"name\": \"" + EscapeForJson(fmd.Author) + "\"");
                lines.Add("  },");
                lines.Add("  \"included_missions\": " + (fmd.MissionCount != null ? ((int)fmd.MissionCount).ToString(CultureInfo.InvariantCulture) : "") + ",");
                lines.Add("  \"details\": {");
                lines.Add("    \"game\": \"" + fmd.Game + "\",");
                // TODO: This means "genre" essentially, so we could parse that from the tags string if we wanted
                lines.Add("    \"category\": null,");
                lines.Add("    \"languages\": [");
                for (int langsI = 0; langsI < fmd.Languages.Length; langsI++)
                {
                    string lang = fmd.Languages[langsI];
                    lines.Add("      \"" + EscapeForJson(lang) + "\"" + (langsI == fmd.Languages.Length - 1 ? "" : ","));
                }
                lines.Add("    ],");
                lines.Add("    \"version\": \"" + EscapeForJson(fmd.Version) + "\",");
                lines.Add("    \"newdark\": {");
                lines.Add("      \"is_required\": " + NullableBool(fmd.NewDarkRequired) + ",");
                lines.Add("      \"minimum_required_version\": \"" + EscapeForJson(fmd.NewDarkMinRequiredVersion) + "\"");
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
                lines.Add("      \"has_map\": " + NullableBool(fmd.HasMap));
                lines.Add("    }");
                lines.Add("  }");
                lines.Add("}" + (scannedI == fmDataList.Count - 1 ? "" : ","));
            }

            File.WriteAllLines(Path.Combine(OutputDirTextBox.Text, JsonFile), lines, Encoding.UTF8);
        }

        internal static bool TryGetCatAndTag(string item, out string cat, out string tag)
        {
            switch (item.CountCharsUpToAmount(':', 2))
            {
                case > 1:
                    cat = "";
                    tag = "";
                    return false;
                case 1:
                    int index = item.IndexOf(':');
                    cat = item.Substring(0, index).Trim();
                    // Save an alloc if we're ascii lowercase already (case conversion always allocs, even if
                    // the new string is the same as the old)
                    if (!cat.IsAsciiLower()) cat = cat.ToLowerInvariant();
                    tag = item.Substring(index + 1).Trim();
                    break;
                default:
                    cat = "misc";
                    tag = item.Trim();
                    break;
            }

            return true;
        }

        private static string[] ParseGenres(string tags)
        {
            var list = new List<string>();

            string[] tagsArray = tags.Split(CA_CommaSemicolon, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < tagsArray.Length; i++)
            {
                if (!TryGetCatAndTag(tagsArray[i], out string cat, out string tag) ||
                    cat.IsEmpty() || tag.IsEmpty())
                {
                    continue;
                }

                if (cat.EqualsI("genre"))
                {
                    list.Add(tag);
                }
            }

            return list.Count > 0 ? list.ToArray() : new[] { "" };
        }

        private string SingleOrMultiple(FMType value) => value == FMType.FanMission ? "single" : "multiple";

        private void WriteJson_Lib(List<ScannedFMDataAndError> fmDataList)
        {
            var jsonData = new List<JSON_Object>();

            for (int scannedI = 0; scannedI < fmDataList.Count; scannedI++)
            {
                ScannedFMDataAndError item = fmDataList[scannedI];

                if (item.ScannedFMData == null) continue;

                ScannedFMData fmd = item.ScannedFMData;

                var obj = new JSON_Object
                {
                    name = fmd.Title,
                    author = fmd.Author,
                    type = SingleOrMultiple(fmd.Type),
                    //included_missions = fmd.MissionCount ?? 0,
                    details =
                    {
                        game = fmd.Game.ToString(),
                        categories = ParseGenres(fmd.TagsString),
                        languages = fmd.Languages.ToArray(),
                        version = fmd.Version,
                        newdark =
                        {
                            is_required = fmd.NewDarkRequired ?? false,
                            minimum_required_version = fmd.NewDarkMinRequiredVersion
                        },
                        last_update_date = NullableDateTimeToUTC(fmd.LastUpdateDate),
                        characteristics =
                        {
                            has_custom_scripts = fmd.HasCustomScripts == true,
                            has_custom_textures = fmd.HasCustomTextures == true,
                            has_custom_sounds = fmd.HasCustomSounds == true,
                            has_custom_objects = fmd.HasCustomObjects == true,
                            has_custom_creatures = fmd.HasCustomCreatures == true,
                            has_custom_motions = fmd.HasCustomMotions == true,
                            has_custom_subtitles = fmd.HasCustomSubtitles == true,
                            has_automap = fmd.HasAutomap == true,
                            has_movies = fmd.HasMovies == true,
                            has_map = fmd.HasMap == true
                        }
                    }
                };

                jsonData.Add(obj);
            }

            string output = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
            File.WriteAllText(Path.Combine(OutputDirTextBox.Text, JsonFile), output, Encoding.UTF8);
        }

        private async void ScanButton_Click(object sender, EventArgs e)
        {
            await Scan();
        }

        private void ClearInputFilesButton_Click(object sender, EventArgs e)
        {
            InputFilesListBox.Items.Clear();
        }

        private void AddFMsButton_Click(object sender, EventArgs e)
        {
            using var d = new OpenFileDialog { Multiselect = true, Filter = "FM archives (*.zip, *.7z)|*.zip;*.7z" };
            if (d.ShowDialog(this) != DialogResult.OK) return;
            AddToListBox(d.FileNames.ToList());
        }

        private void CancelScanButton_Click(object sender, EventArgs e) => CancelToken();
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
