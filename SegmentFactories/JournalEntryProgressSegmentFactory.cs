using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.Spelunky.SegmentFactories
{
    public class JournalEntryProgressSegment : ISegment
    {
        private int TargetEntryUnlockCount;
        private string TargetPercTotalStr => (100 * TargetEntryUnlockCount / JournalState.NUM_ENTRIES).ToString();

        public JournalEntryProgressSegment(int targetEntryUnlockCount)
        {
            TargetEntryUnlockCount = targetEntryUnlockCount;
        }

        private SegmentStatus CheckStatus(JournalState journal)
        {
            var numEntriesUnlocked = journal.NumUnlockedEntries;
            if(numEntriesUnlocked > TargetEntryUnlockCount)
            {
                return new SegmentStatus()
                {
                    Type = SegmentStatusType.ERROR,
                    Message = $"More than {TargetEntryUnlockCount} entries have been unlocked."
                };
            }
            else
            {
                return new SegmentStatus()
                {
                    Type = SegmentStatusType.INFO,
                    Message = $"Waiting for journal ({TargetEntryUnlockCount - numEntriesUnlocked} entries until {TargetPercTotalStr}% unlocked)."
                };
            }
        }

        public SegmentStatus CheckStatus(SpelunkyHooks spelunky)
        {
            return CheckStatus(spelunky.JournalState);
        }

        public bool Cycle(SpelunkyHooks spelunky)
        {
            var journal = spelunky.JournalState;
            if(CheckStatus(journal).Type == SegmentStatusType.ERROR)
                return false;
            else if(journal.NumUnlockedEntries >= TargetEntryUnlockCount)
                return true;
            else
                return false;
        }
    }

    public class JournalEntryProgressSegmentFactory : ISegmentFactory
    {
        private int TargetEntryUnlockCount;
        private string TargetPercTotalStr => (100 * TargetEntryUnlockCount / JournalState.NUM_ENTRIES).ToString();

        public JournalEntryProgressSegmentFactory(int targetEntryUnlockCount)
        {
            TargetEntryUnlockCount = targetEntryUnlockCount;
        }

        public ISegment NewInstance()
        {
            return new JournalEntryProgressSegment(TargetEntryUnlockCount);
        }
    }
}
