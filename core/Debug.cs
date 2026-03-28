namespace Biasfish.Core
{
    public class Debug
    {
        public static readonly char[] PieceSymbols = {
            '♟', '♞', '♝', '♜', '♛', '♚', '♙', '♘', '♗', '♖', '♕', '♔'
        };

        public static void PrintBoard(Board board)
        {
            Console.WriteLine("  +---+---+---+---+---+---+---+---+");
            for (int rank = 7; rank >= 0; rank--)
            {
                Console.Write($"{rank + 1} | ");
                for (int file = 0; file < 8; file++)
                {
                    int square = rank * 8 + file;

                    bool pieceFound = false;
                    foreach (int pieceType in Piece.PieceTypes)
                    {
                        ulong bitboard = board.Get(pieceType);

                        if ((bitboard & (1UL << square)) != 0)
                        {
                            Console.Write($"{PieceSymbols[pieceType]} | ");
                            pieceFound = true;
                            break;
                        }
                    }

                    if (!pieceFound)
                    {
                        Console.Write("  | ");
                    }
                }
                Console.WriteLine("\n  +---+---+---+---+---+---+---+---+");
            }
            Console.WriteLine("    a   b   c   d   e   f   g   h   ");
        }

        public static void PrintBB(ulong bitboard)
        {
            Console.WriteLine();
            for (int rank = 7; rank >= 0; rank--)
            {
                Console.Write($"{rank} | ");
                for (int file = 0; file < 8; file++)
                {
                    int square = rank * 8 + file;

                    if ((bitboard & (1UL << square)) != 0)
                    {
                        Console.Write("1 | ");
                    }
                    else
                    {
                        Console.Write(". | ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}