namespace Biasfish.V2
{
    public partial struct Board
    {
        public void Push(Move move)
        {
            // TODO: Save current state to state history

            int piece = PieceAt(move.FromSquare);
            int color = ColorAt(move.FromSquare);

            // Handle captures
            int enemy = PieceAt(move.ToSquare);
            if (enemy != Piece.Null)
            {
                RemovePiece(enemy, move.ToSquare);
            }

            // Move hero piece
            RemovePiece(piece, move.FromSquare, color);
            AddPiece(piece, move.ToSquare, color);

            // TODO: Handle promotions

            // TODO: Handle en passant

            // TODO: Handle castling

            // TODO: Update castling rights

            // TODO: Update Ep square

            // TODO: Update half-move clock

            // TODO: Increment sideToMove

            // TODO: Update zobrist key
        }
    }
}