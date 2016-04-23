using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.Spelunky.SegmentFactories
{
    public class CharSelectSegment : ISegment
    {
        SpelunkyState? LastState;
        int? LastCharSelectCountdown;

        /*
            CharSelect segment is only valid when:
              1. The lobby is locked. 
            & 2. Game is in the character selection screen.
        */
        public SegmentStatus CheckStatus(SpelunkyHooks spelunky)
        {
            // Do not do game save checks in the splash screen as the save file may not be reloaded yet
            if(spelunky.CurrentLobbyType != LobbyType.Tutorial && spelunky.CurrentState != SpelunkyState.SplashScreen)
            {
                return new SegmentStatus()
                {
                    Type = SegmentStatusType.ERROR,
                    Message = "The game save must be reset to lock the lobby."
                };
            }
            else if(spelunky.CurrentState != SpelunkyState.CharacterSelect)
            {
                return new SegmentStatus()
                {
                    Type = SegmentStatusType.INFO,
                    Message = "Waiting for character selection screen."
                };
            }
            else
            {
                return new SegmentStatus()
                {
                    Type = SegmentStatusType.INFO,
                    Message = "Waiting for character selection."
                };
            }
        }

        public bool Cycle(SpelunkyHooks spelunky)
        {
            if(CheckStatus(spelunky).Type == SegmentStatusType.ERROR)
                return false;

            var state = spelunky.CurrentState;
            var charSelectCountdown = spelunky.CharSelectCountdown;

            bool shouldSplit =
                LastState == SpelunkyState.CharacterSelect && LastCharSelectCountdown == 0 &&
                state == SpelunkyState.CharacterSelect && charSelectCountdown != 0;

            LastState = state;
            LastCharSelectCountdown = charSelectCountdown;

            return shouldSplit;
        }
    }

    public class CharSelectSegmentFactory : ISegmentFactory
    {
        public ISegment NewInstance()
        {
            return new CharSelectSegment();
        }
    }
}
