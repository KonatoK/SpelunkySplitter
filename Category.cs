using System;
using System.Reflection;
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
        ISegment NewInstance();
    }

    public static class Category
    {
        public static string GetFriendlyName(Type categoryType)
        {
            return (string)categoryType.GetField("Name").GetValue(null);
        }

        public static ISegment[] NewSegmentInstances(Type categoryType)
        {
            var segmentFactories = (ISegmentFactory[])categoryType.GetField("SegmentFactories").GetValue(null);
            return segmentFactories.Select(segmentFactory => segmentFactory.NewInstance()).ToArray();
        }
    }
}
