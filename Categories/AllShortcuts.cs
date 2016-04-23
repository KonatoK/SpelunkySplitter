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
            new SegmentFactories.TunnelManSegmentFactory("a bomb", "Mines", "Jungle", TunnelManChapter.MinesToJungle, TunnelManChapter.MinesToJungle, 3, 2),
            new SegmentFactories.TunnelManSegmentFactory("a rope", "Mines", "Jungle", TunnelManChapter.MinesToJungle, TunnelManChapter.MinesToJungle, 2, 1),
            new SegmentFactories.TunnelManSegmentFactory("$10000", "Mines", "Jungle", TunnelManChapter.MinesToJungle, TunnelManChapter.EmptyPostMtj, 1, 0),
            new SegmentFactories.TunnelManSegmentFactory("two bombs", "Jungle", "Ice Caves", TunnelManChapter.JungleToIceCaves, TunnelManChapter.JungleToIceCaves, 3, 2),
            new SegmentFactories.TunnelManSegmentFactory("two ropes", "Jungle", "Ice Caves", TunnelManChapter.JungleToIceCaves, TunnelManChapter.JungleToIceCaves, 2, 1),
            new SegmentFactories.TunnelManSegmentFactory("a shotgun", "Jungle", "Ice Caves", TunnelManChapter.JungleToIceCaves, TunnelManChapter.EmptyPostJtic, 1, 0),
            new SegmentFactories.TunnelManSegmentFactory("3 bombs", "Ice Caves", "Temple", TunnelManChapter.IceCavesToTemple, TunnelManChapter.IceCavesToTemple, 3, 2),
            new SegmentFactories.TunnelManSegmentFactory("3 ropes", "Ice Caves", "Temple", TunnelManChapter.IceCavesToTemple, TunnelManChapter.IceCavesToTemple, 2, 1),
            new SegmentFactories.TunnelManSegmentFactory("a key", "Ice Caves", "Temple", TunnelManChapter.IceCavesToTemple, TunnelManChapter.EmptyCompleted, 1, 0),
            new SegmentFactories.OlmecSegmentFactory()
        };
    }
}
