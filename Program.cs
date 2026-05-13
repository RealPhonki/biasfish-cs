using Biasfish.Core;

namespace Biasfish
{
    class Program
    {
        static void Main(string[] args)
        {
            Position position = new Position("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            Console.WriteLine(position);
        }
    }
}