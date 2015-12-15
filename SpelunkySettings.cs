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
        public enum Category : int { AllShortcuts = 0 }
        private readonly string[] CategoryNames = { "All Shortcuts%" };

        public SpelunkySettings()
        {
            InitializeComponent();
            RunCategoryName.DataSource = CategoryNames;
        }

        static XmlElement ToXMLElement<T>(XmlDocument document, string name, T value)
        {
            XmlElement str = document.CreateElement(name);
            str.InnerText = value.ToString();
            return str;
        }

        public XmlNode GetSettings(XmlDocument doc)
        {
            XmlElement settings = doc.CreateElement("Settings");
            settings.AppendChild(ToXMLElement(doc, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));
            settings.AppendChild(ToXMLElement(doc, nameof(AutoSplittingEnabled), AutoSplittingEnabled.Checked));
            settings.AppendChild(ToXMLElement(doc, nameof(RunCategoryName), ((Category)RunCategoryName.SelectedIndex).ToString()));
            return settings;
        }

        public void SetSettings(XmlNode settings)
        {
            AutoSplittingEnabled.Checked = bool.Parse(settings[nameof(AutoSplittingEnabled)].InnerText);
            RunCategoryName.SelectedIndex = (int)(Category)Enum.Parse(typeof(Category), settings[nameof(RunCategoryName)].InnerText);
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
