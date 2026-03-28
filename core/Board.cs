namespace Biasfish.Core
{
    public struct Board
    {
        public ulong whitePawns;
        public ulong whiteKnights;
        public ulong whiteBishops;
        public ulong whiteRooks;
        public ulong whiteQueens;
        public ulong whiteKings;
        public ulong blackPawns;
        public ulong blackKnights;
        public ulong blackBishops;
        public ulong blackRooks;
        public ulong blackQueens;
        public ulong blackKings;
        public ulong occupiedWhite;
        public ulong occupiedBlack;
        public ulong occupiedAll;

        public int sideToMove;
        public int epSquare;
        public int castlingRights;
        public int halfMoveClock;
        public ulong key;

        public ulong GetBitboard(int pieceType)
        {
            switch (pieceType)
            {
                case Piece.WhitePawns:   return whitePawns;
                case Piece.WhiteKnights: return whiteKnights;
                case Piece.WhiteBishops: return whiteBishops;
                case Piece.WhiteRooks:   return whiteRooks;
                case Piece.WhiteQueens:  return whiteQueens;
                case Piece.WhiteKings:   return whiteKings;
                case Piece.BlackPawns:   return blackPawns;
                case Piece.BlackKnights: return blackKnights;
                case Piece.BlackRooks:   return blackRooks;
                case Piece.BlackQueens:  return blackQueens;
                case Piece.BlackKings:   return blackKings;
                default: return 0;
            }
        }

        public void SetBitboard(int pieceType, ulong value)
        {
            switch (pieceType)
            {
                case Piece.WhitePawns:   whitePawns   = value; return;
                case Piece.WhiteKnights: whiteKnights = value; return;
                case Piece.WhiteBishops: whiteBishops = value; return;
                case Piece.WhiteRooks:   whiteRooks   = value; return;
                case Piece.WhiteQueens:  whiteQueens  = value; return;
                case Piece.WhiteKings:   whiteKings   = value; return;
                case Piece.BlackPawns:   blackPawns   = value; return;
                case Piece.BlackKnights: blackKnights = value; return;
                case Piece.BlackRooks:   blackRooks   = value; return;
                case Piece.BlackQueens:  blackQueens  = value; return;
                case Piece.BlackKings:   blackKings   = value; return;
            }
        }
    }
}