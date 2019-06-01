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

            var k = Helpers.Computek(g, N, H);

            // kv % N
            var left = (k * v) % N;

            // g^b % N
            var right = BigInteger.ModPow(g, b, N);

            // B = kv + g^b
            B = (left + right) % N;

            return B;
        }

        public BigInteger ComputeSessionKey(BigInteger v, BigInteger u, BigInteger A)
        {
            // (Av^u)
            var left = A * BigInteger.ModPow(v, u, N) % N;
            
            // S = (Av^u) ^ b
            return BigInteger.ModPow(left, b, N);
        }

        public bool ValidateClientProof(BigInteger M1, BigInteger A, BigInteger S)
        {
            return M1 == Helpers.ComputeClientProof(N, H, A, B, S);
        }

        public BigInteger GenerateServerProof(BigInteger A, BigInteger M1, BigInteger S)
        {
            return Helpers.ComputeServerProof(N, H, A, M1, S);
        }
    }
}