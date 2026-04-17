using System.Diagnostics;

namespace Biasfish.Core
{
    public struct BoardState
    {
        public int EpSquare;
        public int CastlingRights;
        public int HalfmoveClock;
        public int CapturedPiece;
    }

    /// <summary>
    /// Represents a board state and metadata.
    /// * Note: This struct should be passed into methods using the `ref` keyword.
    /// </summary>
    public struct Board
    {
        private BoardState[] stateHistory;
        private int plyCount;
        // The board state is represented by 12 unsigned 64-bit integers where
        // each active bit represents whether or not a piece is located there.
        // This allows for fast mutations through bitwise operations.
        // * ref: https://www.chessprogramming.org/Bitboards
        private unsafe fixed ulong Bitboards[16];

        private unsafe fixed byte MailBox[64];

        // Pieces are represented with 4 bits where the first three bits represent the
        // piece type and the fourth bit represents the color. Because of this, sideToMove
        // is represented as either 0 (0000) or 8 (1000).
        public int sideToMove;
        public int currEpSquare;
        public int lastEpSquare;

        // Castling rights are represented with 4 bits with the following format:
        // (black queenside)(black kingside)(white queenside)(white kingside)
        public int castlingRights;

        // TODO: This will be used for 50-move rule
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

                // clear mailbox
                for (int index = 0; index < 64; index++)
                {
                    MailBox[index] = Piece.Null;
                }

                // clear state history
                stateHistory = new BoardState[1024];
                plyCount = 0;
                
                // parse parts
                string[] fenParts = fenString.Split(' ');

                // parse board
                string fenBoard = fenParts[0];

                // parse turn
                sideToMove = fenParts[1] == "w" ? Piece.White : Piece.Black;

                // parse rights
                castlingRights = 0;
                if (fenParts[2][0] == 'K') castlingRights |= CastlingRights.WhiteKingSide;
                if (fenParts[2][1] == 'Q') castlingRights |= CastlingRights.WhiteQueenSide;
                if (fenParts[2][2] == 'k') castlingRights |= CastlingRights.BlackKingSide;
                if (fenParts[2][3] == 'q') castlingRights |= CastlingRights.BlackQueenSide;

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
                        Bitboards[pieceType] |= Masks.Square[square];
                        Bitboards[pieceColor] |= Masks.Square[square];

                        MailBox[square] = (byte)pieceType;
                        
