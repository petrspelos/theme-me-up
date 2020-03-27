using System;

namespace ThemeMeUp.ApiWrapper
{
    public static class Extensions
    {
        public static bool IsBitSet(this byte b, int pos) => (b & (1 << pos)) != 0;
        public static string ToBinaryString(this bool value) => value ? "1" : "0";
        public static ulong NextULong(this Random self, ulong min, ulong max)
        {
            var buf = new byte[sizeof(ulong)];
            self.NextBytes(buf);
            ulong n = BitConverter.ToUInt64(buf, 0);
            double normalised = n / (ulong.MaxValue + 1.0);
            double range = (double)max - min;
            return (ulong)(normalised * range) + min;
        }
    }
}
