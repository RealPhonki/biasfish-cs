namespace Biasfish.Core
{
    public static class Flags
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
        public const int EnPassant = 5;
        public const int Promotion = 8;
        public const int KnightPromote = 8;
        public const int BishopPromote = 9;
        public const int RookPromote = 10;
        public const int QueenPromote = 11;
        public const int KnightPromoteCapture = 12;
        public const int BishopPromoteCapture = 13;
        public const int RookPromoteCapture = 14;
        public const int QueenPromoteCapture = 15;

        public static bool IsQuiet(int flags) => flags == Quiet;
        public static bool IsCaptureOnly(int flags) => flags == Capture;
        public static bool IsCapture(int flags) => (flags & Capture) != 0;
        public static bool IsDoublePawnPush(int flags) => flags == DoublePawnPush;
        public static bool IsKingCastle(int flags) => flags == KingCastle;
        public static bool IsQueenCastle(int flags) => flags == QueenCastle;
        public static bool IsEnPassant(int flags) => flags == EnPassant;
        public static bool IsPromotion(int flags) => (flags & Promotion) != 0;
    }
}