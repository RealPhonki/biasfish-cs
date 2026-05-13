using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Biasfish.Core
{
    public unsafe partial struct Position
    {
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

        // TODO: Make all mutator methods private
        [Conditional("DEBUG")]
        public void ValidateSquare(Piece piece, Square square)
        {
            // search mailbox
            if ((Piece)board[(int)square] != piece)
            {
                throw new InvalidOperationException($"Piece '{piece}' does not exist at square '{square}'");
            }

            // search all type bitboards
            if ((this.Pieces(piece.Type()) & Masks.Square[(int)square]) == 0)
            {
                throw new InvalidOperationException($"Piece '{piece}' does not exist at square '{square}'");
            }

            // search all color bitboards
            if ((this.Pieces(piece.Color()) & Masks.Square[(int)square]) == 0)
            {
                throw new InvalidOperationException($"Piece '{piece}' does not exist at square '{square}'");
            }
        }

        [Conditional("DEBUG")]
        public void ValidateEmptySquare(Square square)
        {
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
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PutPiece(Piece piece, Square square)
        {
            ValidateEmptySquare(square);

            ByColorBB[(int)piece.Color()] |= Masks.Square[(int)square];
            ByTypeBB[(int)piece.Type()]    |= Masks.Square[(int)square];
            board[(int)square] = (byte)piece;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemovePiece(Piece piece, Square square)
        {
            ValidateSquare(piece, square);

            ByColorBB[(int)piece.Color()] &= ~Masks.Square[(int)square];
            ByTypeBB[(int)piece.Type()]    &= ~Masks.Square[(int)square];
            board[(int)square] = (byte)Piece.NoPiece;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MovePiece(Piece piece, Square from, Square to)
        {
            ValidateSquare(piece, from);
            ValidateEmptySquare(to);

            ByColorBB[(int)piece.Color()] ^= Masks.Square[(int)from] | Masks.Square[(int)to];
            ByTypeBB[(int)piece.Type()]    ^= Masks.Square[(int)from] | Masks.Square[(int)to];
            board[(int)from] = (byte)Piece.NoPiece;
            board[(int)to]   = (byte)piece;
        }
    }
}