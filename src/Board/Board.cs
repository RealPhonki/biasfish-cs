namespace Biasfish.Core
{
    /// <summary>
    /// Represents a board state and metadata.
    /// * Note: This struct should be passed into methods using the `ref` keyword.
    /// </summary>
    public struct Board
    {
        // The board state is represented by 12 unsigned 64-bit integers where
        // each active bit represents whether or not a piece is located there.
        // This allows for fast mutations through bitwise operations.
        // * ref: https://www.chessprogramming.org/Bitboards
        public unsafe fixed ulong Bitboards[16];

        public unsafe fixed byte MailBox[64];

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
        /// This method reassigns all bitboards and metadata based on the contents of the
        /// given fen string.
        /// ref: https://www.chessprogramming.org/Forsyth-Edwards_Notation
        /// </summary>
        /// <param name="fenString">Represents the board state and metadata with fen notation.</param>
        public void LoadFEN(string fenString)
        {
            unsafe
            {
                // clear all bitboards
                for (int index = 0; index < 16; index++)
                {
                    Bitboards[index] = 0;
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
                        Bitboards[pieceType] |= 1UL << square;
                        Bitboards[pieceColor] |= 1UL << square;
                        Bitboards[Piece.Any] |= 1UL << square;

                        MailBox[square] = (byte)pieceType;
                        
                        file++;
                    }
                }
            }
        }

        /// <summary>
        /// Plays a given move.
        /// </summary>
        /// <param name="move">Represents the move</param>
        public void Push(Move move)
        {
            sideToMove = Piece.FlipColor(sideToMove);

            switch (move.Flags)
            {
                case 0:
                    // get piece metadata
                    int pieceType = PieceAt(move.FromSquare);
                    int pieceColor = Piece.GetColor(pieceType);

                    // assign the piece bitboard and occupancy bitboards
                    unsafe
                    {
                        Bitboards[pieceType] ^= 1UL << move.FromSquare | 1UL << move.ToSquare;
                        Bitboards[pieceColor] ^= 1UL << move.FromSquare | 1UL << move.ToSquare;
                        Bitboards[Piece.Any] ^= 1UL << move.FromSquare | 1UL << move.ToSquare;

                        MailBox[move.FromSquare] = Piece.None;
                        MailBox[move.ToSquare] = (byte)pieceType;
                    }
                    
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
        public int PieceAt(int square)
        {
            unsafe
            {
                return MailBox[square];
            }
        }

        public ulong Get(int pieceType)
        {
            unsafe
            {
                return Bitboards[pieceType];
            }
        }

        public void Set(int pieceType, ulong value)
        {
            unsafe
            {
                Bitboards[pieceType] = value;
            }
        }
    }
}