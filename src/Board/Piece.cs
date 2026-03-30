namespace Biasfish.Core
{
    public static class Piece
    {
        // Contains an array of the enumerations for pieces only
        public static readonly int[] PieceTypes = {1, 2, 3, 4, 5, 6, 9, 10, 11, 12, 13, 14};
        
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
}