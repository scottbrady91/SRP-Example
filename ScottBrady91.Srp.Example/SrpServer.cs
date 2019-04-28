using System;
using System.Linq;
using System.Numerics;
// ReSharper disable InconsistentNaming

namespace ScottBrady91.Srp.Example
{
    public class SrpServer
    {
        private readonly Func<byte[], byte[]> H;
        private readonly int g;
        private readonly BigInteger N;

        private BigInteger B;
        private BigInteger b;

        public SrpServer(Func<byte[], byte[]> H, int g, BigInteger N)
        {
            this.H = H;
            this.g = g;
            this.N = N;
        }

        public BigInteger GenerateBValues(BigInteger v)
        {
            // b = random()
            b = TestVectors.b;

            // k = H(N, g)
            var gBytes = BitConverter.GetBytes(g).Reverse().ToArray();
            var NBytes = N.ToByteArray(true, true);

            var paddedG = new byte[NBytes.Length];
            Array.Copy(gBytes, 0, paddedG, NBytes.Length - gBytes.Length, gBytes.Length);

            var k = H(NBytes.Concat(paddedG).ToArray());
            if (!k.SequenceEqual("7556AA045AEF2CDD07ABAF0F665C3E818913186F".ToBytes())) throw new Exception();

            // B = kv + g^b

            // kv % N
            var left = (new BigInteger(k, isBigEndian: true) * v) % N;

            // g^b % N
            var right = BigInteger.ModPow(g, b, N);

            // (left + right) % N
            B = (right + left) % N;

            return B;
        }
    }
}