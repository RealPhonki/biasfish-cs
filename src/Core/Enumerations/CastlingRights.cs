namespace Biasfish.Core
{
    public static class CastlingRights
    {
        public const int WhiteKingSide      = 0b0001;
        public const int WhiteQueenSide     = 0b0010;
        public const int BlackKingSide      = 0b0100;
        public const int BlackQueenSide     = 0b1000;
        public const int WhiteKingSideMask  = 0b1110;
        public const int WhiteQueenSideMask = 0b1101;
        public const int BlackKingSideMask  = 0b1011;
        public const int BlackQueenSideMask = 0b0111;
        public const int WhiteMask          = 0b1100;
        public const int BlackMask          = 0b1100;
    }
}