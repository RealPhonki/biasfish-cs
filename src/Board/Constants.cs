using System.Reflection.Metadata;

namespace Biasfish.Core
{
    public static class Piece
    {
        // Contains an array of the enumerations for pieces only
        public static readonly byte[] PieceTypes = 
        {
            WhitePawns,
            WhiteKnights,
            WhiteBishops,
            WhiteRooks,
            WhiteQueens,
            WhiteKings,
            BlackPawns,
            BlackKnights,
            BlackBishops,
            BlackRooks,
            BlackQueens,
            BlackKings,
        };
        
        // Represents pieces with 3-bit values. These can be ORed with color enumerations, for example:
        // Piece.Pawns | Piece.Black = Piece.BlackPawns
        // Piece.Kings | Piece.White = Piece.WhiteKings
        public const int Pawns = 1;
        public const int Knights = 2;
        public const int Bishops = 3;
        public const int Rooks = 4;
        public const int Queens = 5;
        public const int Kings = 6;

        // Represents pieces with 4-bit values. The first three bits represent the piece type,
        // while the fourth bit represent the color of the piece. For example:
        // 0001 = Piece.WhitePawns
        // 1001 = Piece.BlackPawns
        public const int WhitePawns = 1;
        public const int WhiteKnights = 2;
        public const int WhiteBishops = 3;
        public const int WhiteRooks = 4;
        public const int WhiteQueens = 5;
        public const int WhiteKings = 6;
        public const int BlackPawns = 9;
        public const int BlackKnights = 10;
        public const int BlackBishops = 11;
        public const int BlackRooks = 12;
        public const int BlackQueens = 13;
        public const int BlackKings = 14;

        // Helper enumerations
        public const int None = 0;
        public const int Any = 15;
        public const int White = 0;
        public const int Black = 8;

        /// <summary>
        /// Gets the color of the piece passed.
        /// </summary>
        /// <param name="pieceType">Represents the piece type using the enumerations above.</param>
        /// <returns>Represents the piece color using the enumerations above.</returns>
        public static int GetColor(int pieceType) => pieceType & 8;
        public static int FlipColor(int pieceType) => pieceType ^ 8;
    }

    public static class Flag
    {
        /// <summary>
        /// First bit represents promotion
        /// Second bit represents capture
        /// Reference: https://www.chessprogramming.org/Encoding_Moves
        /// </summary>
        public const int Quiet = 0;
        public const int DoublePawnPush = 1;
        public const int KingCastle = 2;
        public const int QueenCastle = 3;
        public const int Capture = 4;
        public const int EpCapture = 5;
        public const int KnightPromote = 8;
        public const int BishopPromote = 9;
        public const int RookPromote = 10;
        public const int QueenPromote = 11;
        public const int KnightPromoteCapture = 12;
        public const int BishopPromoteCapture = 13;
        public const int RookPromoteCapture = 14;
        public const int QueenPromoteCapture = 15;

        public static bool IsCapture(int flag) => (flag & Capture) != 0;
    }

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
    }
    
    public static class Squares
    {
        public const int A1 = 0;
        public const int B1 = 1;
        public const int C1 = 2;
        public const int D1 = 3;
        public const int E1 = 4;
        public const int F1 = 5;
        public const int G1 = 6;
        public const int H1 = 7;
        public const int A2 = 8;
        public const int B2 = 9;
        public const int C2 = 10;
        public const int D2 = 11;
        public const int E2 = 12;
        public const int F2 = 13;
        public const int G2 = 14;
        public const int H2 = 15;
        public const int A3 = 16;
        public const int B3 = 17;
        public const int C3 = 18;
        public const int D3 = 19;
        public const int E3 = 20;
        public const int F3 = 21;
        public const int G3 = 22;
        public const int H3 = 23;
        public const int A4 = 24;
        public const int B4 = 25;
        public const int C4 = 26;
        public const int D4 = 27;
        public const int E4 = 28;
        public const int F4 = 29;
        public const int G4 = 30;
        public const int H4 = 31;
        public const int A5 = 32;
        public const int B5 = 33;
        public const int C5 = 34;
        public const int D5 = 35;
        public const int E5 = 36;
        public const int F5 = 37;
        public const int G5 = 38;
        public const int H5 = 39;
        public const int A6 = 40;
        public const int B6 = 41;
        public const int C6 = 42;
        public const int D6 = 43;
        public const int E6 = 44;
        public const int F6 = 45;
        public const int G6 = 46;
        public const int H6 = 47;
        public const int A7 = 48;
        public const int B7 = 49;
        public const int C7 = 50;
        public const int D7 = 51;
        public const int E7 = 52;
        public const int F7 = 53;
        public const int G7 = 54;
        public const int H7 = 55;
        public const int A8 = 56;
        public const int B8 = 57;
        public const int C8 = 58;
        public const int D8 = 59;
        public const int E8 = 60;
        public const int F8 = 61;
        public const int G8 = 62;
        public const int H8 = 63;
    }
}