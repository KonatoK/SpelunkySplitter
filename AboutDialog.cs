using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LiveSplit.Spelunky
{
    public partial class AboutDialog : Form
    {
        public AboutDialog()
        {
            InitializeComponent();
            TitleLabel.Text = "SpelunkySplitter v" + new Factory().Version.ToString();
            DevelopedBySashavol2.LinkClicked += DevelopedBySashavol2_LinkClicked;
            OKButton.Click += OKButton_Click;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DevelopedBySashavol2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://sashavol.com");
        }
    }
}
