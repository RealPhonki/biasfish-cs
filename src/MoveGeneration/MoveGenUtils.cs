using System.Numerics;

namespace Biasfish.Core
{
    public static class MoveGenUtils
    {
        public static void SerializeMoves(ref MoveList moveList, ref Board board, ulong attackBitboard, int fromSquare)
        {
            ulong enemy = board.Get(Piece.FlipColor(board.sideToMove));
            ulong empty = ~board.Get(Piece.Any);

            ulong captureMoves = attackBitboard & enemy;
            while (captureMoves != 0)
            {
                moveList.Add(new Move(fromSquare, BitOperations.TrailingZeroCount(captureMoves), Flag.Capture));
                captureMoves &= captureMoves - 1;
            }

            ulong quietMoves = attackBitboard & empty;
            while (quietMoves != 0)
            {
                moveList.Add(new Move(fromSquare, BitOperations.TrailingZeroCount(quietMoves), Flag.Quiet));
                quietMoves &= quietMoves - 1;
            }
        }
    }
}