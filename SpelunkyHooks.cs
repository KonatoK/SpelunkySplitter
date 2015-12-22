using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.Spelunky
{
    public enum SpelunkyState : int
    {
        INPUTLOCK_PART_2 = 2,
        INPUTLOCK_PART_3 = 3,
        SPLASH_SCREEN = 5,
        CHARACTER_SELECT = 17, 
        VICTORY_CUTSCENE = 18,
        LOBBY = 22
    }

    public enum SpelunkyLevel : int
    {
        EMPTY = 0,
        L0_1_OR_1_1 = 1,
        L0_2_OR_1_2 = 2,
        L0_3_OR_1_3 = 3,
        L1_4 = 4,
        L2_1 = 5,
        L2_2 = 6,
        L2_3 = 7,
        L2_4 = 8,
        L3_1 = 9,
        L3_2 = 10,
        L3_3 = 11,
        L3_4 = 12,
        L4_1 = 13,
        L4_2 = 14,
        L4_3 = 15,
        L4_4 = 16,
        L5_1 = 17,
        L5_2 = 18,
        L5_3 = 19,
        L5_4 = 20
    }

    public enum TMChapter : int
    {
        EMPTY_UNENCOUNTERED = 0,
        MINES_TO_JUNGLE = 1,
        EMPTY_POST_MTJ = 2,
        JUNGLE_TO_ICE_CAVES = 3,
        EMPTY_POST_JTIC = 4,
        ICE_CAVES_TO_TEMPLE = 5,
        EMPTY_COMPLETED = 6
    }
    
    public enum LobbyType : int
    {
        TUTORIAL = 0,
        BREAKING = 1,
        DOOR_OPEN_NO_GEM = 2
    }

    public class SpelunkyHooks : IDisposable
    {
        private const int BASE_GAME_OFFSET = 0x1384b4;
        private const int GAME_STATE_OFFSET = 0x58;
        private const int GAME_LEVEL_OFFSET = 0x4405d4;
        private const int GAME_GFX_OFFSET = 0x4c; // <- This will give a struct containing data related to animation (though the semantics are not completely clear)
        private const int GFX_CHAR_SELECT_COUNTDOWN_OFFSET = 0x122bec;
        private const int GAME_LOBBY_MODE = 0x445be0;
        private const int GAME_TM_CHAPTER_OFFSET = 0x445be4;
        private const int GAME_TM_REMAINING_OFFSET = 0x445be8;

        public RawProcess Process { get; private set; }


        public SpelunkyHooks(RawProcess process)
        {
            this.Process = process;
        }

        public string GameDirectoryPath => Path.GetDirectoryName(Process.FilePath);
        public string GameSavePath => GameDirectoryPath + @"\Data\spelunky_save.sav";

        private int Game => Process.ReadInt32(Process.BaseAddress + BASE_GAME_OFFSET);
        private int Gfx => Process.ReadInt32(Game + GAME_GFX_OFFSET);
        public int CharSelectCountdown => Process.ReadInt32(Gfx + GFX_CHAR_SELECT_COUNTDOWN_OFFSET);
        public SpelunkyState CurrentState => (SpelunkyState)Process.ReadInt32(Game + GAME_STATE_OFFSET);
        public SpelunkyLevel CurrentLevel => (SpelunkyLevel)Process.ReadInt32(Game + GAME_LEVEL_OFFSET);
        public TMChapter TunnelManChapter => (TMChapter)Process.ReadInt32(Game + GAME_TM_CHAPTER_OFFSET);
        public LobbyType CurrentLobbyType => (LobbyType)Process.ReadInt32(Game + GAME_LOBBY_MODE);
        public bool Invalidated => Process.HasExited;

        /*
            3 => 1/2/3 Rope(s)
            2 => 1/2/3 Bomb(s)
            1 => $10000/Shotgun/Key
            0 => Nothing
        */
        public int TunnelManRemaining => Process.ReadInt32(Game + GAME_TM_REMAINING_OFFSET);

        public void Dispose()
        {
            Process.Dispose();
        }
    }
}
