using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.Spelunky.Categories
{
    public static class AllJournalEntries
    {
        public static readonly string Name = "All Journal Entries%";

        public static readonly ISegmentFactory[] SegmentFactories =
        {
            new SegmentFactories.CharSelectSegmentFactory(),
            new SegmentFactories.TutorialSegmentFactory(),
            new SegmentFactories.JournalEntryProgressSegmentFactory(JournalState.NumEntries/2),
            new SegmentFactories.JournalEntryProgressSegmentFactory(JournalState.NumEntries)
        };
    }
}
