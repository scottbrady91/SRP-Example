using System;
using System.Linq;
using System.Numerics;
// ReSharper disable InconsistentNaming

namespace ScottBrady91.Srp.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new SrpClient(TestVectors.H, TestVectors.g, TestVectors.N);
            var server = new SrpServer(TestVectors.H, TestVectors.g, TestVectors.N);

            // generate password verifier to store 
            BigInteger v = client.GenerateVerifier(TestVectors.I, TestVectors.P, TestVectors.s);
            if (v != TestVectors.expected_v) throw new Exception();

            var A = client.GenerateAValues();
            if (A != TestVectors.expected_A) throw new Exception();

            var B = server.GenerateBValues(v);
            if (B != TestVectors.expected_B) throw new Exception();

            var u = TestVectors.H(A.ToByteArray(true, true).Concat(B.ToByteArray(true, true)).ToArray()).ToSrpBigInt();

            var clientS = client.ComputeSessionKey(TestVectors.I, TestVectors.P, TestVectors.s, u, B);
            var serverS = server.ComputeSessionKey(v, u, A);
            if (clientS != serverS || clientS != TestVectors.expected_S) throw new Exception();


        }
    }
}
