using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.Spelunky
{
    public enum SegmentStatusType
    {
        INFO,
        ERROR
    }

    public class SegmentStatus
    {
        public SegmentStatusType Type;
        public string Message;
    }

    public interface ISegment
    {
        SegmentStatus CheckStatus(SpelunkyHooks spelunky);
        bool Cycle(SpelunkyHooks spelunky);
    }

    public interface ISegmentFactory
    {
        ISegment Create();
    }

    public class Category
    {
        public string Name { get; private set; }
        public ISegmentFactory[] SegmentFactories { get; private set; }

        public Category(string name, ISegmentFactory[] segmentFactories)
        {
            this.Name = name;
            this.SegmentFactories = segmentFactories;
        }

        public ISegment[] NewInstance()
        {
            ISegment[] segments = new ISegment[SegmentFactories.Length];
            for(var i = 0; i < SegmentFactories.Length; ++i)
                segments[i] = SegmentFactories[i].Create();
            return segments;
        }
    }
}
