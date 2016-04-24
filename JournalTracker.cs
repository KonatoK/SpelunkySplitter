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
        public static readonly Size InitialSize = new Size(366, 379);
        private static double RequiredHeightWidthRatio => InitialSize.Height / (double)InitialSize.Width;

        private JournalVisualizer JournalVisualizer;

        public JournalTracker()
        {
            JournalVisualizer = new JournalVisualizer();

            InitializeComponent();

            Utilities.Forms.DisableCloseButton(this);
            JournalVisualizer.Location = new Point(0, 0);
            JournalVisualizer.Size = ClientSize;
            Controls.Add(JournalVisualizer);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            var currentHeightWidthRatio = Height / (double)Width;
            if(currentHeightWidthRatio != RequiredHeightWidthRatio)
                Size = new Size(Width, (int)(RequiredHeightWidthRatio * Width));
            JournalVisualizer.Invalidate();
            JournalVisualizer.Size = ClientSize;
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
            private const int NUM_ENTRIES_VERTICAL = 12;

            private int EntryRenderHeightPx;
            private int EntryMarginPx;

            private Graphics Graphics;
            private Point CurrentEntryDrawPosition;
            private int MaxColumnImageWidth;
            private int Height;

            public JournalPainter(Graphics graphics, int height)
            {
                Graphics = graphics;
                Height = height;

                EntryMarginPx = 3;
                EntryRenderHeightPx = (Height - (NUM_ENTRIES_VERTICAL+1)*EntryMarginPx) / (NUM_ENTRIES_VERTICAL);

                CurrentEntryDrawPosition = new Point(EntryMarginPx, EntryMarginPx);
                MaxColumnImageWidth = 0;
            }

            public void DrawEntry(string entryType, int entryIndex, bool entryIsUnlocked)
            {
                var entryImageResourceName = $"{entryType}_{entryIndex}" + (!entryIsUnlocked ? "" : "_Inactive");
                var entryImage = (Image)Properties.Resources.ResourceManager.GetObject(entryImageResourceName);
                var entryImageScale = EntryRenderHeightPx / (float)entryImage.Height;
                var entryImageRenderWidth = entryImageScale * entryImage.Width;
                Graphics.DrawImage(entryImage, CurrentEntryDrawPosition.X, CurrentEntryDrawPosition.Y, entryImageRenderWidth, EntryRenderHeightPx);
                MaxColumnImageWidth = Math.Max((int)entryImageRenderWidth, MaxColumnImageWidth);
                AdvanceToNextCell();
            }

            private void AdvanceToNextCell()
            {
                CurrentEntryDrawPosition.Y += EntryRenderHeightPx + EntryMarginPx;
                if(CurrentEntryDrawPosition.Y + EntryRenderHeightPx + EntryMarginPx >= Height)
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
                // Height is wrongly displaced by JournalTracker.InitialAutoScaleDimensions.Height-26 (due to WinForms)
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
