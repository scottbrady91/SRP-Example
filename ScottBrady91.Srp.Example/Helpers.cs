using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;

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
            var NBytes = N.ToByteArray(true, true);
            var gBytes = PadBytes(BitConverter.GetBytes(g).Reverse().ToArray(), NBytes.Length);

            var k = H(NBytes.Concat(gBytes).ToArray());

            return new BigInteger(k, isBigEndian: true);
        }

        public static byte[] ComputeClientProof(
            BigInteger N,
            Func<byte[], byte[]> H,
            BigInteger A,
            BigInteger B,
            byte[] S)
        {
            var NBytes = N.ToByteArray(true, true);

            // M1 = H( A | B | S )
            return H((PadBytes(A.ToByteArray(true, true), NBytes.Length))
                .Concat(PadBytes(B.ToByteArray(true, true), NBytes.Length))
                .Concat(PadBytes(S, NBytes.Length))
                .ToArray());
        }

        public static byte[] ComputeServerProof(BigInteger N, Func<byte[], byte[]> H, BigInteger A, byte[] M1, byte[] S)
        {
            var NBytes = N.ToByteArray(true, true);

            // M2 = H( A | M1 | S )
            return H((PadBytes(A.ToByteArray(true, true), NBytes.Length))
                .Concat(PadBytes(M1, NBytes.Length))
                .Concat(PadBytes(S, NBytes.Length))
                .ToArray());
        }

        public static byte[] PadBytes(byte[] bytes, int length)
        {
            var paddedBytes = new byte[length];
            Array.Copy(bytes, 0, paddedBytes, length - bytes.Length, bytes.Length);

            return paddedBytes;
        }
    }
}