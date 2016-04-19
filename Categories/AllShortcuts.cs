using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.Spelunky.Categories
{
    public static class AllShortcuts
    {
        public static readonly string Name = "All Shortcuts + Olmec%";

        public static readonly ISegmentFactory[] SegmentFactories =
        {
            new SegmentFactories.CharSelectSegmentFactory(),
            new SegmentFactories.TutorialSegmentFactory(),
            new SegmentFactories.TunnelManSegmentFactory("a bomb", "Mines", "Jungle", TunnelManChapter.MINES_TO_JUNGLE, TunnelManChapter.MINES_TO_JUNGLE, 3, 2),
            new SegmentFactories.TunnelManSegmentFactory("a rope", "Mines", "Jungle", TunnelManChapter.MINES_TO_JUNGLE, TunnelManChapter.MINES_TO_JUNGLE, 2, 1),
            new SegmentFactories.TunnelManSegmentFactory("$10000", "Mines", "Jungle", TunnelManChapter.MINES_TO_JUNGLE, TunnelManChapter.EMPTY_POST_MTJ, 1, 0),
            new SegmentFactories.TunnelManSegmentFactory("two bombs", "Jungle", "Ice Caves", TunnelManChapter.JUNGLE_TO_ICE_CAVES, TunnelManChapter.JUNGLE_TO_ICE_CAVES, 3, 2),
            new SegmentFactories.TunnelManSegmentFactory("two ropes", "Jungle", "Ice Caves", TunnelManChapter.JUNGLE_TO_ICE_CAVES, TunnelManChapter.JUNGLE_TO_ICE_CAVES, 2, 1),
            new SegmentFactories.TunnelManSegmentFactory("a shotgun", "Jungle", "Ice Caves", TunnelManChapter.JUNGLE_TO_ICE_CAVES, TunnelManChapter.EMPTY_POST_JTIC, 1, 0),
            new SegmentFactories.TunnelManSegmentFactory("3 bombs", "Ice Caves", "Temple", TunnelManChapter.ICE_CAVES_TO_TEMPLE, TunnelManChapter.ICE_CAVES_TO_TEMPLE, 3, 2),
            new SegmentFactories.TunnelManSegmentFactory("3 ropes", "Ice Caves", "Temple", TunnelManChapter.ICE_CAVES_TO_TEMPLE, TunnelManChapter.ICE_CAVES_TO_TEMPLE, 2, 1),
            new SegmentFactories.TunnelManSegmentFactory("a key", "Ice Caves", "Temple", TunnelManChapter.ICE_CAVES_TO_TEMPLE, TunnelManChapter.EMPTY_COMPLETED, 1, 0),
            new SegmentFactories.OlmecSegmentFactory()
        };
    }
}
