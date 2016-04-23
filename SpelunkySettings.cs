using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml;
using System.Windows.Forms;
using System.Diagnostics;
using LiveSplit.View;

namespace LiveSplit.Spelunky
{
    public partial class SpelunkySettings : UserControl
    {
        public delegate void PropertyChangedHandler(object sender, EventArgs args);

        public static readonly Type[] CategoryTypes = {
            typeof(Categories.AllShortcuts),
            typeof(Categories.AllJournalEntries)
        };

        const bool DefaultAutoSplittingEnabled = true;
        const bool DefaultAutoLoadSaveFile = false;
        const bool DefaultUseAlternativeSaveFile = false;
        const bool DefaultShowJournalTracker = false;
        const string DefaultSaveFile = "";
        const int DefaultRunCategoryIndex = 0;

        public bool AutoSplittingEnabled => AutoSplittingEnabledCheckBox.Checked;
        public bool ShowJournalTracker => ShowJournalTrackerCheckBox.Checked;
        public Type CurrentRunCategoryType => CategoryTypes[RunCategoryNameComboBox.SelectedIndex];
        public string SaveFile => SaveFileTextBox.Text;
        public bool AutoLoadSaveFile => AutoLoadSaveCheckBox.Checked;
        public bool ForceAlternativeSaveFile => ForceAlternativeSaveFileCheckBox.Checked;

        public event PropertyChangedHandler PropertyChanged;

        public SpelunkySettings()
        {
            InitializeComponent();
            RunCategoryNameComboBox.DataSource = CategoryTypes.Select(categoryType => Category.GetFriendlyName(categoryType)).ToArray();

            AutoSplittingEnabledCheckBox.Checked = DefaultAutoSplittingEnabled;
            RunCategoryNameComboBox.SelectedIndex = DefaultRunCategoryIndex;
            SaveFileTextBox.Text = DefaultSaveFile;
            AutoLoadSaveCheckBox.Checked = DefaultAutoLoadSaveFile;
            ForceAlternativeSaveFileCheckBox.Checked = DefaultUseAlternativeSaveFile;

            AutoSplittingEnabledCheckBox.CheckedChanged += HandleAutoSplittingCheckedChanged;
            AutoLoadSaveCheckBox.CheckedChanged += HandleAutoLoadSaveFileCheckedChanged;
            ForceAlternativeSaveFileCheckBox.CheckedChanged += HandleForceAlternativeSaveFileCheckedChanged;
            ShowJournalTrackerCheckBox.CheckedChanged += HandleShowJournalTrackerCheckBoxCheckedChanged;
            RunCategoryNameComboBox.SelectedIndexChanged += HandleRunSelectedIndexChanged;
            SaveFileBrowseButton.Click += HandleSaveFileBrowseButtonClick;

            AlternativeSaveFileLinkLabel.LinkClicked += HandleAlternativeSaveFileLinkLabelClicked;
            DownloadReferenceSplitsLabel.LinkClicked += HandleDownloadReferenceSplitsLinkClicked;
        }

