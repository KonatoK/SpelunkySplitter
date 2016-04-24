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
    public partial class JournalTracker : Utilities.Forms.SquareResizableForm
    {
        public static readonly Size InitialClientSize = new Size(348, 340);
        private JournalVisualizer JournalVisualizer;

        public JournalTracker() : base(InitialClientSize.Height / (double)InitialClientSize.Width)
        {
            JournalVisualizer = new JournalVisualizer();

            InitializeComponent();

            Utilities.Forms.MenuUtils.DisableCloseButton(this);
            JournalVisualizer.Location = new Point(0, 0);
            JournalVisualizer.Size = ClientSize;
            Controls.Add(JournalVisualizer);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
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
            public int EntryRenderHeightPx { get; }
            public int EntryMarginPx { get; }

            private Graphics Graphics;
            private Point CurrentEntryDrawPosition;
            public int ColumnRenderWidth;
            private int Height;

            public JournalPainter(Graphics graphics, int entriesPerColumn, int entryMarginPx, int height)
            {
                Graphics = graphics;
                Height = height;

                EntryMarginPx = entryMarginPx;
                EntryRenderHeightPx = (Height - (entriesPerColumn+1)*EntryMarginPx) / entriesPerColumn;

                CurrentEntryDrawPosition = new Point(EntryMarginPx, EntryMarginPx);
                ColumnRenderWidth = 0;
            }

            public void DrawEntry(string entryType, int entryIndex, bool entryIsUnlocked)
            {
                var entryImageResourceName = $"{entryType}_{entryIndex}" + (!entryIsUnlocked ? "" : "_Inactive");
                var entryImage = (Image)Properties.Resources.ResourceManager.GetObject(entryImageResourceName);
                var entryImageScale = EntryRenderHeightPx / (float)entryImage.Height;
                Graphics.DrawImage(entryImage, CurrentEntryDrawPosition.X, CurrentEntryDrawPosition.Y, ColumnRenderWidth, EntryRenderHeightPx);
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
                CurrentEntryDrawPosition.X += ColumnRenderWidth + EntryMarginPx;
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

        private const int EntriesPerColumn = 12;
        private const int EntryMarginPx = 3;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if(MaybeJournal != null)
            {
                var Journal = MaybeJournal;
                // Height is wrongly displaced by JournalTracker.InitialAutoScaleDimensions.Height-26 (due to WinForms)
                var painter = new JournalPainter(e.Graphics, EntriesPerColumn, EntryMarginPx, Height);
                painter.ColumnRenderWidth = painter.EntryRenderHeightPx*Properties.Resources.Place_0.Width/Properties.Resources.Place_0.Height;
                foreach(var placeIndex in Enumerable.Range(0, JournalState.NumPlaceEntries))
                    painter.DrawEntry("Place", placeIndex, Journal.PlaceEntries[placeIndex]);
                painter.NewColumn();
                painter.ColumnRenderWidth = painter.EntryRenderHeightPx*Properties.Resources.Monster_0.Width/Properties.Resources.Monster_0.Height;
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
