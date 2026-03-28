namespace Biasfish.Core
{
    public class Debug
    {
        public static readonly char[] PieceSymbols = {
            'P', 'N', 'B', 'R', 'Q', 'K', 'p', 'n', 'b', 'r', 'q', 'k'
        };

        public static void PrintBoard(Board board)
        {
            Console.WriteLine("  +---+---+---+---+---+---+---+---+");
            for (int rank = 7; rank >= 0; rank--)
            {
                Console.Write($"{rank + 1} | ");
                for (int file = 7; file >= 0; file--)
                {
                    int square = rank * 8 + file;

                    bool pieceFound = false;
                    foreach (int pieceType in Piece.PieceTypes)
                    {
                        ulong bitboard = board.GetBitboard(pieceType);

                        if ((bitboard & (1UL << square)) != 0)
                        {
                            Console.Write($"{PieceSymbols[pieceType]} | ");
                            pieceFound = true;
                            break;
                        }
                    }

                    if (!pieceFound)
                    {
                        Console.Write(". | ");
                    }
                }
                Console.WriteLine("\n  +---+---+---+---+---+---+---+---+");
            }
            Console.WriteLine("    A   B   C   D   E   F   G   H    ");
        }

        public static void PrintBB(ulong bitboard)
        {
            Console.WriteLine();
            for (int rank = 7; rank >= 0; rank--)
            {
                Console.Write($"{rank} | ");
                for (int file = 7; rank >= 0; rank--)
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