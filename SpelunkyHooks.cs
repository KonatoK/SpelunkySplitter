using System;
using System.IO;
using System.Diagnostics;
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

    public enum TunnelManChapter : int
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

    public enum PlaceEntry : int
    {
        THE_MINES = 0,
        THE_JUNGLE = 1,
        THE_ICE_CAVES = 2,
        THE_TEMPLE = 3,
        HELL = 4,
        THE_HAUNTED_CASTLE = 5,
        THE_BLACK_MARKET = 6,
        THE_WORM = 7,
        THE_MOTHERSHIP = 8,
        THE_CITY_OF_GOLD = 9
    }

    public enum MonsterEntry : int
    {
        SNAKE = 0,
        COBRA = 1,
        BAT = 2,
        SPIDER = 3,
        SPINNER_SPIDER = 4,
        GIANT_SPIDER = 5,
        SKELETON = 6,
        SCORPION = 7,
        CAVEMAN = 8,
        DAMSEL = 9,
        SHOPKEEPER = 10,
        THE_TUNNEL_MAN = 11,
        SCARAB = 12,
        TIKI_MAN = 13,
        FROG = 14,
        FIRE_FROG = 15,
        GIANT_FROG = 16,
        MANTRAP = 17,
        PIRANHA = 18,
        OLD_BITEY = 19,
        KILLER_BEE = 20,
        QUEEN_BEE = 21,
        SNAIL = 22,
        MONKEY = 23,
        GOLDEN_MONKEY = 24,
        JIANG_SHI = 25,
        GREEN_KNIGHT = 26,
        BLACK_KNIGHT = 27,
        VAMPIRE = 28,
        THE_GHOST = 29,
        BACTERIUM = 30,
        WORM_EGG = 31,
        WORM_BABY = 32,
        YETI = 33,
        YETI_KING = 34,
        MAMMOTH = 35,
        ALIEN = 36,
        UFO = 37,
        ALIEN_TANK = 38,
        ALIEN_LORD = 39,
        ALIEN_QUEEN = 40,
        HAWK_MAN = 41,
        CROC_MAN = 42,
        MAGMA_MAN = 43,
        SCORPION_FLY = 44,
        MUMMY = 45,
        ANUBIS = 46,
        ANUBIS_II = 47,
        OLMEC = 48,
        VLAD = 49,
        IMP = 50,
        DEVIL = 51,
        SUCCUBUS = 52,
        HORSE_HEAD = 53,
        OX_FACE = 54,
        KING_YAMA = 55
    }

    public enum ItemEntry : int
    {
        ROPE_PILE = 0,
        BOMB_BAG = 1,
        BOMB_BOX = 2,
        SPECTACLES = 3,
        CLIMBING_GLOVES = 4,
        PITCHERS_MITT = 5,
        SPRING_SHOES = 6,
        SPIKE_SHOES = 7,
        PASTE = 8,
        COMPASS = 9,
        MATTOCK = 10,
        BOOMERANG = 11,
        MACHETE = 12,
        CRYSKNIFE = 13,
        WEB_GUN = 14,
        SHOTGUN = 15,
        FREEZE_RAY = 16,
        PLASMA_CANNON = 17,
        CAMERA = 18,
        TELEPORTER = 19,
        PARACHUTE = 20,
        CAPE = 21,
        JETPACK = 22,
        SHIELD = 23,
        ROYAL_JELLY = 24,
        IDOL = 25,
        KAPALA = 26,
        UDJAT_EYE = 27,
        ANKH = 28,
        HEDJET = 29,
        SCEPTER = 30,
        BOOK_OF_THE_DEAD = 31,
        VLADS_CAPE = 32,
        VLADS_AMULET = 33
    }

    public enum TrapEntry : int
    {
        SPIKES = 0,
        ARROW_TRAP = 1,
        POWDER_BOX = 2,
        BOULDER = 3,
        TIKI_TRAP = 4,
        ACID = 5,
        SPRING = 6,
        MINE = 7,
        TURRET = 8,
        FORCEFIELD = 9,
        CRUSH_TRAP = 10,
        CEILING_TRAP = 11,
        SPIKE_BALL = 12,
        LAVA = 13
    }

    public class JournalState
    {
        public static readonly int NUM_PLACE_ENTRIES = Enum.GetNames(typeof(PlaceEntry)).Length;
        public static readonly int NUM_MONSTER_ENTRIES = Enum.GetNames(typeof(MonsterEntry)).Length;
        public static readonly int NUM_ITEM_ENTRIES = Enum.GetNames(typeof(ItemEntry)).Length;
        public static readonly int NUM_TRAP_ENTRIES = Enum.GetNames(typeof(TrapEntry)).Length;
        public static readonly int NUM_ENTRIES = NUM_PLACE_ENTRIES + NUM_MONSTER_ENTRIES + NUM_ITEM_ENTRIES + NUM_TRAP_ENTRIES;

        public bool[] PlaceEntries;
        public bool[] MonsterEntries;
        public bool[] ItemEntries;
        public bool[] TrapEntries;
    
        public JournalState(bool[] placeEntries, bool[] monsterEntries, bool[] itemEntries, bool[] trapEntries)
        {
            Debug.Assert(placeEntries.Length == NUM_PLACE_ENTRIES);
            Debug.Assert(monsterEntries.Length == NUM_MONSTER_ENTRIES);
            Debug.Assert(itemEntries.Length == NUM_ITEM_ENTRIES);
            Debug.Assert(trapEntries.Length == NUM_TRAP_ENTRIES);
            PlaceEntries = placeEntries;
            MonsterEntries = monsterEntries;
            ItemEntries = itemEntries;
            TrapEntries = trapEntries;
        }

        private static int CountUnlockedEntries(bool[] entries)
        {
            return entries.Where(b => b).Count();
        }

        public int NumUnlockedPlaceEntries => CountUnlockedEntries(PlaceEntries);
        public int NumUnlockedMonsterEntries => CountUnlockedEntries(MonsterEntries);
        public int NumUnlockedItemEntries => CountUnlockedEntries(ItemEntries);
        public int NumUnlockedTrapEntries => CountUnlockedEntries(TrapEntries);
        public int NumUnlockedEntries => NumUnlockedPlaceEntries + NumUnlockedMonsterEntries + NumUnlockedItemEntries + NumUnlockedTrapEntries;
    }

    public class SpelunkyHooks : IDisposable
    {
        public RawProcess Process { get; private set; }

        public SpelunkyHooks(RawProcess process)
        {
            Process = process;
        }

        public string DeviatedGameSavePath { get; set; }
        public string GameDirectoryPath => Path.GetDirectoryName(Process.FilePath);
        public string GameSavePath => DeviatedGameSavePath ?? GameDirectoryPath + @"\Data\spelunky_save.sav";

        private int Game => Process.ReadInt32(Process.BaseAddress + 0x1384b4);
        private int Gfx => Process.ReadInt32(Game + 0x4c);
        public int CharSelectCountdown => Process.ReadInt32(Gfx + 0x122bec);
        public SpelunkyState CurrentState => (SpelunkyState)Process.ReadInt32(Game + 0x58);
        public SpelunkyLevel CurrentLevel => (SpelunkyLevel)Process.ReadInt32(Game + 0x4405d4);
        public TunnelManChapter TunnelManChapter => (TunnelManChapter)Process.ReadInt32(Game + 0x445be4);
        public LobbyType CurrentLobbyType => (LobbyType)Process.ReadInt32(Game + 0x445be0);
        public bool Invalidated => Process.HasExited;

        /*
            3 => 1/2/3 Rope(s)
            2 => 1/2/3 Bomb(s)
            1 => $10000/Shotgun/Key
            0 => Nothing
        */
        public int TunnelManRemaining => Process.ReadInt32(Game + 0x445be8);

        private int JournalUnlocksTable => Game + 0x445bec;
        private int JournalPlaceUnlocksTable => JournalUnlocksTable + 0x200;
        private int JournalMonsterUnlocksTable => JournalUnlocksTable + 0x300;
        private int JournalItemUnlocksTable => JournalUnlocksTable + 0x400;
        private int JournalTrapUnlocksTable => JournalUnlocksTable + 0x500;

        private bool[] ReadJournalEntries(int processOffset, int size)
        {
            var entries = new bool[size];
            foreach(var entryIndex in Enumerable.Range(0, size))
            {
                var entryValue = Process.ReadInt32(processOffset + entryIndex*4);
                if(entryValue == 0)
                    entries[entryIndex] = false;
                else if(entryValue == 1)
                    entries[entryIndex] = true;
                else
                    throw new Exception($"Unexpected journal entry value: {entryValue}");
            }
            return entries;
        }

        public JournalState JournalState =>
            new JournalState(
                ReadJournalEntries(JournalPlaceUnlocksTable, JournalState.NUM_PLACE_ENTRIES),
                ReadJournalEntries(JournalMonsterUnlocksTable, JournalState.NUM_MONSTER_ENTRIES),
                ReadJournalEntries(JournalItemUnlocksTable, JournalState.NUM_ITEM_ENTRIES),
                ReadJournalEntries(JournalTrapUnlocksTable, JournalState.NUM_TRAP_ENTRIES));
        
        public void Dispose()
        {
            Process.Dispose();
        }
    }
}
