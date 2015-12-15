using LiveSplit.Model;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using LiveSplit.UI;
using System.Drawing;
using System.Windows.Forms;

namespace LiveSplit.Spelunky
{
    class Component : IComponent
    {
        public string ComponentName => "SpelunkySplitter";

        public SpelunkySettings Settings { get; set; }
        public bool IsLayoutComponent { get; private set; }

        public float HorizontalWidth => 100.0f;
        public float MinimumHeight => 100.0f;
        public float VerticalHeight => 100.0f;
        public float MinimumWidth => 100.0f;
        public float MaximumHeight => 100.0f;
        public float PaddingTop => 1.0f;
        public float PaddingBottom => 1.0f;
        public float PaddingLeft => 1.0f;
        public float PaddingRight => 1.0f; 

        private IDictionary<string, Action> _ContextMenuControls = new Dictionary<string, Action>();
        public IDictionary<string, Action> ContextMenuControls => _ContextMenuControls;

        public Component(LiveSplitState state, bool isLayoutComponent)
        {
            this.IsLayoutComponent = isLayoutComponent;
            this.Settings = new SpelunkySettings();
        }

        public XmlNode GetSettings(XmlDocument document)
        {
            return this.Settings.GetSettings(document);
        }

        public void SetSettings(XmlNode node)
        {
            this.Settings.SetSettings(node);
        }

        public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion) {}

        public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion) {}

        public Control GetSettingsControl(LayoutMode mode)
        {
            return this.Settings;
        }

        public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            Console.WriteLine("[LiveSplit.Spelunky] Update(...) called with CurrentSplit=" + state.CurrentSplit);
            //TODO
        }

        public void Dispose() {}
    }
}
