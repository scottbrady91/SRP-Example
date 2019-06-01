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
            var x = GeneratePrivateKey(I, P, s);

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

        public BigInteger ComputeSessionKey(string I, string P, byte[] s, BigInteger u, BigInteger B)
        {
            var x = GeneratePrivateKey(I, P, s);
            var k = Helpers.Computek(g, N, H);

            // (a + ux)
            var exp = a + u * x;

            // (B - kg ^ x)
            var val = mod(B - (BigInteger.ModPow(g, x, N) * k % N), N);

            // S = (B - kg ^ x) ^ (a + ux)
            return BigInteger.ModPow(val, exp, N);
        }

        public BigInteger GenerateClientProof(BigInteger B, BigInteger S)
        {
            return Helpers.ComputeClientProof(N, H, A, B, S);
        }

        public bool ValidateServerProof(BigInteger M2, BigInteger M1, BigInteger S)
        {
            return M2 == Helpers.ComputeServerProof(N, H, A, M1, S);
        }

        private BigInteger GeneratePrivateKey(string I, string P, byte[] s)
        {
            // x = H(s | H(I | ":" | P))
            var x = H(s.Concat(H(Encoding.UTF8.GetBytes(I + ":" + P))).ToArray());

            return x.ToSrpBigInt();
        }

        private static BigInteger mod(BigInteger x, BigInteger m)
        {
            return (x % m + m) % m;
        }
    }
}