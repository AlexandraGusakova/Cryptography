using System;
using System.Security.Cryptography;
using System.Text;

namespace Lab9
{
    class Program
    {
        static void Main()
        {
            Md5.Do();
            Console.ReadLine();
        }
    }

    public static class Md5
    {
        public static void Do()
        {
            var str = "Hello world from JUDE!";
            Console.WriteLine("Hash: ");
            Console.WriteLine(Hash(str));
        }

        private static string Hash(string s)
        {
            var bytes = Encoding.ASCII.GetBytes(s);
            var csp = new MD5CryptoServiceProvider();
            var byteHash = csp.ComputeHash(bytes);
            var hash = string.Empty;
            foreach (var b in byteHash)
            {
                hash += $"{b:x2}";
            }

            return hash;
        }
    }
}