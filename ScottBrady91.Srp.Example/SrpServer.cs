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

        public BigInteger ValidateClientProof(string I, byte[] s, BigInteger B, BigInteger S)
        {
            // TODO
            throw new NotImplementedException();
        }

        public BigInteger GenerateServerProof()
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}