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

        public bool IsLayoutComponent { get; private set; }

        SpelunkySettings Settings;
        StatusWindow StatusWindow;

        AutoSplitter AutoSplitter;

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
            this.StatusWindow = new StatusWindow();
            HandleAutoSplitterChange(Settings, EventArgs.Empty); // Simulate a property change (for default values)
            Settings.PropertyChanged += HandleAutoSplitterChange;
        }

        void HandleAutoSplitterChange(object sender, EventArgs args)
        {
            ClearAutoSplitter();

            if (StatusWindow.IsDisposed) { StatusWindow = new StatusWindow(); }

            StatusWindow.CurrentRun = SpelunkySettings.CategoryNames[(int)Settings.RunCategory];
            if (Settings.AutoSplittingEnabled)
                StatusWindow.Show();
            else
                StatusWindow.Hide();
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

        Category GetCategory(SpelunkySettings.Category category)
        {
            switch(category)
            {
                case SpelunkySettings.Category.AllShortcuts:
                    return AllShortcuts.Category;
                default:
                    throw new Exception("Encountered unknown category: " + category.ToString());
            }
        }

        void GetHooksIfNeeded(LiveSplitState state)
        {
            if(AutoSplitter != null)
            {
                if (AutoSplitter.Hooks.Invalidated)
                {
                    AutoSplitter.Dispose();
                    AutoSplitter = null;
                }
                else return;
            }

            AutoSplitter = new AutoSplitter(new SpelunkyHooks(new ReadOnlyProcess("Spelunky")), GetCategory(Settings.RunCategory), new TimerModel() { CurrentState = state });
        }

        void ClearAutoSplitter()
        {
            if (AutoSplitter != null)
            {
                AutoSplitter.Dispose();
                AutoSplitter = null;
            }
        }

        public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            if (!Settings.AutoSplittingEnabled)
                return;
            
            try
            {
                GetHooksIfNeeded(state); // After this call we assume we have a valid AutoSplitter if no exception was thrown
                var status = AutoSplitter.Update(state);
                if (!StatusWindow.IsDisposed) { StatusWindow.SetStatus(status.Type, status.Message); }
            }
            catch(Exception e)
            {
                ClearAutoSplitter();
                if (!StatusWindow.IsDisposed) { StatusWindow.SetErrorStatus(e.Message); }
            }
        }

        public void Dispose()
        {
            ClearAutoSplitter();

            if(!StatusWindow.IsDisposed)
            {
                StatusWindow.Hide();
                StatusWindow.Dispose();
            }

            Settings.Hide();
            Settings.Dispose();
        }
    }
}
