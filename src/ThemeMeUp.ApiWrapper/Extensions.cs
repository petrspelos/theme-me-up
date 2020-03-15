namespace ThemeMeUp.ApiWrapper
{
    public static class Extensions
    {
        public static bool IsBitSet(this byte b, int pos) => (b & (1 << pos)) != 0;
    }
}
