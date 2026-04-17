namespace Biasfish.V2
{
    public static class Piece
    {
        public const int Null = 0;
        public const int White = 0;
        public const int Black = 1;
        public const int Pawn = 2;
        public const int Knight = 3;
        public const int Bishop = 4;
        public const int Rook = 5;
        public const int Queen = 6;
        public const int King = 7;
        public static readonly int[] PieceTypes = {White, Black, Pawn, Knight, Bishop, Rook, Queen, King};
    }
}