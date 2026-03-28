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
                case Piece.WhitePawns:   whitePawns   = value; break;
                case Piece.WhiteKnights: whiteKnights = value; break;
                case Piece.WhiteBishops: whiteBishops = value; break;
                case Piece.WhiteRooks:   whiteRooks   = value; break;
                case Piece.WhiteQueens:  whiteQueens  = value; break;
                case Piece.BlackPawns:   blackPawns   = value; break;
                case Piece.BlackKnights: blackKnights = value; break;
                case Piece.BlackRooks:   blackRooks   = value; break;
                case Piece.BlackQueens:  blackQueens  = value; break;
                case Piece.BlackKings:   blackKings   = value; break;
            }
        }
    }
}