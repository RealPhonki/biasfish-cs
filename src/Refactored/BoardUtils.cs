namespace Biasfish.V2
{
    public partial struct Board
    {
        private unsafe fixed ulong Bitboards[8];
        private unsafe fixed byte MailBox[64];
        private int sideToMove;
        // TODO: Implement ep square
        private int currEpSquare;
        private int lastEpSquare;
        // TODO: Implement castling rights
        private int castlingRights;
        // TODO: Implement half move clock
        private int halfMoveClock;
        // TODO: Implement full move clock
        private int fullMoveClock;
        // TODO: Implement zobrist keys
        private int zobristKey;
        
        public int EncodingAt(int square)
        {
            unsafe
            {
                return MailBox[square];
            }
        }

        private int PieceAt(int square)
        {
            unsafe
            {
                return Piece.Type(MailBox[square]);
            }
        }

        private int ColorAt(int square)
        {
            unsafe
            {
                return Piece.Color(MailBox[square]);
            }
        }

        private void AddPiece(int piece, int square, int color)
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

        private void RemovePiece(int piece, int square)
        {
            unsafe
            {
                Bitboards[piece] &= ~(1UL << square);
                Bitboards[Piece.White] &= ~(1UL << square);
                Bitboards[Piece.Black] &= ~(1UL << square);
                MailBox[square] = Piece.Null;
            }
        }

        private void RemovePiece(int piece, int square, int color)
        {
            unsafe
            {
                Bitboards[piece] &= ~(1UL << square);
                Bitboards[color] &= ~(1UL << square);
                MailBox[square] = Piece.Null;
            }
        }
    }
}