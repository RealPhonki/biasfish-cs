namespace Biasfish.Core
{
    public static class Piece
    {
        // Contains an array of the enumerations for pieces only
        public static readonly int[] PieceTypes = 
        {
            1, 2, 3, 4, 5, 6, 9, 10, 11, 12, 13, 14
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
        public const int White = 0;
        public const int Black = 8;

        /// <summary>
        /// Gets the color of the piece passed.
        /// </summary>
        /// <param name="pieceType">Represents the piece type using the enumerations above.</param>
        /// <returns>Represents the piece color using the enumerations above.</returns>
        public static int GetColor(int pieceType)
        {
            return pieceType >> 3 == 0 ? White : Black;
        }
    }

    public static class Offset
    {
        public const int File = 1;
        public const int Rank = 8;
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
            Rank1, Rank2, Rank3, Rank4, Rank5, Rank6, Rank7, Rank8
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
            NotRank1, NotRank2, NotRank3, NotRank4, NotRank5, NotRank6, NotRank7, NotRank8
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
            FileA, FileB, FileC, FileD, FileE, FileF, FileG, FileH,
        };
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
            NotFileA, NotFileB, NotFileC, NotFileD, NotFileE, NotFileF, NotFileG, NotFileH
        };
    }
}