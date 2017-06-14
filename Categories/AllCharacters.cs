using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.Spelunky.Categories
{
    public static class AllCharacters
    {
        public static readonly string Name = "All Characters%";

        public static readonly ISegmentFactory[] SegmentFactories =
        {
            new SegmentFactories.CharSelectSegmentFactory(),
            new SegmentFactories.TutorialSegmentFactory(),
            new SegmentFactories.AllCharactersSegmentFactory()
        };
    }
}
