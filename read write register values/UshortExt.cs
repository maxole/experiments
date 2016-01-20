using System;
using System.Globalization;
using System.Linq;

namespace Sparc.Kpk12.Certification.Domain
{
    public static class UshortExt
    {
        public static bool Setted(this ushort value, params byte[] bit)
        {
            return bit.All(b => (value & (1 << b)) != 0);
        }

        public static bool Resetted(this ushort value, params byte[] bit)
        {
            return bit.All(b => (value & (1 << b)) == 0);
        }

        public static ushort Set(this ushort value, byte bit)
        {
            var mask = 1 << bit;
            value |= (ushort) mask;
            return value;
        }

        public static ushort Reset(this ushort value, byte bit)
        {
            var mask = 1 << bit;
            value &= (ushort) ~mask;
            return value;
        }

        public static ushort ToUshort(this string value, MaskType mask)
        {
            if (string.IsNullOrEmpty(value))
                return 0;
            return Convert.ToUInt16(value, mask == MaskType.Hex ? 16 : 10);
        }

        public static string ToHex(this ushort value)
        {
            return value.ToString("X");
        }

        public static string ToString(this ushort value, MaskType mask)
        {
            return mask == MaskType.Hex ? value.ToString("X") : Convert.ToString(value);
        }
		
		        public static string GroupingBin(this string value, int size = 4, string separator = " ")
        {
            var v = Regex.Replace(value, ".{" + size + "}", "$0" + separator);
            return v.Remove(v.Length - 1);
        }

        public static ushort FromValue(this float value, float[] range)
        {     
            ushort i = 0;
            var tuple = Enumerable.Range(0, range.Length).Reverse()
                .Aggregate(new Tuple<float, ushort>(value, i), (f, f1) => (range[f1] > f.Item1) ? f : new Tuple<float, ushort>(f.Item1 - range[f1], i |= (ushort) (1 << f1)));
            return tuple.Item2;
        }

        public static float ExtractCode(this ushort code, float[] range)
        {     
            return Enumerable.Range(0, range.Length)
                .Aggregate(0f, (d, i) => d + ((code & (1 << i)) == 0 ? range[i] : 0f));
        }
    }
}