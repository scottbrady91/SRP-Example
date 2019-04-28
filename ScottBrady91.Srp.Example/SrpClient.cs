using System;
using System.Linq;
using System.Numerics;
using System.Text;

// ReSharper disable InconsistentNaming
namespace ScottBrady91.Srp.Example
{
    public class SrpClient
    {
        private readonly Func<byte[], byte[]> H;
        private readonly int g;
        private readonly BigInteger N;

        private BigInteger A;
        private BigInteger a;

        public SrpClient(Func<byte[], byte[]> H, int g, BigInteger N)
        {
            this.H = H;
            this.g = g;
            this.N = N;
        }

        public BigInteger GenerateVerifier(string I, string P, byte[] s)
        {
            // x = H(s | H(I | ":" | P))
            var xBytes = H(s.Concat(H(Encoding.UTF8.GetBytes(I + ":" + P))).ToArray());
            var x = xBytes.ToSrpBigInt();

            // v = g^x
            var v = BigInteger.ModPow(g, x, N);

            return v;
        }

        public BigInteger GenerateAValues()
        {
            // a = random()
            a = TestVectors.a;

            // A = g^a
            A = BigInteger.ModPow(g, a, TestVectors.N);

            return A;
        }
    }
}