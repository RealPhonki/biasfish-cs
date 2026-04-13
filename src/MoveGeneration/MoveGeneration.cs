using System.Numerics;

namespace Biasfish.Core
{
    public static class MoveGeneration
    {
        public static bool IsLegal(Move move)
        {
            // case 1 - king moves

            // case 2 - double check

            // case 3 - single check

            // case 4 - the piece is pinned

            // case 5 - en passant case
            
            return true;
        }

        public static void GetPseudoLegal(ref Board board, ref MoveList moveList)
        {
            Pawns.GetPseudoLegal(  ref board, ref moveList);
            Knights.GetPseudoLegal(ref board, ref moveList);
            Bishops.GetPseudoLegal(ref board, ref moveList);
            Rooks.GetPseudoLegal(  ref board, ref moveList);
            Queens.GetPseudoLegal( ref board, ref moveList);
            Kings.GetPseudoLegal(  ref board, ref moveList);
        }

        public static void SerializeMoves(ref Board board, ref MoveList moveList, ulong attackBitboard, int fromSquare)
        {
            ulong enemy = board.Get(Piece.FlipColor(board.sideToMove));
            ulong empty = ~board.Get(Piece.Any);

            ulong captureMoves = attackBitboard & enemy;
            while (captureMoves != 0)
            {
                moveList.Add(new Move(fromSquare, BitOperations.TrailingZeroCount(captureMoves), Flags.Capture));
                captureMoves &= captureMoves - 1;
            }

            ulong quietMoves = attackBitboard & empty;
            while (quietMoves != 0)
            {
                moveList.Add(new Move(fromSquare, BitOperations.TrailingZeroCount(quietMoves), Flags.Quiet));
                quietMoves &= quietMoves - 1;
            }
        }
    }
}