using Bitboard = ulong;

namespace Biasfish.Core
{
    public unsafe partial struct Position
    {
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
            SetEPSquare(fenParts[3]);
            if (fenParts.Length > 4) SetHalfMove(fenParts[4]);
            if (fenParts.Length > 5) SetFullMove(fenParts[5]);
        }

        private void SetBoard(string fen)
        {
            // start on A8 moving rightwards
            int rank = 7;
            int file = 0;
            foreach (char symbol in fen)
            {
                // move down one rank
                if (symbol == '/' && file <= 8)
                {
                    rank--;
                    file = 0;
                    continue;
                }

                if (file > 7 || rank < 0)
                {
                    throw new ArgumentException($"Invalid FEN, pointer hit file {file}, rank {rank + 1}: '{fen}");
                }

                // convert the symbol to an int and move right
                if (char.IsDigit(symbol))
                {
                    file += symbol - '0';
                }

                // convert the symbol into a piece type
                else
                {
                    // TODO: check if FromChar is inlineable
                    // parse symbol
                    Piece piece = Piece.FromChar(symbol);
                    Square square = (Square)(rank * 8 + file);

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

        private void SetSideToMove(string fen)
        {
            if (!"wb".Contains(fen))
            {
                throw new ArgumentException($"Invalid FEN, illegal character for side to move: '{fen}'");
            }
            sideToMove = fen == "w" ? Color.White : Color.Black;
        }

        private void SetCastlingRights(string fen)
        {
            castlingRights = 0;
            if (fen == "-") return;

            foreach (char character in fen)
            {
                if (!fen.All(character => "KQkq-".Contains(character)))
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

        private void SetEPSquare(string fen)
        {
            
        }

        private void SetHalfMove(string fen)
        {
            
        }

        private void SetFullMove(string fen)
        {
            
        }
    }
}