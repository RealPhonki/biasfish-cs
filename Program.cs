using Biasfish.Core;

namespace Biasfish
{
    class Program
    {
        static void Main(string[] args)
        {
            Position position = new Position("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            position.MovePiece(Piece.WhitePawn, Square.E2, Square.E4);
            position.MovePiece(Piece.BlackPawn, Square.E7, Square.E5);
            Console.WriteLine(position);
        }
    }
}