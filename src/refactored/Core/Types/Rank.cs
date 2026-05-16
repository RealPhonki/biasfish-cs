namespace Biasfish.Core
{
    public enum Rank : int
    {
        Rank1, Rank2, Rank3, Rank4, Rank5, Rank6, Rank7, Rank8, RankNB,
    }

    public static class RankExtensions
    {
        private const string RankToChar = "12345678";

        extension(Rank)
        {
            public static Rank FromChar(char character) => (Rank)RankToChar.IndexOf(character);
        }
    }
}