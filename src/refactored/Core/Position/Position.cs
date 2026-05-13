using System.Diagnostics;
using System.Runtime.CompilerServices;
using Bitboard = ulong;

namespace Biasfish.Core
{
    public unsafe partial struct Position
    {
        private fixed Bitboard ByTypeBB[(int)PieceType.PieceTypeNB];
        private fixed Bitboard ByColorBB[(int)Color.ColorNB];
        private fixed byte board[(int)Square.SquareNB];
        private Color sideToMove;
        private int castlingRights;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Bitboard Pieces()
        {
            return ByColorBB[(int)Color.White] | ByColorBB[(int)Color.Black];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Bitboard Pieces(Color color)
        {
            return ByColorBB[(int)color];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Bitboard Pieces(PieceType pieceType)
        {
            return ByTypeBB[(int)pieceType];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Bitboard Pieces(PieceType pieceType, Color color)
        {
            return Pieces(pieceType) & Pieces(color);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Bitboard Pieces(Piece piece)
        {
            return Pieces(piece.Type(), piece.Color());
        }


        public Position()
        {
            Set("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        }

        public Position(string fenString)
        {
            Set(fenString);
        }

        public override string ToString()
        {
            string output = "";
            output += "  +---+---+---+---+---+---+---+---+\n";
            for (int rank = 7; rank >= 0; rank--)
            {
                output += $"{rank + 1} | ";
                for (int file = 0; file < 8; file++)
                {
                    int square = rank * 8 + file;
                    Piece piece = (Piece)board[square];

                    Debug.Assert(piece >= 0 && piece <= Piece.PieceNB, $"Illegal piece '{piece}'");

                    output += $"{Piece.ToSymbol(piece)} | ";
                }
                output += "\n  +---+---+---+---+---+---+---+---+\n";
            }
            output += "    a   b   c   d   e   f   g   h   \n";

            return output;
        }
    }
}