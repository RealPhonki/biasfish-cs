namespace Biasfish.V2
{
    public class Debug
    {
        public static readonly char[] WhiteSymbols = {' ', ' ', '♟', '♞', '♝', '♜', '♛', '♚'};
        public static readonly char[] BlackSymbols = {' ', ' ', '♙', '♘', '♗', '♖', '♕', '♔'};

        public static void PrintBoard(Board board)
        {
            Console.WriteLine("  +---+---+---+---+---+---+---+---+");
            int square;
            int piece;

            for (int rank = 7; rank >= 0; rank--)
            {
                Console.Write($"{rank + 1} | ");
                for (int file = 0; file < 8; file++)
                {
                    square = rank * 8 + file;  
                    piece = board.PieceAt(square);

                    if ((board.Get(Piece.White) & (1UL << square)) != 0)
                    {
                        Console.Write($"{WhiteSymbols[piece]} | ");
                    }
                    else if ((board.Get(Piece.Black) & (1UL << square)) != 0)
                    {
                        Console.Write($"{BlackSymbols[piece]} | ");
                    }
                    else
                    {
                        Console.Write($"  | ");
                    }
                }
                Console.WriteLine("\n  +---+---+---+---+---+---+---+---+");
            }
            Console.WriteLine("    a   b   c   d   e   f   g   h   ");
        }

        public static void PrintBB(ulong bitboard)
        {
            Console.WriteLine("  +---+---+---+---+---+---+---+---+");
            for (int rank = 7; rank >= 0; rank--)
            {
                Console.Write($"{rank + 1} | ");
                for (int file = 0; file < 8; file++)
                {
                    int square = rank * 8 + file;

                    if ((bitboard & (1UL << square)) != 0)
                    {
                        Console.Write("1 | ");
                    }
                    else
                    {
                        Console.Write("  | ");
                    }
                }
                Console.WriteLine("\n  +---+---+---+---+---+---+---+---+");
            }
            Console.WriteLine("    a   b   c   d   e   f   g   h   ");
        }
    }
}