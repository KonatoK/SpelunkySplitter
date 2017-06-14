using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.Spelunky.SegmentFactories
{
    public class AllCharactersSegment : ISegment
    {
        private int TargetUnlockCount = 16; // I doubt they'll add more characters, could be changed to enum size though

        private SegmentStatus CheckStatus(CharactersState chars)
        {
            var numEntriesUnlocked = chars.NumUnlockedCharacters;
            if (numEntriesUnlocked > 16)
            {
                return new SegmentStatus()
                {
                    Type = SegmentStatusType.ERROR,
                    Message = $"More than {TargetUnlockCount} entries have been unlocked."
                };
            }
            else
            {
                return new SegmentStatus()
                {
                    Type = SegmentStatusType.INFO,
                    Message = $"Waiting for characters to be unlocked ({chars.NumUnlockedCharacters}/{CharactersState.NumCharacters} characters)."
                };
            }
        }

        public SegmentStatus CheckStatus(SpelunkyHooks spelunky)
        {
            return CheckStatus(spelunky.CharactersState);
        }

        public bool Cycle(SpelunkyHooks spelunky)
        {
            var chars = spelunky.CharactersState;
            if (CheckStatus(chars).Type == SegmentStatusType.ERROR)
                return false;
            else if (chars.NumUnlockedCharacters >= TargetUnlockCount)
                return true;
            else
                return false;
        }
    }

    public class AllCharactersSegmentFactory : ISegmentFactory
    {
        public ISegment NewInstance()
        {
            return new AllCharactersSegment();
        }
    }
}
