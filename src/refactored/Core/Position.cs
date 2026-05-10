using Bitboard = ulong;

namespace Biasfish.Core
{
    public unsafe struct Position
    {
        private fixed Bitboard ByTypeBB[(int)PieceType.PieceTypeNB];
        private fixed Bitboard ByColorBB[(int)Color.ColorNB];
        private fixed byte board[(int)Square.SquareNB];
    }
}