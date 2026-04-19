namespace Biasfish.V2
{
    public enum Color
    {
        White = 0,
        Black = 1,
    }

    public static class ColorExtensions
    {
        public static Color Opposite(this Color color)
        {
            return (Color)((int)color ^ 1);
        }
    }
}