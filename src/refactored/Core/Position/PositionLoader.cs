using Bitboard = ulong;

namespace Biasfish.Core
{
    public unsafe partial struct Position
    {
        /// <summary>
        /// Loads board state and metadata from the given fen string.
        /// </summary>
        /// <param name="fenString"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Set(string fenString)
        {
            // TODO: Use read-only span
            string[] fenParts = fenString.Split(' ');
            if (fenParts.Length < 4 || fenParts.Length > 6)
            {
                throw new ArgumentException($"Invalid FEN, {fenParts.Length} parts found: '{fenString}'");
            }

            Clear();

            SetBoard(fenParts[0]);
            SetSideToMove(fenParts[1]);
            SetCastlingRights(fenParts[2]);
            SetEpSquare(fenParts[3]);
            if (fenParts.Length > 4) SetHalfMove(fenParts[4]);
            if (fenParts.Length > 5) SetFullMove(fenParts[5]);
        }

        /// <summary>
        /// Mutates byTypeBB, byColorBB, and board with the given fen sub-string
        /// </summary>
        /// <param name="fen"></param>
        /// <exception cref="ArgumentException"></exception>
        private void SetBoard(string fen)
        {
            // start on A8 moving rightwards
            Rank rank = Rank.Rank8;
            File file = File.FileA;
            foreach (char symbol in fen)
            {
                // move down one rank
                if (symbol == '/')
                {
                    rank--;
                    file = File.FileA;
                }

                // convert the symbol to an int and move right
                else if (char.IsDigit(symbol))
                {
                    int skip = symbol - '0';
                    if (skip < 1 || skip > (int)File.FileNB || file + skip > File.FileNB)
                    {
                        throw new ArgumentException($"Invalid FEN, pointer hit file {(int)file + skip}, rank {(int)rank + 1}: '{fen}");
                    }
                    file += skip;
                }

                // convert the symbol into a piece type
                else
                {
                    if (file + 1 > File.FileNB)
                    {
                        throw new ArgumentException($"Invalid FEN, pointer hit file {(int)file + 1}, rank {(int)rank + 1}: '{fen}");
                    }

                    // TODO: check if FromChar is inlineable
                    // parse symbol
                    Piece piece = Piece.FromChar(symbol);
                    Square square = Square.MakeSquare(rank, file);

                    // add piece
                    PutPiece(piece, square);

                    // move to the next file
                    file++;
                }
            }

            Bitboard whiteKing = this.Pieces(Piece.WhiteKing);
            Bitboard blackKing = this.Pieces(Piece.BlackKing);

            if (BitboardUtils.MoreThanOne(whiteKing)) throw new ArgumentException($"Invalid FEN, multiple white kings found");
            if (whiteKing == 0UL) throw new ArgumentException($"Invalid FEN, missing white king");
            if (BitboardUtils.MoreThanOne(blackKing)) throw new ArgumentException($"Invalid FEN, multiple black kings found");
            if (blackKing == 0UL) throw new ArgumentException($"Invalid FEN, missing black king");
        }

        /// <summary>
        /// Mutates sideToMove with the given fen sub-string
        /// </summary>
        /// <param name="fen"></param>
        /// <exception cref="ArgumentException"></exception>
        private void SetSideToMove(string fen)
        {
            if (fen != "w" && fen != "b")
            {
                throw new ArgumentException($"Invalid FEN, illegal character for side to move: '{fen}'");
            }
            sideToMove = fen == "w" ? Color.White : Color.Black;
        }

        /// <summary>
        /// Mutates castlingRights with the given fen sub-string
        /// </summary>
        /// <param name="fen"></param>
        /// <exception cref="ArgumentException"></exception>
        private void SetCastlingRights(string fen)
        {
            castlingRights = CastlingRights.NoCastling;
            if (fen == "-") return;

            foreach (char character in fen)
            {
                if (!"KQkq".Contains(character))
                {
                    throw new ArgumentException($"Invalid FEN, illegal character '{character}' in castling rights: '{fen}'");
                }

                castlingRights |= character switch
                {
                    'K' => CastlingRights.WhiteOO,
                    'Q' => CastlingRights.WhiteOOO,
                    'k' => CastlingRights.BlackOO,
                    'q' => CastlingRights.BlackOOO,
                    _   => 0
                };
            }
        }

        /// <summary>
        /// Mutates EpSquare using the given fen sub-string
        /// </summary>
        /// <param name="fen"></param>
        /// <exception cref="ArgumentException"></exception>
        private void SetEpSquare(string fen)
        {
            if (fen == "-") return;
            
            if (fen.Length != 2)
            {
                throw new ArgumentException($"Invalid FEN, illegal format for ep square: '{fen}'");
            }
            if (!"abcdefgh".Contains(fen[0]))
            {
                throw new ArgumentException($"Invalid FEN, illegal character '{fen[0]}' for rank in ep square: '{fen}'");
            }
            if (!"12345678".Contains(fen[1]))
            {
                throw new ArgumentException($"Invalid FEN, illegal character '{fen[1]}' for file in ep square: '{fen}'");
            }

            EpSquare = Square.FromUci(fen);
        }

        private void SetHalfMove(string fen)
        {
            
        }

        private void SetFullMove(string fen)
        {
            
        }
    }
}