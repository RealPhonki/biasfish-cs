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
        public const int White = 0;
        public const int WhitePawns = 1;
        public const int WhiteKnights = 2;
        public const int WhiteBishops = 3;
        public const int WhiteRooks = 4;
        public const int WhiteQueens = 5;
        public const int WhiteKings = 6;
        public const int Black = 8;
        public const int BlackPawns = 9;
        public const int BlackKnights = 10;
        public const int BlackBishops = 11;
        public const int BlackRooks = 12;
        public const int BlackQueens = 13;
        public const int BlackKings = 14;

        // Helper enumerations
        public const int Null = 0;

        public static int GetNeutral(int pieceType) => pieceType & 7;
        public static int GetColor(int pieceType) => pieceType & 8;
        public static int FlipColor(int pieceType) => pieceType ^ 8;
    }
}