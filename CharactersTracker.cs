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
    public partial class CharactersTracker : Utilities.Forms.SquareResizableForm
    {
        public static readonly Size InitialClientSize = new Size(640, 40);
        private CharactersVisualizer CharactersVisualizer;

        public CharactersTracker() : base(InitialClientSize.Height / (double)InitialClientSize.Width)
        {
            CharactersVisualizer = new CharactersVisualizer();

            InitializeComponent();

            Utilities.Forms.MenuUtils.DisableCloseButton(this);
            CharactersVisualizer.Location = new Point(0, 0);
            CharactersVisualizer.Size = ClientSize;
            Controls.Add(CharactersVisualizer);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CharactersVisualizer.Invalidate();
            CharactersVisualizer.Size = ClientSize;
        }

        public void Update(SpelunkyHooks spelunky)
        {
            CharactersVisualizer.SetCharacters(spelunky.CharactersState);
        }
    }

    public class CharactersVisualizer : Control
    {
        CharactersState MaybeCharacters;

        public CharactersVisualizer()
        {
            DoubleBuffered = true;
        }

        private class CharactersPainter
        {
            public int EntryRenderHeightPx { get; }
            public int EntryMarginPx { get; }

            private Graphics Graphics;
            private Point CurrentEntryDrawPosition;
            public int ColumnRenderWidth;
            private int Height;

            public CharactersPainter(Graphics graphics, int entriesPerColumn, int entryMarginPx, int height)
            {
                Graphics = graphics;
                Height = height;

                EntryMarginPx = entryMarginPx;
                EntryRenderHeightPx = (Height - (entriesPerColumn + 1) * EntryMarginPx) / entriesPerColumn;

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

        public void SetCharacters(CharactersState journal)
        {
            if(MaybeCharacters == null || !MaybeCharacters.Equals(journal))
            {
                MaybeCharacters = journal;
                Invalidate();
            }
        }

        private const int EntriesPerColumn = 1;
        private const int EntryMarginPx = 0;

        private int[] CharacterOrder = {2, 4, 6, 7, 9, 12, 5, 11, 13, 1, 8, 14, 10, 3, 15, 0};

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if(MaybeCharacters != null)
            {
                var Chars = MaybeCharacters;
                // Height is wrongly displaced by CharactersTracker.InitialAutoScaleDimensions.Height-26 (due to WinForms)
                var painter = new CharactersPainter(e.Graphics, EntriesPerColumn, EntryMarginPx, Height);
                painter.ColumnRenderWidth = painter.EntryRenderHeightPx * Properties.Resources.Char_0.Width / Properties.Resources.Char_0.Height;
                foreach(var placeIndex in CharacterOrder)
                    painter.DrawEntry("Char", placeIndex, Chars.UnlockedChars[placeIndex]);
            }
        }
    }
}
