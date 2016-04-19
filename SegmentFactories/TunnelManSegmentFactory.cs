using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.Spelunky.SegmentFactories
{
    public class TunnelManSegment : ISegment
    {
        string Item;
        string Area1;
        string Area2;
        TunnelManChapter BeforeChapter;
        TunnelManChapter AfterChapter;
        int BeforeRemaining;
        int AfterRemaining;

        TunnelManChapter? LastChapter;
        int? LastRemaining;

        public TunnelManSegment(string item, string area1, string area2, TunnelManChapter beforeChapter, TunnelManChapter afterChapter, int beforeRemaining, int afterRemaining)
        {
            Item = item;
            Area1 = area1;
            Area2 = area2;
            BeforeChapter = beforeChapter;
            AfterChapter = afterChapter;
            BeforeRemaining = beforeRemaining;
            AfterRemaining = afterRemaining;
        }

        /*
            A tunnel man segment is only valid when:
              1. The current TM chapter <= BeforeChapter.
              2. If the current TM chapter == BeforeChapter, chapter remainder >= BeforeRemaining
        */
        public SegmentStatus CheckStatus(SpelunkyHooks spelunky)
        {
            var chapter = spelunky.TunnelManChapter;
            if((chapter > BeforeChapter && LastChapter > BeforeChapter)
            || ((chapter == BeforeChapter && LastChapter == BeforeChapter) && (spelunky.TunnelManRemaining < BeforeRemaining && LastRemaining < BeforeRemaining)))
            {
                return new SegmentStatus()
                {
                    Type = SegmentStatusType.ERROR,
                    Message = $"Tunnel Man has already received {Item} between the {Area1} and {Area2}."
                };
            }
            else
            {
                return new SegmentStatus()
                {
                    Type = SegmentStatusType.INFO,
                    Message = $"Waiting for Tunnel Man to receive {Item} between the {Area1} and {Area2}."
                };
            }
        }

        public bool Cycle(SpelunkyHooks spelunky)
        {
            if(CheckStatus(spelunky).Type == SegmentStatusType.ERROR)
                return false;

            var chapter = spelunky.TunnelManChapter;
            var remaining = spelunky.TunnelManRemaining;

            bool shouldSplit =
                LastChapter == BeforeChapter && LastRemaining == BeforeRemaining &&
                chapter == AfterChapter && remaining == AfterRemaining;

            LastChapter = chapter;
            LastRemaining = remaining;

            return shouldSplit;
        }
    }

    public class TunnelManSegmentFactory : ISegmentFactory
    {
        string Item;
        string Area1;
        string Area2;
        TunnelManChapter BeforeChapter;
        TunnelManChapter AfterChapter;
        int BeforeRemaining;
        int AfterRemaining;

        public TunnelManSegmentFactory(string item, string area1, string area2, TunnelManChapter beforeChapter, TunnelManChapter afterChapter, int beforeRemaining, int afterRemaining)
        {
            Item = item;
            Area1 = area1;
            Area2 = area2;
            BeforeChapter = beforeChapter;
            AfterChapter = afterChapter;
            BeforeRemaining = beforeRemaining;
            AfterRemaining = afterRemaining;
        }

        public ISegment NewInstance()
        {
            return new TunnelManSegment(Item, Area1, Area2, BeforeChapter, AfterChapter, BeforeRemaining, AfterRemaining);
        }
    }
}
