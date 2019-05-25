using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
// ReSharper disable InconsistentNaming

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

        public static BigInteger Computek(int g, BigInteger N ,Func<byte[], byte[]> H)
        {
            // k = H(N, g)
            var gBytes = BitConverter.GetBytes(g).Reverse().ToArray();
            var NBytes = N.ToByteArray(true, true);

            var paddedG = new byte[NBytes.Length];
            Array.Copy(gBytes, 0, paddedG, NBytes.Length - gBytes.Length, gBytes.Length);

            var k = H(NBytes.Concat(paddedG).ToArray());

            return new BigInteger(k, isBigEndian: true);
        }
    }
}