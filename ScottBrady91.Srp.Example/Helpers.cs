using System;
using System.Globalization;
using System.Numerics;

namespace ScottBrady91.Srp.Example
{
    public static class Helpers
    {
        public static byte[] ToBytes(this string hex)
        {
            var hexAsBytes = new byte[hex.Length / 2];

            for (var i = 0; i < hex.Length; i += 2)
            {
                hexAsBytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return hexAsBytes;
        }

        // both unsigned and big endian
        public static BigInteger ToSrpBigInt(this byte[] bytes)
        {
            return new BigInteger(bytes, true, true);
        }

        // Add padding character back to hex before parsing
        public static BigInteger ToSrpBigInt(this string hex)
        {
            return BigInteger.Parse("0" + hex, NumberStyles.HexNumber);
        }
    }
}