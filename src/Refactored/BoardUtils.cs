namespace Biasfish.V2
{
    public partial struct Board
    {
        private unsafe fixed ulong Bitboards[8];
        private unsafe fixed byte MailBox[64];
        private int sideToMove;
        private int currEpSquare;
        private int lastEpSquare;
        private int castlingRights;
        private int halfMoveClock;
        private int fullMoveClock;
        
        public int EncodingAt(int square)
        {
            unsafe
            {
                return MailBox[square];
            }
        }

        public int PieceAt(int square)
        {
            unsafe
            {
                return Piece.Type(MailBox[square]);
            }
        }

        private void AddPiece(int piece, int color, int square)
        {
            unsafe
            {
                Bitboards[piece] |= 1UL << square;
                Bitboards[color] |= 1UL << square;
                MailBox[square] = Piece.Encode(piece, color);
            }
        }

        private void RemovePiece(int square)
        {
            unsafe
            {
                int piece = PieceAt(square);

                Bitboards[piece] &= ~(1UL << square);
                Bitboards[Piece.White] &= ~(1UL << square);
                Bitboards[Piece.Black] &= ~(1UL << square);
                MailBox[square] = Piece.Null;
            }
        }
    }
}