                        file++;
                    }
                }
            }
        }

        public bool CanWhiteCastleKingside()
        {
            return (castlingRights & CastlingRights.WhiteKingSide) != 0;
        }

        public bool CanWhiteCastleQueenside()
        {
            return (castlingRights & CastlingRights.WhiteQueenSide) != 0;
        }
        public bool CanBlackCastleKingside()
        {
            return (castlingRights & CastlingRights.BlackKingSide) != 0;
        }

        public bool CanBlackCastleQueenside()
        {
            return (castlingRights & CastlingRights.BlackQueenSide) != 0;
        }

        // ! The following was written by Gemini
        public void Pop(Move move)
        {
            unsafe
            {
                // 1. Restore previous metadata state
                plyCount--;
                BoardState previousState = stateHistory[plyCount];
                currEpSquare   = previousState.EpSquare;
                castlingRights = previousState.CastlingRights;
                halfMoveClock  = previousState.HalfmoveClock;

                // 2. Revert the side to move to the player who actually made the move
                sideToMove = Piece.FlipColor(sideToMove);

                // 3. Route to the appropriate unmake handler based on move flags
                if      (Flags.IsQuiet(move.Flags))          UnmakeQuiet(move);
                else if (Flags.IsCaptureOnly(move.Flags))    UnmakeCapture(move, previousState.CapturedPiece);
                else if (Flags.IsDoublePawnPush(move.Flags)) UnmakeQuiet(move); // Double pawn push unmake is mechanically identical to a quiet move
                else if (Flags.IsKingCastle(move.Flags))     UnmakeKingCastle();
                else if (Flags.IsQueenCastle(move.Flags))    UnmakeQueenCastle();
                else if (Flags.IsPromotion(move.Flags))      UnmakePromotion(move, previousState.CapturedPiece);
                else if (Flags.IsEnPassant(move.Flags))      UnmakeEnPassant(move);
            }
        }

        private unsafe void UnmakeQuiet(Move move)
        {
            int pieceType = PieceAt(move.ToSquare);

            // Revert bitboards using XOR
            Bitboards[pieceType]  ^= Masks.Square[move.FromSquare] | Masks.Square[move.ToSquare];
            Bitboards[sideToMove] ^= Masks.Square[move.FromSquare] | Masks.Square[move.ToSquare];

            // Revert mailbox
            MailBox[move.FromSquare] = (byte)pieceType;
            MailBox[move.ToSquare]   = Piece.Null;
        }

        private unsafe void UnmakeCapture(Move move, int capturedPiece)
        {
            int pieceType = PieceAt(move.ToSquare);
            int enemyColor = Piece.FlipColor(sideToMove);

            // Move the attacking piece back
            Bitboards[pieceType]  ^= Masks.Square[move.FromSquare] | Masks.Square[move.ToSquare];
            Bitboards[sideToMove] ^= Masks.Square[move.FromSquare] | Masks.Square[move.ToSquare];

            // Restore the captured piece
            Bitboards[capturedPiece] ^= Masks.Square[move.ToSquare];
            Bitboards[enemyColor]    ^= Masks.Square[move.ToSquare];

            // Revert mailbox
            MailBox[move.FromSquare] = (byte)pieceType;
            MailBox[move.ToSquare]   = (byte)capturedPiece;
        }

        private unsafe void UnmakeKingCastle()
        {
            if (sideToMove == Piece.White)
            {
                Bitboards[Piece.Kings | sideToMove] ^= Masks.Square[Squares.E1] | Masks.Square[Squares.G1];
                Bitboards[Piece.Rooks | sideToMove] ^= Masks.Square[Squares.F1] | Masks.Square[Squares.H1];
                Bitboards[sideToMove] ^= Masks.Square[Squares.E1] | Masks.Square[Squares.F1] | Masks.Square[Squares.G1] | Masks.Square[Squares.H1];

                MailBox[Squares.E1] = (byte)(Piece.Kings | sideToMove);
                MailBox[Squares.F1] = Piece.Null;
                MailBox[Squares.G1] = Piece.Null;
                MailBox[Squares.H1] = (byte)(Piece.Rooks | sideToMove);
            }
            else
            {
                Bitboards[Piece.Kings | sideToMove] ^= Masks.Square[Squares.E8] | Masks.Square[Squares.G8];
                Bitboards[Piece.Rooks | sideToMove] ^= Masks.Square[Squares.F8] | Masks.Square[Squares.H8];
                Bitboards[sideToMove] ^= Masks.Square[Squares.E8] | Masks.Square[Squares.F8] | Masks.Square[Squares.G8] | Masks.Square[Squares.H8];

                MailBox[Squares.E8] = (byte)(Piece.Kings | sideToMove);
                MailBox[Squares.F8] = Piece.Null;
                MailBox[Squares.G8] = Piece.Null;
                MailBox[Squares.H8] = (byte)(Piece.Rooks | sideToMove);
            }
        }

        private unsafe void UnmakeQueenCastle()
        {
            if (sideToMove == Piece.White)
            {
                Bitboards[Piece.Rooks | sideToMove] ^= Masks.Square[Squares.A1] | Masks.Square[Squares.D1];
                Bitboards[Piece.Kings | sideToMove] ^= Masks.Square[Squares.C1] | Masks.Square[Squares.E1];
                Bitboards[sideToMove] ^= Masks.Square[Squares.A1] | Masks.Square[Squares.C1] | Masks.Square[Squares.E1] | Masks.Square[Squares.D1];

                MailBox[Squares.A1] = (byte)(Piece.Rooks | sideToMove);
                MailBox[Squares.C1] = Piece.Null;
                MailBox[Squares.D1] = Piece.Null;
                MailBox[Squares.E1] = (byte)(Piece.Kings | sideToMove);
            }
            else
            {
                Bitboards[Piece.Rooks | sideToMove] ^= Masks.Square[Squares.A8] | Masks.Square[Squares.D8];
                Bitboards[Piece.Kings | sideToMove] ^= Masks.Square[Squares.C8] | Masks.Square[Squares.E8];
                Bitboards[sideToMove] ^= Masks.Square[Squares.A8] | Masks.Square[Squares.C8] | Masks.Square[Squares.E8] | Masks.Square[Squares.D8];

                MailBox[Squares.A8] = (byte)(Piece.Rooks | sideToMove);
                MailBox[Squares.C8] = Piece.Null;
                MailBox[Squares.D8] = Piece.Null;
                MailBox[Squares.E8] = (byte)(Piece.Kings | sideToMove);
            }
        }

        private unsafe void UnmakePromotion(Move move, int capturedPiece)
        {
            int promotedPiece = PieceAt(move.ToSquare);
            int pawnType = Piece.Pawns | sideToMove;

            // Remove the promoted piece from the destination square
            Bitboards[promotedPiece] ^= Masks.Square[move.ToSquare];
            
            // Add the pawn back to the origin square
            Bitboards[pawnType] ^= Masks.Square[move.FromSquare];
            
            // Shift the color occupancy back
            Bitboards[sideToMove] ^= Masks.Square[move.FromSquare] | Masks.Square[move.ToSquare];

            MailBox[move.FromSquare] = (byte)pawnType;
            MailBox[move.ToSquare]   = Piece.Null;

            // If it was a capture-promotion, restore the captured piece
            if (Flags.IsCapture(move.Flags))
            {
                int enemyColor = Piece.FlipColor(sideToMove);
                Bitboards[capturedPiece] ^= Masks.Square[move.ToSquare];
                Bitboards[enemyColor]    ^= Masks.Square[move.ToSquare];
                
                MailBox[move.ToSquare] = (byte)capturedPiece;
            }
        }

        private unsafe void UnmakeEnPassant(Move move)
        {
            int captureSquare = (sideToMove == Piece.White) ? move.ToSquare - 8 : move.ToSquare + 8;
            int pawnType = Piece.Pawns | sideToMove;
            
            int enemyColor = Piece.FlipColor(sideToMove);
            int enemyPawnType = Piece.Pawns | enemyColor;

            // Move hero pawn back
            Bitboards[pawnType]   ^= Masks.Square[move.FromSquare] | Masks.Square[move.ToSquare];
            Bitboards[sideToMove] ^= Masks.Square[move.FromSquare] | Masks.Square[move.ToSquare];

            // Restore the captured enemy pawn
            Bitboards[enemyPawnType] ^= Masks.Square[captureSquare];
            Bitboards[enemyColor]    ^= Masks.Square[captureSquare];

            // Revert mailbox
            MailBox[move.FromSquare] = (byte)pawnType;
            MailBox[move.ToSquare]   = Piece.Null;
            MailBox[captureSquare]   = (byte)enemyPawnType;
        }
        // ! End of Gemini code

        /// <summary>
        /// Plays a given move.
        /// </summary>
        /// <param name="move">Represents the move</param>
        public void Push(Move move)
        {
            unsafe
            {
                // save the current state in the state history
                stateHistory[plyCount] = new BoardState
                {
                    EpSquare = currEpSquare,
                    CastlingRights = castlingRights,
                    HalfmoveClock = halfMoveClock,
                    CapturedPiece = PieceAt(move.ToSquare)
                };
                plyCount++;

                // parse move data
                int pieceType = PieceAt(move.FromSquare);

                lastEpSquare = currEpSquare;
                currEpSquare = Squares.Null;

                // Update castling rights
                int neutralType = Piece.GetNeutral(pieceType);
                if (neutralType == Piece.Rooks)              HandleRookMoves(move);
                if (neutralType == Piece.Kings)              HandleKingMoves(move);

                if      (Flags.IsQuiet(move.Flags))          HandleQuiet(move, pieceType);
                else if (Flags.IsCaptureOnly(move.Flags))    HandleCapture(move, pieceType);
                else if (Flags.IsDoublePawnPush(move.Flags)) HandleDoublePawnPush(move, pieceType);
                else if (Flags.IsKingCastle(move.Flags))     HandleKingCastle(move);
                else if (Flags.IsQueenCastle(move.Flags))    HandleQueenCastle(move);
                else if (Flags.IsPromotion(move.Flags))      HandlePromotion(move);
                else if (Flags.IsEnPassant(move.Flags))      HandleEnPassant(move);

                sideToMove = Piece.FlipColor(sideToMove);
            }
        }

        private void HandleRookMoves(Move move)
        {
            if (sideToMove == Piece.White)
            {
                if (move.FromSquare == Squares.A1)
                {
                    castlingRights &= CastlingRights.WhiteQueenSideMask;
                }
                else if (move.FromSquare == Squares.H1)
                {
                    castlingRights &= CastlingRights.WhiteKingSideMask;
                }
            }
            else
            {
                if (move.FromSquare == Squares.A8)
                {
                    castlingRights &= CastlingRights.BlackQueenSideMask;
                }
                else if (move.FromSquare == Squares.H8)
                {
                    castlingRights &= CastlingRights.BlackKingSideMask;
                }
            }
        }

        private void HandleKingMoves(Move move)
        {
            if (sideToMove == Piece.White)
            {
                castlingRights &= CastlingRights.WhiteMask;
            }
            else
            {
                castlingRights &= CastlingRights.BlackMask;
            }
        }

        private unsafe void HandleQuiet(Move move, int pieceType)
        {
            Bitboards[pieceType]  ^= Masks.Square[move.FromSquare] | Masks.Square[move.ToSquare];
            Bitboards[sideToMove] ^= Masks.Square[move.FromSquare] | Masks.Square[move.ToSquare];

            MailBox[move.FromSquare] = Piece.Null;
            MailBox[move.ToSquare] = (byte)pieceType;
        }

        private unsafe void HandleCapture(Move move, int pieceType)
        {
            int enemyType = PieceAt(move.ToSquare);

            Bitboards[pieceType]   ^= Masks.Square[move.FromSquare] | Masks.Square[move.ToSquare];
            Bitboards[sideToMove]  ^= Masks.Square[move.FromSquare] | Masks.Square[move.ToSquare];

            Bitboards[enemyType]   ^= Masks.Square[move.ToSquare];
            Bitboards[Piece.FlipColor(sideToMove)] ^= Masks.Square[move.ToSquare];

            MailBox[move.FromSquare] = Piece.Null;
            MailBox[move.ToSquare] = (byte)pieceType;
        }

        private unsafe void HandleDoublePawnPush(Move move, int pieceType)
        {
            HandleQuiet(move, pieceType);
            currEpSquare = sideToMove == Piece.White ? move.ToSquare - 8: move.ToSquare + 8;
        }

        private unsafe void HandleKingCastle(Move move)
        {
            if (sideToMove == Piece.White)
            {
                // update castling rights
                castlingRights &= CastlingRights.WhiteMask;
                
                Bitboards[Piece.Kings | sideToMove] ^= Masks.Square[Squares.E1] | Masks.Square[Squares.G1];
                Bitboards[Piece.Rooks | sideToMove] ^= Masks.Square[Squares.F1] | Masks.Square[Squares.H1];
                Bitboards[sideToMove] ^= Masks.Square[Squares.E1] | Masks.Square[Squares.F1] | Masks.Square[Squares.G1] | Masks.Square[Squares.H1];

                MailBox[Squares.E1] = Piece.Null;
                MailBox[Squares.F1] = Piece.Rooks;
                MailBox[Squares.G1] = Piece.Kings;
                MailBox[Squares.H1] = Piece.Null;
            }
            else
            {
                // update castling rights
                castlingRights &= CastlingRights.BlackMask;

                Bitboards[Piece.Kings | sideToMove] ^= Masks.Square[Squares.E8] | Masks.Square[Squares.G8];
                Bitboards[Piece.Rooks | sideToMove] ^= Masks.Square[Squares.F8] | Masks.Square[Squares.H8];
                Bitboards[sideToMove] ^= Masks.Square[Squares.E8] | Masks.Square[Squares.F8] | Masks.Square[Squares.G8] | Masks.Square[Squares.H8];

                MailBox[Squares.E8] = Piece.Null;
                MailBox[Squares.F8] = Piece.Rooks | Piece.Black;
                MailBox[Squares.G8] = Piece.Kings | Piece.Black;
                MailBox[Squares.H8] = Piece.Null;
            }
        }

        private unsafe void HandleQueenCastle(Move move)
        {
            if (sideToMove == Piece.White)
            {
                Bitboards[Piece.Rooks | sideToMove] ^= Masks.Square[Squares.A1] | Masks.Square[Squares.D1];
                Bitboards[Piece.Kings | sideToMove] ^= Masks.Square[Squares.C1] | Masks.Square[Squares.E1];
                Bitboards[sideToMove] ^= Masks.Square[Squares.A1] | Masks.Square[Squares.C1] | Masks.Square[Squares.E1] | Masks.Square[Squares.D1];

                MailBox[Squares.A1] = Piece.Null;
                MailBox[Squares.C1] = Piece.Kings;
                MailBox[Squares.D1] = Piece.Rooks;
                MailBox[Squares.E1] = Piece.Null;
            }
            else
            {
                Bitboards[Piece.Rooks | sideToMove] ^= Masks.Square[Squares.A8] | Masks.Square[Squares.D8];
                Bitboards[Piece.Kings | sideToMove] ^= Masks.Square[Squares.C8] | Masks.Square[Squares.E8];
                Bitboards[sideToMove] ^= Masks.Square[Squares.A8] | Masks.Square[Squares.C8] | Masks.Square[Squares.E8] | Masks.Square[Squares.D8];

                MailBox[Squares.A8] = Piece.Null;
                MailBox[Squares.C8] = Piece.Kings | Piece.Black;
                MailBox[Squares.D8] = Piece.Rooks | Piece.Black;
                MailBox[Squares.E8] = Piece.Null;
            }
        }

        private unsafe void HandlePromotion(Move move)
        {
            int newPiece;
            switch (move.Flags & ~Flags.Capture)
            {
                case Flags.KnightPromote: newPiece = Piece.Knights | sideToMove; break;
                case Flags.BishopPromote: newPiece = Piece.Bishops | sideToMove; break;
                case Flags.RookPromote:   newPiece = Piece.Rooks   | sideToMove; break;
                case Flags.QueenPromote:  newPiece = Piece.Queens  | sideToMove; break;
                default: throw new UnreachableException($"Illegal move flag: {move.Flags}");
            }

            if (Flags.IsCapture(move.Flags))
            {
                // clear hero pawn
                Bitboards[Piece.Pawns | sideToMove] ^= Masks.Square[move.FromSquare];
                
                // create new piece
                Bitboards[newPiece]   ^= Masks.Square[move.ToSquare];
                Bitboards[sideToMove] ^= Masks.Square[move.FromSquare] | Masks.Square[move.ToSquare];

                // clear enemy piece
                int enemyType = PieceAt(move.ToSquare);
                Bitboards[enemyType]                 ^= Masks.Square[move.ToSquare];
                Bitboards[Piece.GetColor(enemyType)] ^= Masks.Square[move.ToSquare];
            }
            else
            {
                // clear hero pawn
                Bitboards[Piece.Pawns | sideToMove] ^= Masks.Square[move.FromSquare];
                
                // create new piece
                Bitboards[newPiece]   ^= Masks.Square[move.ToSquare];
                Bitboards[sideToMove] ^= Masks.Square[move.FromSquare] | Masks.Square[move.ToSquare];
            }

            // update mailbox
            MailBox[move.FromSquare] = Piece.Null;
            MailBox[move.ToSquare]   = (byte)newPiece;
        }

        private unsafe void HandleEnPassant(Move move)
        {
            int captureSquare = (sideToMove == Piece.White) ? move.ToSquare - 8 : move.ToSquare + 8;

            // move hero pawn
            Bitboards[Piece.Pawns | sideToMove] ^= Masks.Square[move.FromSquare] | Masks.Square[move.ToSquare];
            Bitboards[sideToMove]               ^= Masks.Square[move.FromSquare] | Masks.Square[move.ToSquare];

            // clear enemy pawn
            int enemyColor = Piece.FlipColor(sideToMove);
            Bitboards[Piece.Pawns | enemyColor] ^= Masks.Square[captureSquare];
            Bitboards[enemyColor]               ^= Masks.Square[captureSquare];

            // update mailbox
            MailBox[move.FromSquare] = Piece.Null;
            MailBox[move.ToSquare] = (byte)(Piece.Pawns | sideToMove);
            MailBox[captureSquare] = Piece.Null;
        }

        public ulong GetOccupied()
        {
            unsafe
            {
                return Bitboards[Piece.White] | Bitboards[Piece.Black];
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