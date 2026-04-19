namespace Biasfish.V2
{
    public partial struct Board
    {
        public Board()
        {
            Set("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        }

        public Board(string fenString)
        {
            Set(fenString);
        }

        public void Set(string fenString)
        {
            ClearBoardState();
            // TODO: ClearStateHistory();

            string[] fenParts = fenString.Split(' ');

            ParseBoardState(fenParts[0]);
            ParseSideToMove(fenParts[1]);
            // TODO: ParseCastlingRights(fenParts[2]);
            // TODO: ParseEpSquare(fenParts[3]);
            // TODO: ParseHalfMoveClock(fenParts[4]);
            // TODO: ParseFullMoveClock(fenParts[5]);
        }

        private static Piece PieceTypeFromSymbol(char symbol)
        {
            return symbol switch
            {
                'p' => Piece.Pawn,
                'n' => Piece.Knight,
                'b' => Piece.Bishop,
                'r' => Piece.Rook,
                'q' => Piece.Queen,
                'k' => Piece.King,
                'P' => Piece.Pawn,
                'N' => Piece.Knight,
                'B' => Piece.Bishop,
                'R' => Piece.Rook,
                'Q' => Piece.Queen,
                'K' => Piece.King,
                _ => throw new ArgumentException($"Invalid FEN character: {symbol}")
            };
        }

        private static Color PieceColorFromSymbol(char symbol)
        {
            return symbol switch
            {
                'P' => Color.White,
                'N' => Color.White,
                'B' => Color.White,
                'R' => Color.White,
                'Q' => Color.White,
                'K' => Color.White,
                'p' => Color.Black,
                'n' => Color.Black,
                'b' => Color.Black,
                'r' => Color.Black,
                'q' => Color.Black,
                'k' => Color.Black,
                _ => throw new ArgumentException($"Invalid FEN character: {symbol}")
            };
        }

        private void ClearBoardState()
        {
            for (int square = 0; square < 64; square++)
            {
                RemovePiece((Square)square);
            }
        }

        private void ClearStateHistory()
        {
            throw new NotImplementedException();
        }

        private void ParseBoardState(string fenString)
        {
            int rank = 7;
            int file = 0;
            foreach (char symbol in fenString)
            {
                if (symbol == '/')
                {
                    rank--;
                    file = 0;
                }

                else if (char.IsDigit(symbol))
                {
                    file += symbol - '0';
                }

                else
                {
                    Piece piece = PieceTypeFromSymbol(symbol);
                    Color color = PieceColorFromSymbol(symbol);
                    Square square = (Square)(rank * 8 + file);

                    AddPiece(piece, color, square);

                    file++;
                }
            }
        }

        private void ParseSideToMove(string fenString)
        {
            sideToMove = fenString == "w" ? Color.White : Color.Black;
        }

        private void ParseCastlingRights(string fenString)
        {
            throw new NotImplementedException();
        }

        private void ParseEpSquare(string fenString)
        {
            throw new NotImplementedException();
        }

        private void ParseHalfMoveClock(string fenString)
        {
            throw new NotImplementedException();
        }

        private void ParseFullMoveClock(string fenString)
        {
            throw new NotImplementedException();
        }
    }
}