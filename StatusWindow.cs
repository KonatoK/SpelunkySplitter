using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LiveSplit.Spelunky
{
    public partial class StatusWindow : Form
    {
        SegmentStatusType? LastStatusType;

        public StatusWindow()
        {
            InitializeComponent();
        }

        public string CurrentRun
        {
            get { return CurrentRunLabel.Text; }
            set { CurrentRunLabel.Text = value; }
        }

        public void SetStatus(SegmentStatusType type, string text)
        {
            switch(type)
            {
                case SegmentStatusType.INFO:
                    SetInfoStatus(text);
                    break;
                case SegmentStatusType.ERROR:
                    SetErrorStatus(text);
                    break;
            }
        }

        public void SetInfoStatus(string text)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate () { SetInfoStatus(text); });
                return;
            }

            if (LastStatusType == SegmentStatusType.INFO && StatusLabel.Text.Equals(text))
                return;

            StatusLabel.ForeColor = Color.Black;
            StatusLabel.Text = text;
            LastStatusType = SegmentStatusType.INFO;
        }

        public void SetErrorStatus(string text)
        {
            if(InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate () { SetErrorStatus(text); });
                return;
            }

            if (LastStatusType == SegmentStatusType.ERROR && StatusLabel.Text.Equals(text))
                return;

            StatusLabel.ForeColor = Color.Red;
            StatusLabel.Text = text;
            LastStatusType = SegmentStatusType.ERROR;
        }
    }
}
