namespace Biasfish.Core
{
    public struct Board
    {
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

        public int sideToMove;
        public int epSquare;
        public int castlingRights;
        public int halfMoveClock;
        public ulong key;

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

        public void LoadFEN(string fenString)
        {
            // clear bitboards
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
                if (symbol == '/')
                {
                    // skip to the next rank (row)
                    rank--;
                    file = 0;
                }
                else if (char.IsDigit(symbol))
                {
                    // convert the symbol into an integer and move rightwards by that amount
                    file += symbol - '0';
                }
                else
                {
                    // convert the symbol into a piece type
                    int pieceType = SymbolToPieceType(symbol);
                    int pieceColor = Piece.GetColor(pieceType);
                    int square = rank * 8 + file;

                    SetBitboard(pieceType, GetBitboard(pieceType) | (1UL << square));
                    SetBitboard(pieceColor, GetBitboard(pieceColor) | (1UL << square));
                    occupiedAll |= 1UL << square;

                    file++;
                }
            }
        }

        public void Push(Move move)
        {
            switch (move.Flags)
            {
                case 0:
                    int pieceType = PieceAt(move.FromSquare);
                    int pieceColor = Piece.GetColor(pieceType);
                    SetBitboard(pieceType, GetBitboard(pieceType) ^ (1UL << move.FromSquare | 1UL << move.ToSquare));
                    SetBitboard(pieceColor, GetBitboard(pieceColor) ^ (1UL << move.FromSquare | 1UL << move.ToSquare));
                    occupiedAll ^= 1UL << move.FromSquare | 1UL << move.ToSquare;
                    return;
            }
        }

        public int PieceAt(int square)
        {
            if ((occupiedAll & (1UL << square)) == 0)
            {
                return Piece.None;
            }
            foreach (int pieceType in Piece.PieceTypes)
            {
                if ((GetBitboard(pieceType) & (1UL << square)) != 0)
                {
                    return pieceType;
                }
            }
            throw new InvalidOperationException($"Invalid PieceAt({square}): Occupancy bitboards are not synced.");
        }

        public ulong GetBitboard(int pieceType)
        {
            switch (pieceType)
            {
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

        public void SetBitboard(int pieceType, ulong value)
        {
            switch (pieceType)
            {
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