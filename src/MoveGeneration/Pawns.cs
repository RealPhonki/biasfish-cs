using System.Numerics;

namespace Biasfish.Core
{
    public static class Pawns
    {

        private static void SerializeEnPassant(ref Board board, ref MoveList MoveList, ulong pawns)
        {
            if (board.currEpSquare == Squares.Null) return;

            ulong epTarget = 1UL << board.currEpSquare;

            if (board.sideToMove == Piece.White)
            {
                // back right attacker
                ulong rightAttacker = (epTarget >> 9) & Masks.NotFileA & pawns;
                if (rightAttacker != 0)
                {
                    MoveList.Add(new Move(BitOperations.TrailingZeroCount(rightAttacker), board.currEpSquare, Flags.EnPassant));
                }

                // back left attacker
                ulong leftAttacker = (epTarget >> 7) & Masks.NotFileH & pawns;
                if (leftAttacker != 0)
                {
                    MoveList.Add(new Move(BitOperations.TrailingZeroCount(leftAttacker), board.currEpSquare, Flags.EnPassant));
                }
            }
            else
            {
                // back right attacker (black's perspective)
                ulong rightAttacker = (epTarget << 9) & Masks.NotFileA & pawns;
                if (rightAttacker != 0)
                {
                    MoveList.Add(new Move(BitOperations.TrailingZeroCount(rightAttacker), board.currEpSquare, Flags.EnPassant));
                }

                // back left attacker (black's perspective)
                ulong leftAttacker = (epTarget << 7) & Masks.NotFileH & pawns;
                if (leftAttacker != 0)
                {
                    MoveList.Add(new Move(BitOperations.TrailingZeroCount(leftAttacker), board.currEpSquare, Flags.EnPassant));
                }
            }
        }

        public static void GetPseudoLegal(ref Board board, ref MoveList moveList)
        {
            ulong pawns = board.Get(Piece.Pawns | board.sideToMove);
            ulong empty = ~board.GetOccupied();
            ulong enemy = board.Get(Piece.FlipColor(board.sideToMove));

            SerializeEnPassant(ref board, ref moveList, pawns);

            if (board.sideToMove == Piece.White)
            {
                ulong promotionPawns = pawns & Masks.Rank7;
                pawns &= Masks.NotRank7;

                ulong singlePushes = (pawns << 8) & empty;
                SerializeMoves(ref moveList, singlePushes, 8, Flags.Quiet);

                ulong doublePushes = ((singlePushes & Masks.Rank3) << 8) & empty;
                SerializeMoves(ref moveList, doublePushes, 16, Flags.DoublePawnPush);

                ulong leftCaptures = (pawns << 7) & enemy & Masks.NotFileH;
                SerializeMoves(ref moveList, leftCaptures, 7, Flags.Capture);

                ulong rightCaptures = (pawns << 9) & enemy & Masks.NotFileA;
                SerializeMoves(ref moveList, rightCaptures, 9, Flags.Capture);

                ulong quietPromotions = (promotionPawns << 8) & empty;
                SerializePromotions(ref moveList, quietPromotions, 8, Flags.Quiet);

                ulong leftCapturePromotions = (promotionPawns << 7) & enemy & Masks.NotFileH;
                SerializePromotions(ref moveList, leftCapturePromotions, 7, Flags.Capture);

                ulong rightCapturePromotions = (promotionPawns << 9) & enemy & Masks.NotFileA;
                SerializePromotions(ref moveList, rightCapturePromotions, 9, Flags.Capture);
            }
            else
            {
                ulong promotionPawns = pawns & Masks.Rank2;
                pawns &= Masks.NotRank2;

                ulong singlePushes = (pawns >> 8) & empty;
                SerializeMoves(ref moveList, singlePushes,  -8, Flags.Quiet);

                ulong doublePushes = ((singlePushes & Masks.Rank6) >> 8) & empty;
                SerializeMoves(ref moveList, doublePushes, -16, Flags.DoublePawnPush);

                ulong leftCaptures = (pawns >> 7) & enemy & Masks.NotFileH;
                SerializeMoves(ref moveList, leftCaptures, -7, Flags.Capture);

                ulong rightCaptures = (pawns >> 9) & enemy & Masks.NotFileA;
                SerializeMoves(ref moveList, rightCaptures, -9, Flags.Capture);

                ulong quietPromotions = (promotionPawns >> 8) & empty;
                SerializePromotions(ref moveList, quietPromotions, -8, Flags.Quiet);

                ulong leftCapturePromotions = (promotionPawns >> 7) & enemy & Masks.NotFileH;
                SerializePromotions(ref moveList, leftCapturePromotions, -7, Flags.Capture);

                ulong rightCapturePromotions = (promotionPawns >> 9) & enemy & Masks.NotFileA;
                SerializePromotions(ref moveList, rightCapturePromotions, -9, Flags.Capture);
            }
        }

        public static void SerializeMoves(ref MoveList moveList, ulong bitboard, int displacement, int flag)
        {
            while (bitboard != 0)
            {
                int toSquare = BitOperations.TrailingZeroCount(bitboard);
                moveList.Add(new Move(toSquare - displacement, toSquare, flag));

                bitboard &= bitboard - 1;
            }
        }

        public static void SerializePromotions(ref MoveList moveList, ulong bitboard, int displacement, int captureFlag)
        {
            while (bitboard != 0)
            {
                int toSquare = BitOperations.TrailingZeroCount(bitboard);
                moveList.Add(new Move(toSquare - displacement, toSquare, Flags.KnightPromote | captureFlag));
                moveList.Add(new Move(toSquare - displacement, toSquare, Flags.BishopPromote | captureFlag));
                moveList.Add(new Move(toSquare - displacement, toSquare, Flags.RookPromote   | captureFlag));
                moveList.Add(new Move(toSquare - displacement, toSquare, Flags.QueenPromote  | captureFlag));

                bitboard &= bitboard - 1;
            }
        }
    }
}