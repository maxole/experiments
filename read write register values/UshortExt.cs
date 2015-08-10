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
    }
}