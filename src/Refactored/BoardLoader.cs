namespace Biasfish.V2
{
    public partial struct Board
    {
        public Board()
        {
            LoadFen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        }

        public Board(string fenString)
        {
            LoadFen(fenString);
        }

        public void LoadFen(string fenString)
        {
            ClearBoardState();
            // TODO: ClearStateHistory();

            string[] fenParts = fenString.Split(' ');

            ParseBoardState(fenParts[0]);
            ParseSideToMove(fenParts[1]);
            ParseCastlingRights(fenParts[2]);
            // TODO: ParseEpSquare(fenParts[3]);
            // TODO: ParseHalfMoveClock(fenParts[4]);
            // TODO: ParseFullMoveClock(fenParts[5]);
        }

        private static int PieceTypeFromSymbol(char symbol)
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

        private static int PieceColorFromSymbol(char symbol)
        {
            return symbol switch
            {
                'P' => Piece.White,
                'N' => Piece.White,
                'B' => Piece.White,
                'R' => Piece.White,
                'Q' => Piece.White,
                'K' => Piece.White,
                'p' => Piece.Black,
                'n' => Piece.Black,
                'b' => Piece.Black,
                'r' => Piece.Black,
                'q' => Piece.Black,
                'k' => Piece.Black,
                _ => throw new ArgumentException($"Invalid FEN character: {symbol}")
            };
        }

        private void ClearBoardState()
        {
            for (int square = 0; square < 64; square++)
            {
                RemovePiece(square);
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
                    int piece = PieceTypeFromSymbol(symbol);
                    int color = PieceColorFromSymbol(symbol);
                    int square = rank * 8 + file;

                    AddPiece(piece, square, color);

                    file++;
                }
            }
        }

        private void ParseSideToMove(string fenString)
        {
            sideToMove = fenString == "w" ? Piece.White : Piece.Black;
        }

        private void ParseCastlingRights(string fenString)
        {
            castlingRights = 0;
            if (fenString.Contains('K')) castlingRights |= CastlingRights.WhiteKingSide;
            if (fenString.Contains('Q')) castlingRights |= CastlingRights.WhiteQueenSide;
            if (fenString.Contains('k')) castlingRights |= CastlingRights.BlackKingSide;
            if (fenString.Contains('q')) castlingRights |= CastlingRights.BlackQueenSide;
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