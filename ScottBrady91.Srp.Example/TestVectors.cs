// ReSharper disable InconsistentNaming

using System.Globalization;
using System.Numerics;

namespace ScottBrady91.Srp.Example
{
    public static class TestVectors
    {
        public const string I = "alice"; // I - user's username
        public const string P = "password123"; // P - user's password
        public static byte[] s = "BEB25379D1A8581EB5A727673A2441EE".ToBytes(); // s - user's salt (from server)

        public static int g = 2;
        public static BigInteger N = 
            BigInteger.Parse("0" + "EEAF0AB9ADB38DD69C33F80AFA8FC5E86072618775FF3C0B9EA2314C9C256576D674DF7496EA81D3383B4813D692C6E0E0D5D8E250B98BE48E495C1D6089DAD15DC7D7B46154D6B6CE8EF4AD69B15D4982559B297BCF1885C529F566660E57EC68EDBC3C05726CC02FD4CBF4976EAA9AFD5138FE8376435B9FC61D2FC0EB06E3", 
                NumberStyles.HexNumber);
    }
}