using Bitboard = ulong;

namespace Biasfish.Core
{
    public unsafe partial struct Position
    {
        private fixed Bitboard ByTypeBB[(int)PieceType.PieceTypeNB];
        private fixed Bitboard ByColorBB[(int)Color.ColorNB];
        private fixed byte board[(int)Square.SquareNB];
        private Color sideToMove;
        private int castlingRights;

        public Position()
        {
            Set("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        }

        public Position(string fenString)
        {
            Set(fenString);
        }
    }
}