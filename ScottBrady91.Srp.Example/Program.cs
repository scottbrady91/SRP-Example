using System;

namespace ScottBrady91.Srp.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            const string username = "alice"; // I - user's username
            const string password = "password123"; // P - user's password
            const string saltHex = "BEB25379D1A8581EB5A727673A2441EE"; // s - user's salt (from server)

            var client = new SrpClient();
            var verifier = client.GenerateVerifier(username, password, saltHex.ToBytes());
        }

        
    }
}