        void HandleSaveFileBrowseButtonClick(object sender, EventArgs args)
        {
            var ofd = new OpenFileDialog()
            {
                Title = "Select a save file",
                Filter = "Spelunky Save File (*.sav)|*.sav",
                FilterIndex = 0,
                RestoreDirectory = true,
                CheckFileExists = true
            };
            
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ofd.OpenFile().Close();
                    SaveFileTextBox.Text = ofd.FileName;
                    if(AutoLoadSaveFile) { PropertyChanged(this, EventArgs.Empty); }
                }
                catch(Exception e)
                {
                    MessageBox.Show("Failed to read file: " + e.Message);
                }
            }
        }

        void HandleAutoLoadSaveFileCheckedChanged(object sender, EventArgs args)
        {
            if (AutoLoadSaveCheckBox.Checked && !File.Exists(SaveFile))
            {
                MessageBox.Show(SaveFile.Length == 0 ? "Select a valid save file before enabling auto-load." : "The selected auto-load save file does not exist.");
                AutoLoadSaveCheckBox.Checked = false;
            }
            else
                PropertyChanged(this, EventArgs.Empty);
        }

        void HandleAlternativeSaveFileLinkLabelClicked(object sender, LinkLabelLinkClickedEventArgs args)
        {
            MessageBox.Show("This option enables changing the save file the game uses from 'spelunky_save.sav' to 'splitter_save.sav'. This allows for maintaining a separate speedrunning save file and normal gameplay file. After LiveSplit is closed Spelunky will use the normal save file again. Enabling this is recommended when using save file auto-load.", 
                "Force alternative save file");
        }

        void HandleDownloadReferenceSplitsLinkClicked(object sender, LinkLabelLinkClickedEventArgs args)
        {
            Process.Start("https://github.com/sashavolv2/SpelunkySplitter/tree/master/ReferenceSplits");
        }

        void HandleAutoSplittingCheckedChanged(object sender, EventArgs args)
        {
            PropertyChanged(this, EventArgs.Empty);
        }
        
        void HandleForceAlternativeSaveFileCheckedChanged(object sender, EventArgs args)
        {
            PropertyChanged(this, EventArgs.Empty);
        }

        void HandleShowJournalTrackerCheckBoxCheckedChanged(object sender, EventArgs args)
        {
            PropertyChanged(this, EventArgs.Empty);
        }

        void HandleRunSelectedIndexChanged(object sender, EventArgs args)
        {
            // OPT Bypass HandleShowJournalTrackerCheckBoxCheckedChanged invocation here to avoid double-invoke of PropertyChanged
            ShowJournalTrackerCheckBox.Checked = CurrentRunCategoryType.Equals(typeof(Categories.AllJournalEntries));
            PropertyChanged(this, EventArgs.Empty);
        }

        public XmlNode GetSettings(XmlDocument doc)
        {
            var settings = doc.CreateElement("Settings");
            settings.AppendChild(XMLSettings.ToElement(doc, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));
            settings.AppendChild(XMLSettings.ToElement(doc, nameof(AutoSplittingEnabledCheckBox), AutoSplittingEnabledCheckBox.Checked));
            settings.AppendChild(XMLSettings.ToElement(doc, nameof(RunCategoryNameComboBox), CurrentRunCategoryType.Name));
            settings.AppendChild(XMLSettings.ToElement(doc, nameof(SaveFileTextBox), SaveFileTextBox.Text));
            settings.AppendChild(XMLSettings.ToElement(doc, nameof(AutoLoadSaveCheckBox), AutoLoadSaveCheckBox.Checked));
            settings.AppendChild(XMLSettings.ToElement(doc, nameof(ForceAlternativeSaveFileCheckBox), ForceAlternativeSaveFileCheckBox.Checked));
            settings.AppendChild(XMLSettings.ToElement(doc, nameof(ShowJournalTrackerCheckBox), ShowJournalTrackerCheckBox.Checked));
            return settings;
        }

        int ParseRunCategoryTypeIndex(string str)
        {
            try
            {
                return Array.IndexOf(CategoryTypes, CategoryTypes.Where(categoryType => categoryType.Name == str).First());
            }
            catch(InvalidOperationException e)
            {
                throw new Exception($"Failed to parse category type '{str}'", e);
            }
        }

        string ParseSaveFile(string str)
        {
            return str;
        }

        public void SetSettings(XmlNode settings)
        {
            AutoSplittingEnabledCheckBox.Checked = XMLSettings.Parse(settings[nameof(AutoSplittingEnabledCheckBox)], DefaultAutoSplittingEnabled, bool.Parse);
            RunCategoryNameComboBox.SelectedIndex = XMLSettings.Parse(settings[nameof(RunCategoryNameComboBox)], DefaultRunCategoryIndex, ParseRunCategoryTypeIndex);
            SaveFileTextBox.Text = XMLSettings.Parse(settings[nameof(SaveFileTextBox)], DefaultSaveFile, ParseSaveFile);
            AutoLoadSaveCheckBox.Checked = XMLSettings.Parse(settings[nameof(AutoLoadSaveCheckBox)], DefaultAutoLoadSaveFile, bool.Parse);
            ForceAlternativeSaveFileCheckBox.Checked = XMLSettings.Parse(settings[nameof(ForceAlternativeSaveFileCheckBox)], DefaultUseAlternativeSaveFile, bool.Parse);
            ShowJournalTrackerCheckBox.Checked = XMLSettings.Parse(settings[nameof(ShowJournalTrackerCheckBox)], DefaultShowJournalTracker, bool.Parse);
            PropertyChanged(this, EventArgs.Empty);
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if(Parent?.Parent?.Parent != null && Parent.Parent.Parent.GetType().Equals(typeof(ComponentSettingsDialog)))
            {
                var parentDialog = Parent.Parent.Parent;
                parentDialog.Text = "SpelunkySplitter Settings";
                var targetSize = Size + new Size(40, 110);
                parentDialog.MinimumSize = targetSize;
                parentDialog.Size = targetSize;
            }
        }
    }

    public static class XMLSettings
    {
        public delegate T ExceptingParser<T>(string str);

        public static XmlElement ToElement<T>(XmlDocument doc, string name, T value)
        {
            var strElement = doc.CreateElement(name);
            strElement.InnerText = value.ToString();
            return strElement;
        }

        public static T Parse<T>(XmlElement element, T defaultValue, ExceptingParser<T> p)
        {
            return element == null ? defaultValue : p(element.InnerText);
        }
    }
}
