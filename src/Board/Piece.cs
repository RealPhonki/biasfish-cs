namespace Biasfish.Core
{
    public static class Piece
    {
        public static readonly int[] PieceTypes = {1, 2, 3, 4, 5, 6, 9, 10, 11, 12, 13, 14};
        
        // all enums can be obtained with Piece | Color
        public const int Pawns = 1;
        public const int Knights = 2;
        public const int Bishops = 3;
        public const int Rooks = 4;
        public const int Queens = 5;
        public const int Kings = 6;

        // explicit enums
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
        public const int None = 0;
        public const int White = 0;
        public const int Black = 8;

        public static int GetColor(int pieceType)
        {
            return pieceType >> 3 == 0 ? White : Black;
        }
    }
}