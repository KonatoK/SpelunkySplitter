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

        public enum Category : int { AllShortcuts = 0 }

        public static readonly string[] CategoryNames = { "All Shortcuts + Olmec %" };

        const bool DEFAULT_AUTOSPLITTING_ENABLED = true;
        const bool DEFAULT_AUTOLOAD_SAVEFILE = false;
        const string DEFAULT_SAVEFILE = "";
        const Category DEFAULT_RUN_CATEGORY = Category.AllShortcuts;

        public bool AutoSplittingEnabled => AutoSplittingEnabledCheckBox.Checked;
        public Category RunCategory => (Category)RunCategoryNameComboBox.SelectedIndex;
        public string SaveFile => SaveFileTextBox.Text;
        public bool AutoLoadSaveFile => AutoLoadSaveCheckBox.Checked;

        public event PropertyChangedHandler PropertyChanged;

        public SpelunkySettings()
        {
            InitializeComponent();
            RunCategoryNameComboBox.DataSource = CategoryNames;

            AutoSplittingEnabledCheckBox.Checked = DEFAULT_AUTOSPLITTING_ENABLED;
            RunCategoryNameComboBox.SelectedIndex = (int)DEFAULT_RUN_CATEGORY;
            SaveFileTextBox.Text = DEFAULT_SAVEFILE;
            AutoLoadSaveCheckBox.Checked = DEFAULT_AUTOLOAD_SAVEFILE;

            AutoSplittingEnabledCheckBox.CheckedChanged += HandleAutoSplittingCheckedChanged;
            AutoLoadSaveCheckBox.CheckedChanged += HandleAutoLoadSaveFileCheckedChanged;
            RunCategoryNameComboBox.SelectedIndexChanged += HandleRunSelectedIndexChanged;
            SaveFileBrowseButton.Click += HandleSaveFileBrowseButtonClick;

            DownloadReferenceSplitsLabel.LinkClicked += HandleDownloadReferenceSplitsLinkClicked;
        }

        void HandleSaveFileBrowseButtonClick(object sender, EventArgs args)
        {
            OpenFileDialog ofd = new OpenFileDialog()
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

        void HandleDownloadReferenceSplitsLinkClicked(object sender, LinkLabelLinkClickedEventArgs args)
        {
            Process.Start("https://github.com/sashavolv2/SpelunkySplitter/tree/master/ReferenceSplits");
        }

        void HandleAutoSplittingCheckedChanged(object sender, EventArgs args)
        {
            PropertyChanged(this, EventArgs.Empty);
        }

        void HandleRunSelectedIndexChanged(object sender, EventArgs args)
        {
            PropertyChanged(this, EventArgs.Empty);
        }

        public XmlNode GetSettings(XmlDocument doc)
        {
            XmlElement settings = doc.CreateElement("Settings");
            settings.AppendChild(XMLSettings.ToElement(doc, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));
            settings.AppendChild(XMLSettings.ToElement(doc, nameof(AutoSplittingEnabledCheckBox), AutoSplittingEnabledCheckBox.Checked));
            settings.AppendChild(XMLSettings.ToElement(doc, nameof(RunCategoryNameComboBox), ((Category)RunCategoryNameComboBox.SelectedIndex).ToString()));
            settings.AppendChild(XMLSettings.ToElement(doc, nameof(SaveFileTextBox), SaveFileTextBox.Text));
            settings.AppendChild(XMLSettings.ToElement(doc, nameof(AutoLoadSaveCheckBox), AutoLoadSaveCheckBox.Checked));
            return settings;
        }

        Category ParseRunCategory(string str) { return (Category)Enum.Parse(typeof(Category), str); }

        string ParseSaveFile(string str) { return str; }

        public void SetSettings(XmlNode settings)
        {
            AutoSplittingEnabledCheckBox.Checked = XMLSettings.Parse(settings[nameof(AutoSplittingEnabledCheckBox)], DEFAULT_AUTOSPLITTING_ENABLED, bool.Parse);
            RunCategoryNameComboBox.SelectedIndex = (int)XMLSettings.Parse(settings[nameof(RunCategoryNameComboBox)], DEFAULT_RUN_CATEGORY, ParseRunCategory);
            SaveFileTextBox.Text = XMLSettings.Parse(settings[nameof(SaveFileTextBox)], DEFAULT_SAVEFILE, ParseSaveFile);
            AutoLoadSaveCheckBox.Checked = XMLSettings.Parse(settings[nameof(AutoLoadSaveCheckBox)], AutoLoadSaveCheckBox.Checked, bool.Parse);
            PropertyChanged(this, EventArgs.Empty);
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if(this.Parent?.Parent?.Parent != null && this.Parent.Parent.Parent.GetType().Equals(typeof(ComponentSettingsDialog)))
            {
                var parentDialog = this.Parent.Parent.Parent;
                parentDialog.Text = "SpelunkySplitter Settings";
                var targetSize = this.Size + new Size(40, 110);
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
            XmlElement str = doc.CreateElement(name);
            str.InnerText = value.ToString();
            return str;
        }

        public static T Parse<T>(XmlElement element, T defaultValue, ExceptingParser<T> p)
        {
            return element == null ? defaultValue : p(element.InnerText);
        }
    }
}
