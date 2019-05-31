using System;
using System.Linq;
using System.Numerics;

// ReSharper disable InconsistentNaming

namespace ScottBrady91.Srp.Example
{
    public class Program
    {
        public static void Main(string[] args)
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

            var M1 = Helpers.ComputeClientProof(
                TestVectors.N,
                TestVectors.H,
                A,
                B,
                clientS.ToByteArray(true, true));

            var M2 = Helpers.ComputeServerProof(
                TestVectors.N, 
                TestVectors.H, 
                A, 
                M1, 
                clientS.ToByteArray(true, true));

            Console.WriteLine("M1: " + M1.ToSrpBigInt());
            Console.WriteLine("M2: " + M2.ToSrpBigInt());
        }
    }
}
