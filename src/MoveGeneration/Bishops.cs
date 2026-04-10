using System.Numerics;
using Biasfish.Core;

namespace Biasfish
{
    public static class Bishops
    {
        public static readonly ulong[] NorthEastRay = new ulong[64];
        public static readonly ulong[] NorthWestRay = new ulong[64];
        public static readonly ulong[] SouthEastRay = new ulong[64];
        public static readonly ulong[] SouthWestRay = new ulong[64];

        static Bishops()
        {
            for (int square = 0; square < 64; square++)
            {
                int x = square % 8 + 1;
                int y = square / 8 + 1;

                while (x < 8 && y < 8)
                {
                    NorthEastRay[square] |= 1UL << (x + 8 * y);
                    x++;
                    y++;
                }

                x = square % 8 - 1;
                y = square / 8 + 1;

                while (x >= 0 && y < 8)
                {
                    NorthWestRay[square] |= 1UL << (x + 8 * y);
                    x--;
                    y++;
                }

                x = square % 8 + 1;
                y = square / 8 - 1;

                while (x < 8 && y >= 0)
                {
                    SouthEastRay[square] |= 1UL << (x + 8 * y);
                    x++;
                    y--;
                }

                x = square % 8 - 1;
                y = square / 8 - 1;

                while (x >= 0 && y >= 0)
                {
                    SouthWestRay[square] |= 1UL << (x + 8 * y);
                    x--;
                    y--;
                }
            }
        }

        public static void GetPseudoLegal(ref Board board, ref MoveList moveList)
        {
            ulong bishops = board.Get(Piece.Bishops | board.sideToMove);

            ulong occupied = board.Get(Piece.Any);
            ulong enemy = board.Get(Piece.FlipColor(board.sideToMove));
            ulong empty = ~occupied;

            while (bishops != 0)
            {
                int fromSquare = BitOperations.TrailingZeroCount(bishops);
                ulong bishopAttacks = 0;
                ulong ray;
                ulong blockers;

                // scan north east
                ray = NorthEastRay[fromSquare];
                blockers = ray & occupied;
                if (blockers != 0)
                {
                    ray ^= NorthEastRay[BitOperations.TrailingZeroCount(blockers)];
                }
                bishopAttacks |= ray;

                // scan north west
                ray = NorthWestRay[fromSquare];
                blockers = ray & occupied;
                if (blockers != 0)
                {
                    ray ^= NorthWestRay[BitOperations.TrailingZeroCount(blockers)];
                }
                bishopAttacks |= ray;

                ray = SouthEastRay[fromSquare];
                blockers = ray & occupied;
                if (blockers != 0)
                {
                    ray ^= SouthEastRay[63 - BitOperations.LeadingZeroCount(blockers)];
                }
                bishopAttacks |= ray;

                ray = SouthWestRay[fromSquare];
                blockers = ray & occupied;
                if (blockers != 0)
                {
                    ray ^= SouthWestRay[BitOperations.TrailingZeroCount(blockers)];
                }
                bishopAttacks |= ray;

                bishops &= bishops - 1;

                ulong quietMoves = bishopAttacks & empty;
                ulong captureMoves = bishopAttacks & enemy;

                while (quietMoves != 0)
                {
                    int toSquare = BitOperations.TrailingZeroCount(quietMoves);
                    moveList.Add(new Move(fromSquare, toSquare, Flag.Quiet));

                    quietMoves &= quietMoves - 1;
                }

                while (captureMoves != 0)
                {
                    int toSquare = BitOperations.TrailingZeroCount(captureMoves);
                    moveList.Add(new Move(fromSquare, toSquare, Flag.Capture));

                    captureMoves &= captureMoves - 1;
                }
            }
        }
    }
}