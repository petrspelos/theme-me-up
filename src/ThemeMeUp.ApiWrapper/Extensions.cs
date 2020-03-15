namespace ThemeMeUp.ApiWrapper
{
    public static class Extensions
    {
        public static bool IsBitSet(this byte b, int pos) => (b & (1 << pos)) != 0;
        public static string ToBinaryString(this bool value) => value ? "1" : "0";
    }
}
