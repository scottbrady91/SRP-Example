using System;
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

        public static BigInteger ToUnsignedBigInt(this byte[] bytes)
        {
            return new BigInteger(bytes, true, true);
        }
    }
}