using System;
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

        public static readonly string[] CategoryNames = { "All Shortcuts%" };

        const bool DEFAULT_AUTOSPLITTING_ENABLED = true;
        const Category DEFAULT_RUN_CATEGORY = Category.AllShortcuts;

        public event PropertyChangedHandler PropertyChanged;

        public SpelunkySettings()
        {
            InitializeComponent();
            RunCategoryNameComboBox.DataSource = CategoryNames;

            AutoSplittingEnabledCheckBox.CheckedChanged += HandleAutoSplittingCheckedChanged;
            RunCategoryNameComboBox.SelectedIndexChanged += HandleRunSelectedIndexChanged;
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
            return settings;
        }

        Category ParseRunCategory(string str)
        {
            return (Category)Enum.Parse(typeof(Category), str);
        }

        public void SetSettings(XmlNode settings)
        {
            AutoSplittingEnabledCheckBox.Checked = XMLSettings.Parse(settings[nameof(AutoSplittingEnabledCheckBox)], DEFAULT_AUTOSPLITTING_ENABLED, bool.Parse);
            RunCategoryNameComboBox.SelectedIndex = (int)XMLSettings.Parse(settings[nameof(RunCategoryNameComboBox)], DEFAULT_RUN_CATEGORY, ParseRunCategory);
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

        public bool AutoSplittingEnabled
        {
            get { return AutoSplittingEnabledCheckBox.Checked; }
        }

        public Category RunCategory
        {
            get { return (Category)RunCategoryNameComboBox.SelectedIndex; }
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
