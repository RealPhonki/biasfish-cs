using System.Numerics;
using Biasfish.Core;

namespace Biasfish
{
    public static class Queens
    {
        public static void GetPseudoLegal(ref Board board, ref MoveList moveList)
        {
            ulong queens = board.Get(Piece.Queens | board.sideToMove);

            while (queens != 0)
            {
                int fromSquare = BitOperations.TrailingZeroCount(queens);
                ulong queenAttacks = Rooks.GetAttackBitboard(ref board, fromSquare) | Bishops.GetAttackBitboard(ref board, fromSquare);

                MoveGeneration.SerializeMoves(ref board, ref moveList, queenAttacks, fromSquare);
                queens &= queens - 1;
            }
        }
    }
}