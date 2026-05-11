namespace Biasfish.Core
{
    public unsafe partial struct Position
    {
        public void Set(string fenString)
        {
            // clear board data
            for (int i = 0; i < (int)PieceType.PieceTypeNB; i++)
            {
                ByTypeBB[i] = 0;
            }

            for (int i = 0; i < (int)Color.ColorNB; i++)
            {
                ByColorBB[i] = 0;
            }

            for (int i = 0; i < (int)Square.SquareNB; i++)
            {
                board[i] = (byte)Piece.NoPiece;
            }

            // TODO: Use read-only span
            // validate fen
            string[] fenParts = fenString.Split(' ');
            if (fenParts.Length != 6)
            {
                throw new ArgumentException($"Invalid FEN: '{fenString}'");
            }

            SetBoard(fenParts[0]);
            SetSideToMove(fenParts[1]);
            SetCastlingRights(fenParts[2]);
            SetEPSquare(fenParts[3]);
            SetHalfMove(fenParts[4]);
            SetFullMove(fenParts[5]);
        }

        private void SetBoard(string fen)
        {
            // start on A8 moving rightwards
            int rank = 7;
            int file = 0;
            foreach (char symbol in fen)
            {
                // move down one rank
                if (symbol == '/')
                {
                    rank--;
                    file = 0;
                }

                // convert the symbol to an int and move right
                else if (char.IsDigit(symbol))
                {
                    file += symbol - '0';
                }

                // convert the symbol into a piece type
                else
                {
                    // TODO: check if FromChar is inlineable
                    // parse symbol
                    Piece piece = Piece.FromChar(symbol);
                    int square = rank * 8 + file;

                    // place pieces
                    ByColorBB[(int)piece.GetColor()] |= Masks.Square[square];
                    ByTypeBB[(int)piece.TypeOf()]    |= Masks.Square[square];
                    board[square] = (byte)piece;

                    // move to the next file
                    file++;
                }
            }
        }

        private void SetSideToMove(string fen)
        {
            sideToMove = fen == "w" ? Color.White : Color.Black;
        }

        private void SetCastlingRights(string fen)
        {
            castlingRights = 0;
            if (fen == "-") return;

            foreach (char character in fen)
            {
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