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

        private SegmentStatus CheckStatus(CharactersState chars, int playCount)
        {
            var numEntriesUnlocked = chars.NumUnlockedCharacters;
            if (numEntriesUnlocked > 16)
            {
                return new SegmentStatus()
                {
                    Type = SegmentStatusType.ERROR,
                    Message = $"More than {TargetUnlockCount} characters have been unlocked."
                };
            }
            else
            {
                return new SegmentStatus()
                {
                    Type = SegmentStatusType.INFO,
                    Message = $"Waiting for characters ({chars.NumUnlockedCharacters}/{CharactersState.NumCharacters}). {NextCharacterSpawnChance(chars, playCount)}"
                };
            }
        }

        public SegmentStatus CheckStatus(SpelunkyHooks spelunky)
        {
            return CheckStatus(spelunky.CharactersState, spelunky.PlayCount);
        }

        public bool Cycle(SpelunkyHooks spelunky)
        {
            var chars = spelunky.CharactersState;
            if (CheckStatus(chars, spelunky.PlayCount).Type == SegmentStatusType.ERROR)
                return false;
            else if (chars.NumUnlockedCharacters >= TargetUnlockCount)
                return true;
            else
                return false;
        }

        private String NextCharacterSpawnChance(CharactersState chars, int playCount)
        {
            switch (chars.NumUnlockedRandos)
            {
                case 0:
                    return $"Play count: {playCount} (Mines Spawn: {(100f / Math.Max(9, 51 - playCount)).ToString("0.00")}%)";
                case 1:
                    return $"Play count: {playCount} (Jungle Spawn: {(100f / Math.Max(9, 101 - playCount)).ToString("0.00")}%)";
                case 2:
                    return $"Play count: {playCount} (Ice Spawn: {(100f / Math.Max(9, 201 - playCount)).ToString("0.00")}%)";
                case 3:
                    return $"Play count: {playCount} (Temple Spawn: {(100f / Math.Max(9, 301 - playCount)).ToString("0.00")}%)";
            }
            return "";
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
