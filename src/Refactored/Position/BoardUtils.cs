namespace Biasfish.V2
{
    /// <summary>
    /// This class exists as an interface that (generally) prevents illegal 
    /// modifications to the board state.
    /// </summary>
    public partial struct Board
    {
        private byte[] board; // 64-element mailbox
        private ulong[] byTypeBB; // 7-element bitboard array, 1st element empty
        private ulong[] byColorBB; // 2-element bitboard array
        private int[] pieceCount; // 6-element, tracks the total number of each piece
        private Color sideToMove;
        private int gamePly;
        /*
        ! zobrist, castlingRights, epSquare, rule50, capturePiece, and checkersBitboard
        ! will be implemented in StateInfo nodes externally. Each node will have a pointer
        ! to the previous node.
        */

        public void AddPiece(Piece piece, Color color, Square square)
        {
            // Update the bitboards
            byColorBB[(int)color] |= Bitboard.Square(square);
            byTypeBB[(int)piece]  |= Bitboard.Square(square);

            // Update the mailbox
            board[(int)square] = (byte)((int)piece | ((int)color << 3));
        }

        public void RemovePiece(Square square)
        {
            // Parse the mailbox
            byte pieceEncoding = board[(int)square];
            int piece = pieceEncoding & 7;
            int color = pieceEncoding >> 3;

            // Ignore empty squares
            if (piece == (int)Piece.NoPieceType) return;

            // Update the bitboards
            byColorBB[color] &= Bitboard.Mask(square);
            byTypeBB[piece]  &= Bitboard.Mask(square);

            // Clear the mailbox
            board[(int)square] = (byte)Piece.NoPieceType;
        }

        public void MovePiece(Square fromSquare, Square toSquare)
        {
            byte pieceEncoding = PieceAt(fromSquare);
            int piece = pieceEncoding & 7;
            int color = pieceEncoding >> 3;

            byTypeBB[color] ^= Bitboard.Square(fromSquare) | Bitboard.Square(toSquare);
            byTypeBB[piece] ^= Bitboard.Square(fromSquare) | Bitboard.Square(toSquare);

            board[(int)fromSquare] = (byte)Piece.NoPieceType;
            board[(int)toSquare]   = (byte)(piece | color << 3);;
        }

        /// <summary>
        /// ! Returns the piece encoding at the given square
        /// </summary>
        /// <param name="square"></param>
        /// <returns></returns>
        public byte PieceAt(Square square) => board[(int)square];

        public bool IsEmpty(Square square) => board[(int)square] == (byte)Piece.NoPieceType;

        public ulong Pieces(Piece piece, Color color) => byColorBB[(int)piece] | byTypeBB[(int)color];

        public ulong Pieces(Color color) => byColorBB[(int)color];

        public ulong Pieces(Piece piece) => byTypeBB[(int)piece];

        public Square EpSquare() => throw new NotImplementedException();

        public Color SideToMove() => sideToMove;

        public int CastlingRights() => throw new NotImplementedException();
    }
}