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
        JournalTracker JournalTracker;

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
            IsLayoutComponent = isLayoutComponent;
            Settings = new SpelunkySettings();
            StatusWindow = new StatusWindow();
            JournalTracker = new JournalTracker();
            HandleAutoSplitterChange(Settings, EventArgs.Empty); // Simulate a property change (for default values)
            Settings.PropertyChanged += HandleAutoSplitterChange;
        }

        void HandleAutoSplitterChange(object sender, EventArgs args)
        {
            ClearAutoSplitter();

            if(StatusWindow.IsDisposed) { StatusWindow = new StatusWindow(); }

            StatusWindow.CurrentRun = Category.GetFriendlyName(Settings.CurrentRunCategoryType);
            if(Settings.AutoSplittingEnabled)
                StatusWindow.Show();
            else
                StatusWindow.Hide();
        }

        public XmlNode GetSettings(XmlDocument document)
        {
            return Settings.GetSettings(document);
        }

        public void SetSettings(XmlNode node)
        {
            Settings.SetSettings(node);
        }

        public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion) {}

        public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion) {}

        public Control GetSettingsControl(LayoutMode mode)
        {
            return Settings;
        }

        EnabledPatchContainer MakePatchesFromSettings(SpelunkyHooks hooks)
        {
            var container = new EnabledPatchContainer();
            if (Settings.ForceAlternativeSaveFile)
                container.TryAddAndEnable(() => new SaveChangePatch(hooks));
            return container;
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

            var hooks = new SpelunkyHooks(new RawProcess("Spelunky"));
            JournalTracker.Size = 
                new Size((int)(Spelunky.JournalTracker.InitialSize.Width * Settings.JournalTrackerScale),
                    (int)(Spelunky.JournalTracker.InitialSize.Height * Settings.JournalTrackerScale));
            AutoSplitter = new AutoSplitter(hooks, Settings.CurrentRunCategoryType, 
                MakePatchesFromSettings(hooks), new TimerModel() { CurrentState = state }, 
                Settings.AutoLoadSaveFile ? Settings.SaveFile : null, Settings.ShowJournalTracker ? JournalTracker : null);
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
