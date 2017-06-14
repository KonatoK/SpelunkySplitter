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
        InputLockPart2 = 2,
        InputLockPart3 = 3,
        SplashScreen = 5,
        CharacterSelect = 17, 
        VictoryCutscene = 18,
        Lobby = 22
    }

    public enum SpelunkyLevel : int
    {
        Empty = 0,
        L0_1Or1_1 = 1,
        L0_2Or1_2 = 2,
        L0_3Or1_3 = 3,
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
        EmptyUnencountered = 0,
        MinesToJungle = 1,
        EmptyPostMtj = 2,
        JungleToIceCaves = 3,
        EmptyPostJtic = 4,
        IceCavesToTemple = 5,
        EmptyCompleted = 6
    }
    
    public enum LobbyType : int
    {
        Tutorial = 0,
        Breaking = 1,
        DoorOpenNoGem = 2
    }

    public enum PlaceEntry : int
    {
        TheMines = 0,
        TheJungle = 1,
        TheIceCaves = 2,
        TheTemple = 3,
        Hell = 4,
        TheHauntedCastle = 5,
        TheBlackMarket = 6,
        TheWorm = 7,
        TheMothership = 8,
        TheCityOfGold = 9
    }

    public enum MonsterEntry : int
    {
        Snake = 0,
        Cobra = 1,
        Bat = 2,
        Spider = 3,
        SpinnerSpider = 4,
        GiantSpider = 5,
        Skeleton = 6,
        Scorpion = 7,
        Caveman = 8,
        Damsel = 9,
        Shopkeeper = 10,
        TheTunnelMan = 11,
        Scarab = 12,
        TikiMan = 13,
        Frog = 14,
        FireFrog = 15,
        GiantFrog = 16,
        ManTrap = 17,
        Piranha = 18,
        OldBitey = 19,
        KillerBee = 20,
        QueenBee = 21,
        Snail = 22,
        Monkey = 23,
        GoldenMonkey = 24,
        JiangShi = 25,
        GreenKnight = 26,
        BlackKnight = 27,
        Vampire = 28,
        TheGhost = 29,
        Bacterium = 30,
        WormEgg = 31,
        WormBaby = 32,
        Yeti = 33,
        YetiKing = 34,
        Mammoth = 35,
        Alien = 36,
        Ufo = 37,
        AlienTank = 38,
        AlienLord = 39,
        AlienQueen = 40,
        HawkMan = 41,
        CrocMan = 42,
        MagmaMan = 43,
        ScorpionFly = 44,
        Mummy = 45,
        Anubis = 46,
        AnubisIi = 47,
        Olmec = 48,
        Vlad = 49,
        Imp = 50,
        Devil = 51,
        Succubus = 52,
        HorseHead = 53,
        OxFace = 54,
        KingYama = 55
    }

    public enum ItemEntry : int
    {
        RopePile = 0,
        BombBag = 1,
        BombBox = 2,
        Spectacles = 3,
        ClimbingGloves = 4,
        PitchersMitt = 5,
        SpringShoes = 6,
        SpikeShoes = 7,
        Paste = 8,
        Compass = 9,
        Mattock = 10,
        Boomerang = 11,
        Machete = 12,
        Crysknife = 13,
        WebGun = 14,
        Shotgun = 15,
        FreezeRay = 16,
        PlasmaCannon = 17,
        Camera = 18,
        Teleporter = 19,
        Parachute = 20,
        Cape = 21,
        Jetpack = 22,
        Shield = 23,
        RoyalJelly = 24,
        Idol = 25,
        Kapala = 26,
        UdjatEye = 27,
        Ankh = 28,
        Hedjet = 29,
        Scepter = 30,
        BookOfTheDead = 31,
        VladsCape = 32,
        VladsAmulet = 33
    }

    public enum TrapEntry : int
    {
        Spikes = 0,
        ArrowTrap = 1,
        PowderBox = 2,
        Boulder = 3,
        TikiTrap = 4,
        Acid = 5,
        Spring = 6,
        Mine = 7,
        Turret = 8,
        ForceField = 9,
        CrushTrap = 10,
        CeilingTrap = 11,
        SpikeBall = 12,
        Lava = 13
    }

    public enum SpelunkyCharacter : int
    {
        Yang = 0,
        MeatBoy = 1,
        Yellow = 2,
        JungleWarrior = 3,
        Purple = 4,
        VanHelsing = 5,
        Cyan = 6,
        Lime = 7,
        Eskimo = 8,
        RoundGirl = 9,
        Ninja = 10,
        Viking = 11,
        RoundBoy = 12,
        Cyclops = 13,
        Robot = 14,
        GoldenMonk= 15
    }

    public class CharactersState
    {
        public static readonly int NumCharacters = Enum.GetNames(typeof(SpelunkyCharacter)).Length;

        public bool[] UnlockedChars;

        public CharactersState(bool[] unlockedChars)
        {
            Debug.Assert(unlockedChars.Length == NumCharacters);
            UnlockedChars = unlockedChars;
        }

        private static int CountUnlockedEntries(bool[] entries)
        {
            return entries.Where(b => b).Count();
        }

        public int NumUnlockedCharacters => CountUnlockedEntries(UnlockedChars);
    }

    public class JournalState
    {
        public static readonly int NumPlaceEntries = Enum.GetNames(typeof(PlaceEntry)).Length;
        public static readonly int NumMonsterEntries = Enum.GetNames(typeof(MonsterEntry)).Length;
        public static readonly int NumItemEntries = Enum.GetNames(typeof(ItemEntry)).Length;
        public static readonly int NumTrapEntries = Enum.GetNames(typeof(TrapEntry)).Length;
        public static readonly int NumEntries = NumPlaceEntries + NumMonsterEntries + NumItemEntries + NumTrapEntries;

        public bool[] PlaceEntries;
        public bool[] MonsterEntries;
        public bool[] ItemEntries;
        public bool[] TrapEntries;
    
        public JournalState(bool[] placeEntries, bool[] monsterEntries, bool[] itemEntries, bool[] trapEntries)
        {
            Debug.Assert(placeEntries.Length == NumPlaceEntries);
            Debug.Assert(monsterEntries.Length == NumMonsterEntries);
            Debug.Assert(itemEntries.Length == NumItemEntries);
            Debug.Assert(trapEntries.Length == NumTrapEntries);
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

        public bool Equals(JournalState o)
        {
            return (new List<Tuple<bool[], bool[]>> {
                new Tuple<bool[], bool[]>(PlaceEntries, o.PlaceEntries),
                new Tuple<bool[], bool[]>(MonsterEntries, o.MonsterEntries),
                new Tuple<bool[], bool[]>(ItemEntries, o.ItemEntries),
                new Tuple<bool[], bool[]>(TrapEntries, o.TrapEntries)
            }).Aggregate(true, (totalResult, entryPair) =>
                totalResult && entryPair.Item1.Length == entryPair.Item2.Length
                    && Enumerable.Range(0, entryPair.Item1.Length).Aggregate(true,
                        (result, currentIndex) => result && entryPair.Item1[currentIndex] == entryPair.Item2[currentIndex]));
        }
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

        private int CharacterUnlocksTable => Game + 0x4463ec;

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
                ReadJournalEntries(JournalPlaceUnlocksTable, JournalState.NumPlaceEntries),
                ReadJournalEntries(JournalMonsterUnlocksTable, JournalState.NumMonsterEntries),
                ReadJournalEntries(JournalItemUnlocksTable, JournalState.NumItemEntries),
                ReadJournalEntries(JournalTrapUnlocksTable, JournalState.NumTrapEntries));

        /* This is pretty much the same code as ReadJournalEntries, having a generic method might be a better idea in the future */
        private bool[] ReadCharacters(int processOffset, int size)
        {
            var entries = new bool[size];
            foreach(var entryIndex in Enumerable.Range(0, size))
            {
                var entryValue = Process.ReadInt32(processOffset + entryIndex * 4);
                if(entryValue == 0)
                    entries[entryIndex] = false;
                else if(entryValue == 1)
                    entries[entryIndex] = true;
                else
                    throw new Exception($"Unexpected character unlock value: {entryValue}");
            }
            return entries;
        }

        public CharactersState CharactersState => new CharactersState(ReadCharacters(CharacterUnlocksTable, CharactersState.NumCharacters));

        public void Dispose()
        {
            Process.Dispose();
        }
    }
}
