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

    public partial class SpelunkySettings : UserControl
    {
        public enum Category : int { AllShortcuts = 0 }
        private readonly string[] CategoryNames = { "All Shortcuts%" };

        const bool DEFAULT_AUTOSPLITTING_ENABLED = true;
        const int DEFAULT_RUN_CATEGORY_INDEX = (int)Category.AllShortcuts;
        
        public SpelunkySettings()
        {
            InitializeComponent();
            RunCategoryName.DataSource = CategoryNames;
        }

        public XmlNode GetSettings(XmlDocument doc)
        {
            XmlElement settings = doc.CreateElement("Settings");
            settings.AppendChild(XMLSettings.ToElement(doc, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));
            settings.AppendChild(XMLSettings.ToElement(doc, nameof(AutoSplittingEnabled), AutoSplittingEnabled.Checked));
            settings.AppendChild(XMLSettings.ToElement(doc, nameof(RunCategoryName), ((Category)RunCategoryName.SelectedIndex).ToString()));
            return settings;
        }

        Category ParseRunCategory(string str)
        {
            return (Category)Enum.Parse(typeof(Category), str);
        }

        public void SetSettings(XmlNode settings)
        {
            AutoSplittingEnabled.Checked = XMLSettings.Parse(settings[nameof(AutoSplittingEnabled)], true, bool.Parse);
            RunCategoryName.SelectedIndex = (int)XMLSettings.Parse(settings[nameof(RunCategoryName)], Category.AllShortcuts, ParseRunCategory);
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

        public bool IsAutoSplittingEnabled
        {
            get { return AutoSplittingEnabled.Checked; }
        }

        public Category RunCategory
        {
            get { return (Category)RunCategoryName.SelectedIndex; }
        }
    }
}
