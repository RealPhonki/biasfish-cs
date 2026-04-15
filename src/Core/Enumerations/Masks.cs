namespace Biasfish.Core
{
    public static class Masks
    {
        public const ulong Rank1 = 0xFF;
        public const ulong Rank2 = Rank1 << 8;
        public const ulong Rank3 = Rank1 << 16;
        public const ulong Rank4 = Rank1 << 24;
        public const ulong Rank5 = Rank1 << 32;
        public const ulong Rank6 = Rank1 << 40;
        public const ulong Rank7 = Rank1 << 48;
        public const ulong Rank8 = Rank1 << 56;
        public static readonly ulong[] Rank =
        {
            Rank1,
            Rank2,
            Rank3,
            Rank4,
            Rank5,
            Rank6,
            Rank7,
            Rank8,
        };
        public const ulong NotRank1 = ~Rank1;
        public const ulong NotRank2 = ~Rank2;
        public const ulong NotRank3 = ~Rank3;
        public const ulong NotRank4 = ~Rank4;
        public const ulong NotRank5 = ~Rank5;
        public const ulong NotRank6 = ~Rank6;
        public const ulong NotRank7 = ~Rank7;
        public const ulong NotRank8 = ~Rank8;
        public static readonly ulong[] NotRank =
        {
            NotRank1,
            NotRank2,
            NotRank3,
            NotRank4,
            NotRank5,
            NotRank6,
            NotRank7,
            NotRank8,
        };
        public const ulong AboveRank1 = Rank8 | Rank7 | Rank6 | Rank5 | Rank4 | Rank3 | Rank2;
        public const ulong AboveRank2 = Rank8 | Rank7 | Rank6 | Rank5 | Rank4 | Rank3;
        public const ulong AboveRank3 = Rank8 | Rank7 | Rank6 | Rank5 | Rank4;
        public const ulong AboveRank4 = Rank8 | Rank7 | Rank6 | Rank5;
        public const ulong AboveRank5 = Rank8 | Rank7 | Rank6;
        public const ulong AboveRank6 = Rank8 | Rank7;
        public const ulong AboveRank7 = Rank8;
        public const ulong AboveRank8 = 0;
        public static readonly ulong[] AboveRank =
        {
            AboveRank1,
            AboveRank2,
            AboveRank3,
            AboveRank4,
            AboveRank5,
            AboveRank6,
            AboveRank7,
            AboveRank8,
        };
        public const ulong BelowRank8 = Rank1 | Rank2 | Rank3 | Rank4 | Rank5 | Rank6 | Rank7; 
        public const ulong BelowRank7 = Rank1 | Rank2 | Rank3 | Rank4 | Rank5 | Rank6;
        public const ulong BelowRank6 = Rank1 | Rank2 | Rank3 | Rank4 | Rank5;
        public const ulong BelowRank5 = Rank1 | Rank2 | Rank3 | Rank4;
        public const ulong BelowRank4 = Rank1 | Rank2 | Rank3;
        public const ulong BelowRank3 = Rank1 | Rank2;
        public const ulong BelowRank2 = Rank1;
        public const ulong BelowRank1 = 0;
        public static readonly ulong[] BelowRank =
        {
            BelowRank1,
            BelowRank2,
            BelowRank3,
            BelowRank4,
            BelowRank5,
            BelowRank6,
            BelowRank7,
            BelowRank8,
        };
        public const ulong FileA = 0x0101010101010101;
        public const ulong FileB = FileA << 1;
        public const ulong FileC = FileA << 2;
        public const ulong FileD = FileA << 3;
        public const ulong FileE = FileA << 4;
        public const ulong FileF = FileA << 5;
        public const ulong FileG = FileA << 6;
        public const ulong FileH = FileA << 7;
        public static readonly ulong[] File =
        {
            FileA,
            FileB,
            FileC,
            FileD,
            FileE,
            FileF,
            FileG,
            FileH
        };
        public const ulong FileAB = FileA | FileB;
        public const ulong FileGH = FileG | FileH;
        public const ulong NotFileA = ~FileA;
        public const ulong NotFileB = ~FileB;
        public const ulong NotFileC = ~FileC;
        public const ulong NotFileD = ~FileD;
        public const ulong NotFileE = ~FileE;
        public const ulong NotFileF = ~FileF;
        public const ulong NotFileG = ~FileG;
        public const ulong NotFileH = ~FileH;
        public static readonly ulong[] NotFile =
        {
            NotFileA,
            NotFileB,
            NotFileC,
            NotFileD,
            NotFileE,
            NotFileF,
            NotFileG,
            NotFileH,
        };
        public const ulong NotFileAB = ~FileAB;
        public const ulong NotFileGH = ~FileGH;
        public const ulong RightFileA = FileH | FileG | FileF | FileE | FileD | FileC | FileB;
        public const ulong RightFileB = FileH | FileG | FileF | FileE | FileD | FileC;
        public const ulong RightFileC = FileH | FileG | FileF | FileE | FileD;
        public const ulong RightFileD = FileH | FileG | FileF | FileE;
        public const ulong RightFileE = FileH | FileG | FileF;
        public const ulong RightFileF = FileH | FileG;
        public const ulong RightFileG = FileH;
        public const ulong RightFileH = 0;
        public static readonly ulong[] RightFile =
        {
            RightFileA,
            RightFileB,
            RightFileC,
            RightFileD,
            RightFileE,
            RightFileF,
            RightFileG,
            RightFileH
        };
        public const ulong LeftFileH = FileA | FileB | FileC | FileD | FileE | FileF | FileG;
        public const ulong LeftFileG = FileA | FileB | FileC | FileD | FileE | FileF;
        public const ulong LeftFileF = FileA | FileB | FileC | FileD | FileE;
        public const ulong LeftFileE = FileA | FileB | FileC | FileD;
        public const ulong LeftFileD = FileA | FileB | FileC;
        public const ulong LeftFileC = FileA | FileB;
        public const ulong LeftFileB = FileA;
        public const ulong LeftFileA = 0;
        public static readonly ulong[] LeftFile =
        {
            LeftFileA,
            LeftFileB,
            LeftFileC,
            LeftFileD,
            LeftFileE,
            LeftFileF,
            LeftFileG,
            LeftFileH
        };

        public const ulong A1 = 1UL << 0;
        public const ulong B1 = 1UL << 1;
        public const ulong C1 = 1UL << 2;
        public const ulong D1 = 1UL << 3;
        public const ulong E1 = 1UL << 4;
        public const ulong F1 = 1UL << 5;
        public const ulong G1 = 1UL << 6;
        public const ulong H1 = 1UL << 7;
        public const ulong A2 = 1UL << 8;
        public const ulong B2 = 1UL << 9;
        public const ulong C2 = 1UL << 10;
        public const ulong D2 = 1UL << 11;
        public const ulong E2 = 1UL << 12;
        public const ulong F2 = 1UL << 13;
        public const ulong G2 = 1UL << 14;
        public const ulong H2 = 1UL << 15;
        public const ulong A3 = 1UL << 16;
        public const ulong B3 = 1UL << 17;
        public const ulong C3 = 1UL << 18;
        public const ulong D3 = 1UL << 19;
        public const ulong E3 = 1UL << 20;
        public const ulong F3 = 1UL << 21;
        public const ulong G3 = 1UL << 22;
        public const ulong H3 = 1UL << 23;
        public const ulong A4 = 1UL << 24;
        public const ulong B4 = 1UL << 25;
        public const ulong C4 = 1UL << 26;
        public const ulong D4 = 1UL << 27;
        public const ulong E4 = 1UL << 28;
        public const ulong F4 = 1UL << 29;
        public const ulong G4 = 1UL << 30;
        public const ulong H4 = 1UL << 31;
        public const ulong A5 = 1UL << 32;
        public const ulong B5 = 1UL << 33;
        public const ulong C5 = 1UL << 34;
        public const ulong D5 = 1UL << 35;
        public const ulong E5 = 1UL << 36;
        public const ulong F5 = 1UL << 37;
        public const ulong G5 = 1UL << 38;
        public const ulong H5 = 1UL << 39;
        public const ulong A6 = 1UL << 40;
        public const ulong B6 = 1UL << 41;
        public const ulong C6 = 1UL << 42;
        public const ulong D6 = 1UL << 43;
        public const ulong E6 = 1UL << 44;
        public const ulong F6 = 1UL << 45;
        public const ulong G6 = 1UL << 46;
        public const ulong H6 = 1UL << 47;
        public const ulong A7 = 1UL << 48;
        public const ulong B7 = 1UL << 49;
        public const ulong C7 = 1UL << 50;
        public const ulong D7 = 1UL << 51;
        public const ulong E7 = 1UL << 52;
        public const ulong F7 = 1UL << 53;
        public const ulong G7 = 1UL << 54;
        public const ulong H7 = 1UL << 55;
        public const ulong A8 = 1UL << 56;
        public const ulong B8 = 1UL << 57;
        public const ulong C8 = 1UL << 58;
        public const ulong D8 = 1UL << 59;
        public const ulong E8 = 1UL << 60;
        public const ulong F8 = 1UL << 61;
        public const ulong G8 = 1UL << 62;
        public const ulong H8 = 1UL << 63;
        public static readonly ulong[] Square =
        {
            A1, B1, C1, D1, E1, F1, G1, H1,
            A2, B2, C2, D2, E2, F2, G2, H2,
            A3, B3, C3, D3, E3, F3, G3, H3,
            A4, B4, C4, D4, E4, F4, G4, H4,
            A5, B5, C5, D5, E5, F5, G5, H5,
            A6, B6, C6, D6, E6, F6, G6, H6,
            A7, B7, C7, D7, E7, F7, G7, H7,
            A8, B8, C8, D8, E8, F8, G8, H8,
        };
    }
}