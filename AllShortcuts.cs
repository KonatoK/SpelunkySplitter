using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.Spelunky
{
    public static class AllShortcuts
    {
        public class CharSelectFactory : ISegmentFactory
        {
            public class CharSelect : ISegment
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
                    if (spelunky.CurrentLobbyType != LobbyType.TUTORIAL)
                    {
                        return new SegmentStatus()
                        {
                            Type = SegmentStatusType.ERROR,
                            Message = "The game save must be reset to lock the lobby."
                        };
                    }
                    else if (spelunky.CurrentState != SpelunkyState.CHARACTER_SELECT)
                    {
                        return new SegmentStatus()
                        {
                            Type = SegmentStatusType.ERROR,
                            Message = "The run must begin from the character selection screen."
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
                        LastState == SpelunkyState.CHARACTER_SELECT && LastCharSelectCountdown == 0 && 
                        state == SpelunkyState.CHARACTER_SELECT && charSelectCountdown != 0;

                    LastState = state;
                    LastCharSelectCountdown = charSelectCountdown;

                    return shouldSplit;
                }
            }

            public ISegment Create()
            {
                return new CharSelect();
            }
        }

        public class TutorialFactory : ISegmentFactory
        {
            public class Tutorial : ISegment
            {
                LobbyType? LastLobbyType;

                /*
                    Tutorial segment is only valid when the tutorial is locked or breaking.
                */
                public SegmentStatus CheckStatus(SpelunkyHooks spelunky)
                {
                    if (spelunky.CurrentLobbyType == LobbyType.DOOR_OPEN_NO_GEM)
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
                    if (CheckStatus(spelunky).Type == SegmentStatusType.ERROR)
                        return false;

                    var lobbyType = spelunky.CurrentLobbyType;
                    bool shouldSplit = LastLobbyType == LobbyType.BREAKING && lobbyType == LobbyType.DOOR_OPEN_NO_GEM;
                    LastLobbyType = lobbyType;
                    return shouldSplit;
                }
            }

            public ISegment Create()
            {
                return new Tutorial();
            }
        }

        public class TMSegmentFactory : ISegmentFactory
        {
            TMChapter BeforeChapter;
            TMChapter AfterChapter;
            int BeforeRemaining;
            int AfterRemaining;
            string Item;
            string Area1;
            string Area2;

            public TMSegmentFactory(string item, string area1, string area2, TMChapter ch1, TMChapter ch2, int rem1, int rem2)
            {
                this.BeforeChapter = ch1;
                this.AfterChapter = ch2;
                this.BeforeRemaining = rem1;
                this.AfterRemaining = rem2;
                this.Item = item;
                this.Area1 = area1;
                this.Area2 = area2;
            }

            public class TMSegment : ISegment
            {
                TMSegmentFactory Parent;

                TMChapter? LastChapter;
                int? LastRemaining;

                public TMSegment(TMSegmentFactory parent)
                {
                    this.Parent = parent;
                }

                /*
                    A tunnel man segment is only valid when:
                      1. The current TM chapter <= BeforeChapter.
                      2. If the current TM chapter == BeforeChapter, chapter remainder >= BeforeRemaining
                */ 
                public SegmentStatus CheckStatus(SpelunkyHooks spelunky)
                {
                    var chapter = spelunky.TunnelManChapter;
                    if (chapter > Parent.BeforeChapter
                    || (chapter == Parent.BeforeChapter && spelunky.TunnelManRemaining < Parent.BeforeRemaining))
                    {
                        return new SegmentStatus()
                        {
                            Type = SegmentStatusType.ERROR,
                            Message = "Tunnel Man has already received " + Parent.Item + " between the " + Parent.Area1 + " and " + Parent.Area2 + ": the segment must be skipped."
                        };
                    }
                    else
                    {
                        return new SegmentStatus()
                        {
                            Type = SegmentStatusType.INFO,
                            Message = "Waiting for Tunnel Man to receive " + Parent.Item + " between the " + Parent.Area1 + " and " + Parent.Area2 + "."
                        };
                    }
                }

                public bool Cycle(SpelunkyHooks spelunky)
                {
                    if (CheckStatus(spelunky).Type == SegmentStatusType.ERROR)
                        return false;

                    var chapter = spelunky.TunnelManChapter;
                    var remaining = spelunky.TunnelManRemaining;

                    bool shouldSplit =
                        LastChapter == Parent.BeforeChapter && LastRemaining == Parent.BeforeRemaining &&
                        chapter == Parent.AfterChapter && remaining == Parent.AfterRemaining;

                    LastChapter = chapter;
                    LastRemaining = remaining;

                    return shouldSplit;
                }
            }

            public ISegment Create()
            {
                return new TMSegment(this);
            }
        }

        public class OlmecFactory : ISegmentFactory
        {
            public class Olmec : ISegment
            {
                SpelunkyLevel? LastLevel;
                SpelunkyState? LastState;

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
                    if (CheckStatus(spelunky).Type == SegmentStatusType.ERROR)
                        return false;

                    var level = spelunky.CurrentLevel;
                    var state = spelunky.CurrentState;

                    bool shouldSplit =
                        spelunky.TunnelManChapter == TMChapter.EMPTY_COMPLETED &&
                        LastLevel == SpelunkyLevel.L4_4 && LastState == SpelunkyState.INPUTLOCK_PART &&
                        level == SpelunkyLevel.L4_4 && state == SpelunkyState.VICTORY_CUTSCENE;

                    LastLevel = level;
                    LastState = state;

                    return shouldSplit;
                }
            }
            
            public ISegment Create()
            {
                return new Olmec();
            }
        }

        public static readonly Category Category = new Category("All Shortcuts%", new ISegmentFactory[] {
            new CharSelectFactory(),  // The first segment splits when the timer should start
            new TutorialFactory(),
            new TMSegmentFactory("a bomb", "Mines", "Jungle", TMChapter.MINES_TO_JUNGLE, TMChapter.MINES_TO_JUNGLE, 3, 2),
            new TMSegmentFactory("a rope", "Mines", "Jungle", TMChapter.MINES_TO_JUNGLE, TMChapter.MINES_TO_JUNGLE, 2, 1),
            new TMSegmentFactory("$10000", "Mines", "Jungle", TMChapter.MINES_TO_JUNGLE, TMChapter.EMPTY_POST_MTJ, 1, 0),
            new TMSegmentFactory("two bombs", "Jungle", "Ice Caves", TMChapter.JUNGLE_TO_ICE_CAVES, TMChapter.JUNGLE_TO_ICE_CAVES, 3, 2),
            new TMSegmentFactory("two ropes", "Jungle", "Ice Caves", TMChapter.JUNGLE_TO_ICE_CAVES, TMChapter.JUNGLE_TO_ICE_CAVES, 2, 1),
            new TMSegmentFactory("a shotgun", "Jungle", "Ice Caves", TMChapter.JUNGLE_TO_ICE_CAVES, TMChapter.EMPTY_POST_JTIC, 1, 0),
            new TMSegmentFactory("three bombs", "Ice Caves", "Temple", TMChapter.ICE_CAVES_TO_TEMPLE, TMChapter.ICE_CAVES_TO_TEMPLE, 3, 2),
            new TMSegmentFactory("three ropes", "Ice Caves", "Temple", TMChapter.ICE_CAVES_TO_TEMPLE, TMChapter.ICE_CAVES_TO_TEMPLE, 2, 1),
            new TMSegmentFactory("a key", "Ice Caves", "Temple", TMChapter.ICE_CAVES_TO_TEMPLE, TMChapter.EMPTY_COMPLETED, 1, 0),
            new OlmecFactory()
        });
    }
}
