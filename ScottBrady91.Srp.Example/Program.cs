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

            var clientS = client.ComputeSessionKey(TestVectors.I, TestVectors.P, TestVectors.s, B);
            var serverS = server.ComputeSessionKey(v, A);
            if (clientS != serverS || clientS != TestVectors.expected_S) throw new Exception();

            var M1 = client.GenerateClientProof(B, clientS);
            if (!server.ValidateClientProof(M1, A, serverS)) throw new Exception();

            var M2 = server.GenerateServerProof(A, M1, serverS);
            if (!client.ValidateServerProof(M2, M1, clientS)) throw new Exception();

            Console.WriteLine("SRP success!");
        }
    }
}
