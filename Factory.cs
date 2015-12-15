﻿using LiveSplit.Model;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.Spelunky
{
    public class Factory : IComponentFactory
    {
        public string ComponentName
        {
            get { return "Spelunky Auto Splitter";  }
        }

        public IComponent Create(LiveSplitState state)
        {
            return new Component(state, false);
        }

        public string UpdateName
        {
            get { return ComponentName;  }
        }

        public string XMLURL
        {
            get { return "http://sashavol.com/frozlunky/autosplitter/update.LiveSplit.Spelunky.xml";  }
        }

        public String UpdateURL
        {
            get { return "http://sashavol.com/frozlunky/autosplitter/";  } // TODO Make this update site not be an index page
        }

        public Version Version
        {
            get { return Version.Parse("1.0.0");  }
        }

        public string Description
        {
            get { return "Autosplitter for All-Shortcuts%";  }
        }

        public ComponentCategory Category
        {
            get { return ComponentCategory.Timer; }
        }
    }
}