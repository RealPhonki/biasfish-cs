namespace Biasfish.Core
{
    public class Debug
    {
        public static readonly char[] PieceSymbols =
        {
            ' ', '♟', '♞', '♝', '♜', '♛', '♚', ' ', ' ', '♙', '♘', '♗', '♖', '♕', '♔'
        };

        public static readonly string[] FlagNames =
        {
            "Quiet",
            "Double Pawn Push",
            "O-O",
            "O-O-O",
            "Capture",
            "En-Passant",
            "Invalid",
            "Invalid",
            "Knight Promote",
            "Bishop Promote",
            "Rook Promote",
            "Queen Promote",
            "Knight Promote Capture",
            "Bishop Promote Capture",
            "Rook Promote Capture",
            "Queen Promote Capture"
        };

        public static readonly string[] SquareNames =
        {
            "A1", "B1", "C1", "D1", "E1", "F1", "G1", "H1", 
            "A2", "B2", "C2", "D2", "E2", "F2", "G2", "H2", 
            "A3", "B3", "C3", "D3", "E3", "F3", "G3", "H3", 
            "A4", "B4", "C4", "D4", "E4", "F4", "G4", "H4", 
            "A5", "B5", "C5", "D5", "E5", "F5", "G5", "H5", 
            "A6", "B6", "C6", "D6", "E6", "F6", "G6", "H6", 
            "A7", "B7", "C7", "D7", "E7", "F7", "G7", "H7", 
            "A8", "B8", "C8", "D8", "E8", "F8", "G8", "H8"
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