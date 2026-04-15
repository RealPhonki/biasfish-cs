using System.Numerics;
using Biasfish.Core;

namespace Biasfish.MoveGeneration
{
    public static class Queens
    {
        public static void GetPseudoLegal(ref Board board, ref MoveList moveList)
        {
            ulong queens = board.Get(Piece.Queens | board.sideToMove);

            while (queens != 0)
            {
                int fromSquare = BitOperations.TrailingZeroCount(queens);
                ulong occupied = board.GetOccupied();
                ulong queenAttacks = Rooks.GetAttackBitboard(fromSquare, occupied) | Bishops.GetAttackBitboard(fromSquare, occupied);

                MoveGeneration.SerializeMoves(ref board, ref moveList, queenAttacks, fromSquare);
                queens &= queens - 1;
            }
        }
    }
}