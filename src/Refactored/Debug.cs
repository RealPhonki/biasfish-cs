namespace Biasfish.V2
{
    public class Debug
    {
        public static readonly char[] PieceSymbols =
        {
            ' ', ' ', '♟', '♞', '♝', '♜', '♛', '♚',
            ' ', ' ', '♙', '♘', '♗', '♖', '♕', '♔'
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
                    int encoding = board.EncodingAt(square);

                    if (encoding != Piece.Null)
                    {
                        Console.Write(PieceSymbols[encoding] + " | ");
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