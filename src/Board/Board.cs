namespace Biasfish.Core
{
    /// <summary>
    /// Represents a board state and metadata with 120 bytes.
    /// * Note: This struct should be passed into methods using the `ref` keyword.
    /// </summary>
    public struct Board
    {
        // The board state is represented by 12 unsigned 64-bit integers where
        // each active bit represents whether or not a piece is located there.
        // This allows for fast mutations through bitwise operations.
        // * ref: https://www.chessprogramming.org/Bitboards
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

        // Pieces are represented with 4-bits where the first three bits represent the
        // piece type and the fourth bit represents the color. Because of this, sideToMove
        // is represented as either 0 (0000) or 8 (1000).
        public int sideToMove;

        // TODO: epSquare will used to add en-passant during legal move generation.
        public int epSquare;

        // TODO: castlingRights will be used to add castling during legal move generation.
        public int castlingRights;

        // TODO: halfMoveClock will be used to track the 50 move rule.
        public int halfMoveClock;

        // TODO: key will be used for zobrist hashing
        // ? This variable might need to be named `zobristKey`
        public ulong key;

        /// <summary>
        /// This method returns piece enumerations given a character from a fen string.
        /// </summary>
        /// <param name="symbol">Represents the piece type with a fen character.</param>
        /// <returns>Represents the piece type using the enumerations in `Piece.cs`.</returns>
        /// <exception cref="ArgumentException">Thrown when an invalid fen character is passed.</exception>
        private static int SymbolToPieceType(char symbol)
        {
            return symbol switch
            {
                'P' => Piece.WhitePawns,
                'N' => Piece.WhiteKnights,
                'B' => Piece.WhiteBishops,
                'R' => Piece.WhiteRooks,
                'Q' => Piece.WhiteQueens,
                'K' => Piece.WhiteKings,
                'p' => Piece.BlackPawns,
                'n' => Piece.BlackKnights,
                'b' => Piece.BlackBishops,
                'r' => Piece.BlackRooks,
                'q' => Piece.BlackQueens,
                'k' => Piece.BlackKings,
                _ => throw new ArgumentException($"Invalid FEN character: {symbol}")
            };
        }

        /// <summary>
        /// This method changes the sideToMove for the board struct by flipping the
        /// fourth bit.
        /// </summary>
        private void IncrementTurn()
        {
            sideToMove ^= 1 << 3;
        }

        /// <summary>
        /// This method reassigns all bitboards and metadata based on the contents of the
        /// given fen string.
        /// ref: https://www.chessprogramming.org/Forsyth-Edwards_Notation
        /// </summary>
        /// <param name="fenString">Represents the board state and metadata with fen notation.</param>
        public void LoadFEN(string fenString)
        {
            // clear all bitboards
            occupiedWhite = 0;
            occupiedBlack = 0;
            occupiedAll = 0;
            foreach (int pieceType in Piece.PieceTypes)
            {
                SetBitboard(pieceType, 0);
            }

            // parse parts
            string[] fenParts = fenString.Split(' ');
            string fenBoard = fenParts[0];
            sideToMove = fenParts[1] == "w" ? Piece.White : Piece.Black;

            // parse fenBoard starting at the top left moving rightwards
            int rank = 7;
            int file = 0;
            foreach (char symbol in fenBoard) 
            {
                // skip to the next rank (row)
                if (symbol == '/')
                {
                    rank--;
                    file = 0;
                }

                // convert the symbol into an integer and move rightwards by that amount
                else if (char.IsDigit(symbol))
                {
                    file += symbol - '0';
                }

                // convert the symbol into a piece type
                else
                {
                    int pieceType = SymbolToPieceType(symbol);
                    int pieceColor = Piece.GetColor(pieceType);
                    int square = rank * 8 + file;

                    // assign the piece bitboard and occupancy bitboards
                    SetBitboard(pieceType, GetBitboard(pieceType) | (1UL << square));
                    SetBitboard(pieceColor, GetBitboard(pieceColor) | (1UL << square));
                    occupiedAll |= 1UL << square;

                    file++;
                }
            }
        }

        /// <summary>
        /// Plays a given move.
        /// </summary>
        /// <param name="move">Represents the move</param>
        public void Push(Move move)
        {
            IncrementTurn();

            switch (move.Flags)
            {
                case 0:
                    // get piece metadata
                    int pieceType = PieceAt(move.FromSquare);
                    int pieceColor = Piece.GetColor(pieceType);

                    // assign the piece bitboard and occupancy bitboards
                    SetBitboard(pieceType, GetBitboard(pieceType) ^ (1UL << move.FromSquare | 1UL << move.ToSquare));
                    SetBitboard(pieceColor, GetBitboard(pieceColor) ^ (1UL << move.FromSquare | 1UL << move.ToSquare));
                    occupiedAll ^= 1UL << move.FromSquare | 1UL << move.ToSquare;

                    return;
                // TODO: Implement other flag cases
            }
        }

        /// <summary>
        /// Returns the integer representing the piece type at a given bit index. 
        /// Returns 0 if no piece is found.
        /// </summary>
        /// <param name="square">Represents the square to index with values from 0-63</param>
        /// <returns>Represents the piece type using the enumerations in `Piece.cs`</returns>
        /// <exception cref="InvalidOperationException">Thrown when there is a mismatch between the occupancy bitboard and piece bitboard.</exception>
        public int PieceAt(int square)
        {
            // if the occupancy bitboard has 0 at the index then return none
            if ((occupiedAll & (1UL << square)) == 0)
            {
                return Piece.None;
            }

            // check each bitboard for the piece
            foreach (int pieceType in Piece.PieceTypes)
            {
                if ((GetBitboard(pieceType) & (1UL << square)) != 0)
                {
                    return pieceType;
                }
            }

            // there is a mismatch between the occupancy bitboard and the piece bitboards
            throw new InvalidOperationException($"Invalid PieceAt({square}): Occupancy bitboards are not synced.");
        }

        /// <summary>
        /// Returns a piece bitboard given an integer representing piece type.
        /// </summary>
        /// <param name="pieceType">Represents the piece type using the enumerations in `Piece.cs`.</param>
        /// <returns>Returns the piece bitboard for the given piece type.</returns>
        public ulong GetBitboard(int pieceType)
        {
            switch (pieceType)
            {
                case Piece.Any:          return occupiedAll;
                case Piece.White:        return occupiedWhite;
                case Piece.WhitePawns:   return whitePawns;
                case Piece.WhiteKnights: return whiteKnights;
                case Piece.WhiteBishops: return whiteBishops;
                case Piece.WhiteRooks:   return whiteRooks;
                case Piece.WhiteQueens:  return whiteQueens;
                case Piece.WhiteKings:   return whiteKings;
                case Piece.Black:        return occupiedBlack;
                case Piece.BlackPawns:   return blackPawns;
                case Piece.BlackKnights: return blackKnights;
                case Piece.BlackBishops: return blackBishops;
                case Piece.BlackRooks:   return blackRooks;
                case Piece.BlackQueens:  return blackQueens;
                case Piece.BlackKings:   return blackKings;
                default: return 0;
            }
        }

        /// <summary>
        /// Assigns a given value to a piece bitboard given an integer representing the piece type.
        /// </summary>
        /// <param name="pieceType">Represents the piece type using the enumerations in `Piece.cs`.</param>
        /// <param name="value">Represents the new value for the bitboard.</param>
        public void SetBitboard(int pieceType, ulong value)
        {
            switch (pieceType)
            {
                case Piece.Any:          occupiedAll   = value; return;
                case Piece.White:        occupiedWhite = value; return;
                case Piece.WhitePawns:   whitePawns    = value; return;
                case Piece.WhiteKnights: whiteKnights  = value; return;
                case Piece.WhiteBishops: whiteBishops  = value; return;
                case Piece.WhiteRooks:   whiteRooks    = value; return;
                case Piece.WhiteQueens:  whiteQueens   = value; return;
                case Piece.WhiteKings:   whiteKings    = value; return;
                case Piece.Black:        occupiedBlack = value; return;
                case Piece.BlackPawns:   blackPawns    = value; return;
                case Piece.BlackKnights: blackKnights  = value; return;
                case Piece.BlackBishops: blackBishops  = value; return;
                case Piece.BlackRooks:   blackRooks    = value; return;
                case Piece.BlackQueens:  blackQueens   = value; return;
                case Piece.BlackKings:   blackKings    = value; return;
            }
        }
    }
}