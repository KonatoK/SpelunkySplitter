using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.Spelunky.SegmentFactories
{
    public class TutorialSegment : ISegment
    {
        LobbyType? LastLobbyType;

        /*
            Tutorial segment is only valid when the tutorial is locked or breaking.
        */
        public SegmentStatus CheckStatus(SpelunkyHooks spelunky)
        {
            if(LastLobbyType == LobbyType.DoorOpenNoGem && spelunky.CurrentLobbyType == LobbyType.DoorOpenNoGem)
            {
                return new SegmentStatus()
                {
                    Type = SegmentStatusType.ERROR,
                    Message = "Lobby must be locked during the tutorial segment."
                };
            }
            else
            {
                return new SegmentStatus()
                {
                    Type = SegmentStatusType.INFO,
                    Message = "Waiting for tutorial completion."
                };
            }
        }

        public bool Cycle(SpelunkyHooks spelunky)
        {
            if(CheckStatus(spelunky).Type == SegmentStatusType.ERROR)
                return false;

            var lobbyType = spelunky.CurrentLobbyType;
            bool shouldSplit = LastLobbyType == LobbyType.Breaking && lobbyType == LobbyType.DoorOpenNoGem;
            LastLobbyType = lobbyType;
            return shouldSplit;
        }
    }

    public class TutorialSegmentFactory : ISegmentFactory
    {
        public ISegment NewInstance()
        {
            return new TutorialSegment();
        }
    }
}
