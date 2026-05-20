using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Biasfish.Core
{
    public unsafe partial struct Position
    {

        /// <summary>
        /// Clears all elements in byTypeBB, byColorBB, and board
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Clear()
        {
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
        }

        /// <summary>
        /// Given a piece and a square performs the following:
        /// - Checks if the square is within 0-63
        /// - Checks if board contains the given piece at the square
        /// - Checks if byTypeBB contains the given piece at the square
        /// - Checks if byColorBB contains the given piece at the square
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="square"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        // TODO: Make all mutator methods private
        [Conditional("DEBUG")]
        public void ValidateSquare(Piece piece, Square square)
        {
            // ensure square is within bounds
            if (!Enum.IsDefined(typeof(Square), square))
            {
                throw new ArgumentOutOfRangeException($"Illegal square '{square}'");
            }

            // search mailbox
            if ((Piece)board[(int)square] != piece)
            {
                throw new InvalidOperationException($"Piece '{piece}' does not exist at square '{square}' in board");
            }

            // search all type bitboards
            if ((this.Pieces(piece.Type()) & Masks.Square[(int)square]) == 0)
            {
                throw new InvalidOperationException($"Piece '{piece}' does not exist at square '{square}' in ByTypeBB");
            }

            // search all color bitboards
            if ((this.Pieces(piece.Color()) & Masks.Square[(int)square]) == 0)
            {
                throw new InvalidOperationException($"Piece '{piece}' does not exist at square '{square}' in ByColorBB");
            }
        }

        /// <summary>
        /// Given a square performs the following:
        /// - Checks if the square is within 0-63
        /// - Checks if board contains a NoPiece entry at the given square
        /// - Checks if byTypeBB is empty for all elements at the given square
        /// - Checks if byColorBB is empty for all elements at the given square
        /// </summary>
        /// <param name="square"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        [Conditional("DEBUG")]
        public void ValidateEmptySquare(Square square)
        {
            // ensure square is within bounds
            if (!Enum.IsDefined(typeof(Square), square))
            {
                throw new ArgumentOutOfRangeException($"Invalid square caught: '{square}");
            }

            // search mailbox
            if ((Piece)board[(int)square] != Piece.NoPiece)
            {
                throw new InvalidOperationException("Cannot place piece on an occupied square");
            }

            // search all type bitboards
            for (PieceType pieceType = 0; pieceType < PieceType.PieceTypeNB; pieceType++)
            {
                if ((this.Pieces(pieceType) & Masks.Square[(int)square]) != 0)
                {
                    throw new InvalidOperationException("Cannot place piece on an occupied square");
                }
            }

            // search all color bitboards
            for (Color color = 0; color < Color.ColorNB; color++)
            {
                if ((this.Pieces(color) & Masks.Square[(int)square]) != 0)
                {
                    throw new InvalidOperationException("Cannot place piece on an occupied square");
                }
            }
        }
        
        /// <summary>
        /// - Sets byTypeBB[type] to 1 at the bit that corresponds with the given square
        /// - Sets byColorBB[color] to 1 at the bit that corresponds with the given square
        /// - Sets board[square] to the given piece
        /// 
        /// Contains conditional methods
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="square"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PutPiece(Piece piece, Square square)
        {
            ValidateEmptySquare(square);

            ByColorBB[(int)piece.Color()] |= Masks.Square[(int)square];
            ByTypeBB[(int)piece.Type()]   |= Masks.Square[(int)square];
            board[(int)square] = (byte)piece;
        }

        /// <summary>
        /// - Sets byTypeBB[type] to 0 at the bit that corresponds with the given square
        /// - Sets byColorBB[color] to 0 at the bit that corresponds with the given square
        /// - Sets board[square] to NoPiece
        /// 
        /// Contains conditional methods
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="square"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemovePiece(Piece piece, Square square)
        {
            ValidateSquare(piece, square);

            ByColorBB[(int)piece.Color()] &= ~Masks.Square[(int)square];
            ByTypeBB[(int)piece.Type()]   &= ~Masks.Square[(int)square];
            board[(int)square] = (byte)Piece.NoPiece;
        }

        /// <summary>
        /// - Applies xor to byColorBB[color] at from and to
        /// - Applies xor to byTypeBB[type] at from and to
        /// - Sets board[from] to NoPiece
        /// - Sets board[to] to the given piece
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MovePiece(Piece piece, Square from, Square to)
        {
            ValidateSquare(piece, from);
            ValidateEmptySquare(to);

            ByColorBB[(int)piece.Color()] ^= Masks.Square[(int)from] | Masks.Square[(int)to];
            ByTypeBB[(int)piece.Type()]   ^= Masks.Square[(int)from] | Masks.Square[(int)to];
            board[(int)from] = (byte)Piece.NoPiece;
            board[(int)to]   = (byte)piece;
        }
    }
}