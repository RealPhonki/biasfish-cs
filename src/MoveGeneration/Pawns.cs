using System.Numerics;

namespace Biasfish.Core
{
    public static class Pawns
    {
        public static readonly int[] PawnPushes = new int[64];

        public static void GetPseudoLegal(ref Board board, ref MoveList moveList)
        {
            ulong pawns = board.Get(Piece.Pawns | board.sideToMove);
            ulong empty = ~board.Get(Piece.Any);
            ulong enemy = board.Get(Piece.FlipColor(board.sideToMove));

            if (board.sideToMove == Piece.White)
            {
                ulong promotionPawns = pawns & Masks.Rank7;
                pawns &= Masks.NotRank7;

                ulong singlePushes = (pawns << 8) & empty;
                SerializeMoves(ref moveList, singlePushes, 8, Flag.Quiet);

                ulong doublePushes = ((singlePushes & Masks.Rank3) << 8) & empty;
                SerializeMoves(ref moveList, doublePushes, 16, Flag.DoublePawnPush);

                ulong leftCaptures = (pawns << 7) & enemy & Masks.NotFileH;
                SerializeMoves(ref moveList, leftCaptures, 7, Flag.Capture);

                ulong rightCaptures = (pawns << 9) & enemy & Masks.NotFileA;
                SerializeMoves(ref moveList, rightCaptures, 9, Flag.Capture);

                ulong quietPromotions = (promotionPawns << 8) & empty;
                SerializePromotions(ref moveList, quietPromotions, 8, Flag.Quiet);

                ulong leftCapturePromotions = (promotionPawns << 7) & enemy;
                SerializePromotions(ref moveList, leftCapturePromotions, 7, Flag.Capture);

                ulong rightCapturePromotions = (promotionPawns << 9) & enemy;
                SerializePromotions(ref moveList, rightCapturePromotions, 9, Flag.Capture);
            }
            else
            {
                ulong promotionPawns = pawns & Masks.Rank2;
                pawns &= Masks.NotRank2;

                ulong singlePushes = (pawns >> 8) & empty;
                SerializeMoves(ref moveList, singlePushes,  -8, Flag.Quiet);

                ulong doublePushes = ((singlePushes & Masks.Rank6) >> 8) & empty;
                SerializeMoves(ref moveList, doublePushes, -16, Flag.DoublePawnPush);

                ulong leftCaptures = (pawns >> 7) & enemy & Masks.NotFileH;
                SerializeMoves(ref moveList, leftCaptures, -7, Flag.Capture);

                ulong rightCaptures = (pawns >> 9) & enemy & Masks.NotFileA;
                SerializeMoves(ref moveList, rightCaptures, -9, Flag.Capture);

                ulong quietPromotions = (promotionPawns >> 8) & empty;
                SerializePromotions(ref moveList, quietPromotions, -8, Flag.Quiet);

                ulong leftCapturePromotions = (promotionPawns >> 7) & enemy;
                SerializePromotions(ref moveList, leftCapturePromotions, -7, Flag.Capture);

                ulong rightCapturePromotions = (promotionPawns >> 9) & enemy;
                SerializePromotions(ref moveList, rightCapturePromotions, -9, Flag.Capture);
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
                moveList.Add(new Move(toSquare - displacement, toSquare, Flag.KnightPromote | captureFlag));
                moveList.Add(new Move(toSquare - displacement, toSquare, Flag.BishopPromote | captureFlag));
                moveList.Add(new Move(toSquare - displacement, toSquare, Flag.RookPromote   | captureFlag));
                moveList.Add(new Move(toSquare - displacement, toSquare, Flag.QueenPromote  | captureFlag));

                bitboard &= bitboard - 1;
            }
        }
    }
}