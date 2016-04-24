using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace LiveSplit.Spelunky.Utilities.Forms
{
    public delegate double DesiredHeightWidthRatioComputer(Size size);

    public class SquareResizableForm : Form
    {
        private double DesiredClientHeightWidthRatio;

        public SquareResizableForm(double desiredClientHeightWidthRatio)
        {
            DesiredClientHeightWidthRatio = desiredClientHeightWidthRatio;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            var currentHeightWidthRatio = ClientSize.Height / (double)ClientSize.Width;
            if(Math.Abs(DesiredClientHeightWidthRatio - currentHeightWidthRatio) >= double.Epsilon)
                ClientSize = new Size(ClientSize.Width, (int)Math.Ceiling(DesiredClientHeightWidthRatio * ClientSize.Width));
        }
    }
}
