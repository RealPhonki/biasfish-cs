namespace Biasfish.Core
{
    public enum File : int
    {
        FileA, FileB, FileC, FileD, FileE, FileF, FileG, FileH, FileNB,
    }

    public static class FileExtensions
    {
        private const string FileToChar = "abcdefgh";

        extension(File)
        {
            public static File FromChar(char character) => (File)FileToChar.IndexOf(character);
        }
    }
}