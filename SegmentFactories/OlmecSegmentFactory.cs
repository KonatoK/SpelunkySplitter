using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.Spelunky.SegmentFactories
{
    public class OlmecSegment : ISegment
    {
        SpelunkyLevel? LastLevel;

        public SegmentStatus CheckStatus(SpelunkyHooks spelunky)
        {
            return new SegmentStatus()
            {
                Type = SegmentStatusType.INFO,
                Message = "Waiting for Olmec completion."
            };
        }

        public bool Cycle(SpelunkyHooks spelunky)
        {
            if(CheckStatus(spelunky).Type == SegmentStatusType.ERROR)
                return false;

            var level = spelunky.CurrentLevel;
            var state = spelunky.CurrentState;

            bool shouldSplit =
                spelunky.TunnelManChapter == TunnelManChapter.EMPTY_COMPLETED &&
                spelunky.CurrentState == SpelunkyState.VICTORY_CUTSCENE &&
                LastLevel == SpelunkyLevel.L4_4 && level == SpelunkyLevel.L4_4;

            LastLevel = level;

            return shouldSplit;
        }
    }

    public class OlmecSegmentFactory : ISegmentFactory
    {
        public ISegment NewInstance()
        {
            return new OlmecSegment();
        }
    }
}
