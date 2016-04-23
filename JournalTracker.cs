using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace LiveSplit.Spelunky
{
    public partial class JournalTracker : Form
    {
        private JournalVisualizer JournalVisualizer;

        public JournalTracker()
        {
            InitializeComponent();
            Utilities.Forms.DisableCloseButton(this);

            JournalVisualizer = new JournalVisualizer();
            JournalVisualizer.Location = new Point(0, 0);
            JournalVisualizer.Size = new Size(Width, Height);
            Controls.Add(JournalVisualizer);
        }

        public void Update(SpelunkyHooks spelunky)
        {
            JournalVisualizer.SetJournal(spelunky.JournalState);
        }
    }

    public class JournalVisualizer : Control
    {
        JournalState MaybeJournal;
        
        public JournalVisualizer()
        {
            DoubleBuffered = true;
        }

        private class JournalPainter
        {
            private const int EntryRenderHeightPx = 24;
            private const int EntryMarginPx = 3;

            private Graphics Graphics;
            private Point CurrentEntryDrawPosition;
            private int MaxColumnImageWidth;
            private int Height;

            public JournalPainter(Graphics graphics, int height)
            {
                Graphics = graphics;
                CurrentEntryDrawPosition = new Point(EntryMarginPx, EntryMarginPx);
                MaxColumnImageWidth = 0;
                Height = height;
            }

            public void DrawEntry(string entryType, int entryIndex, bool entryIsUnlocked)
            {
                var entryImageResourceName = $"{entryType}_{entryIndex}" + (!entryIsUnlocked ? "" : "_Inactive");
                var entryImage = (Image)Properties.Resources.ResourceManager.GetObject(entryImageResourceName);
                var entryImageScale = EntryRenderHeightPx / (float)entryImage.Height;
                var entryImageWidth = entryImageScale * entryImage.Width;
                Graphics.DrawImage(entryImage, CurrentEntryDrawPosition.X, CurrentEntryDrawPosition.Y, entryImageWidth, EntryRenderHeightPx);
                MaxColumnImageWidth = Math.Max((int)entryImageWidth, MaxColumnImageWidth);
                AdvanceToNextCell();
            }

            private void AdvanceToNextCell()
            {
                CurrentEntryDrawPosition.Y += EntryRenderHeightPx + EntryMarginPx;
                if(CurrentEntryDrawPosition.Y + EntryRenderHeightPx >= Height)
                    NewColumn();
            }

            public void NewColumn()
            {
                CurrentEntryDrawPosition.Y = EntryMarginPx;
                CurrentEntryDrawPosition.X += MaxColumnImageWidth + EntryMarginPx;
                MaxColumnImageWidth = 0;
            }
        }

        public void SetJournal(JournalState journal)
        {
            if(MaybeJournal == null || !MaybeJournal.Equals(journal))
            {
                MaybeJournal = journal;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if(MaybeJournal != null)
            {
                var Journal = MaybeJournal;
                var painter = new JournalPainter(e.Graphics, Height);
                foreach(var placeIndex in Enumerable.Range(0, JournalState.NumPlaceEntries))
                    painter.DrawEntry("Place", placeIndex, Journal.PlaceEntries[placeIndex]);
                painter.NewColumn();
                foreach(var monsterIndex in Enumerable.Range(0, JournalState.NumMonsterEntries))
                    painter.DrawEntry("Monster", monsterIndex, Journal.MonsterEntries[monsterIndex]);
                painter.NewColumn();
                foreach(var itemIndex in Enumerable.Range(0, JournalState.NumItemEntries))
                    painter.DrawEntry("Item", itemIndex, Journal.ItemEntries[itemIndex]);
                painter.NewColumn();
                foreach(var trapIndex in Enumerable.Range(0, JournalState.NumTrapEntries))
                    painter.DrawEntry("Trap", trapIndex, Journal.TrapEntries[trapIndex]);
            }
        }
    }
}
