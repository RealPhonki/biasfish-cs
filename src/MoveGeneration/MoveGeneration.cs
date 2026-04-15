using System.Numerics;
using Biasfish.Core;

namespace Biasfish.MoveGeneration
{
    public static class MoveGeneration
    {
        // TODO: Castling into attacked squares
        public static void GetLegalMoves(ref Board board, ref MoveList moveList)
        {
            // get pseudo legal moves
            Span<Move> memoryBuffer = stackalloc Move[256];
            MoveList pseudoLegalMoves = new MoveList(memoryBuffer);
            GetPseudoLegal(ref board, ref moveList);

            int sideToMove = board.sideToMove;
            for (int i = 0; i < pseudoLegalMoves.count; i++)
            {
                Move move = pseudoLegalMoves[i];

                // play the move (this increments sideToMove)
                board.Push(move);

                ulong kings = board.Get(Piece.Kings | sideToMove);
                int kingSquare = BitOperations.TrailingZeroCount(kings);
                if (!IsSquareAttacked(ref board, kingSquare, sideToMove))
                {
                    moveList.Add(move);
                }

                board.Pop(move);
            }
        }

        public static bool IsSquareAttacked(ref Board board, int square, int sideToMove)
        {
            ulong occupied = board.GetOccupied();
            int enemyColor = Piece.FlipColor(sideToMove);

            // check pawn attacks
            ulong targetBb = 1UL << square;

            if (enemyColor == Piece.White)
            {
                // White pawns attack "up", so we shift the target square "down" to see if it hits a white pawn
                ulong attackers = ((targetBb >> 7) & Masks.NotFileH) | ((targetBb >> 9) & Masks.NotFileA);
                if ((attackers & board.Get(Piece.Pawns | Piece.White)) != 0) return true;
            }
            else
            {
                // Black pawns attack "down", so we shift the target square "up" to see if it hits a black pawn
                ulong attackers = ((targetBb << 7) & Masks.NotFileH) | ((targetBb << 9) & Masks.NotFileA);
                if ((attackers & board.Get(Piece.Pawns | Piece.Black)) != 0) return true;
            }

            // check knight attacks
            if ((Knights.KnightAttacks[square] & board.Get(Piece.Knights | enemyColor)) != 0)
            {
                return true;
            }

            // check knight attacks
            if ((Kings.KingAttacks[square] & board.Get(Piece.Kings | enemyColor)) != 0)
            {
                return true;
            }

            // check bishop attacks
            ulong bishopAttacks = Bishops.GetAttackBitboard(square, occupied);
            if ((bishopAttacks & (board.Get(Piece.Bishops | enemyColor) | board.Get(Piece.Queens | enemyColor))) != 0)
            {
                return true;
            }

            ulong rookAttacks = Rooks.GetAttackBitboard(square, occupied);
            if ((rookAttacks & (board.Get(Piece.Rooks | enemyColor) | board.Get(Piece.Queens | enemyColor))) != 0)
            {
                return true;
            }

            return false;
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
            ulong empty = ~board.GetOccupied();

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