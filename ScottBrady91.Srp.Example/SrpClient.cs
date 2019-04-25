using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

// ReSharper disable InconsistentNaming
namespace ScottBrady91.Srp.Example
{
    public class SrpClient
    {
        public BigInteger GenerateVerifier(string username, string password, byte[] salt)
        {
            var sha1 = SHA1.Create();

            var I = username;
            var P = password;
            var s = salt;

            // x = H(s | H(I | ":" | P))
            var x = sha1.ComputeHash(s.Concat(sha1.ComputeHash(Encoding.UTF8.GetBytes(I + ":" + P))).ToArray());

            // v = g^x
            var v = BigInteger.ModPow(g, x.ToUnsignedBigInt(), N.ToUnsignedBigInt());

            return v;
        }

        private byte[] GetTestSalt() => "BEB25379D1A8581EB5A727673A2441EE".ToBytes();


        private int g = 2;

        private byte[] N =
            "EEAF0AB9ADB38DD69C33F80AFA8FC5E86072618775FF3C0B9EA2314C9C256576D674DF7496EA81D3383B4813D692C6E0E0D5D8E250B98BE48E495C1D6089DAD15DC7D7B46154D6B6CE8EF4AD69B15D4982559B297BCF1885C529F566660E57EC68EDBC3C05726CC02FD4CBF4976EAA9AFD5138FE8376435B9FC61D2FC0EB06E3"
                .ToBytes();
    }
}