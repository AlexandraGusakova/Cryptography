using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    class Program
    {
        static string Cesare(int k, string crypt)
        {
            string result = string.Empty;
            string alphabit = "abcdefghijklmnopqrstuvwxyz";
            string low = crypt.ToLower();
            int val = k % alphabit.Length;


            for(int i=0; i<low.Length; i++)
            {
                char letter = low[i];
                int el = alphabit.IndexOf(letter);
                el += val;
                el = el % alphabit.Length;
                result += alphabit.ElementAt(el);  
            }
            return result;
        }

        static string Vigenere(string key, string crypt)
        {
            string result = string.Empty;
            string low = crypt.ToLower();
            string alphabit = "abcdefghijklmnopqrstuvwxyz";
            int keyindex = 0;
            foreach (char symbol in low)
            {
                if (symbol == ' ')
                {

                }
                else
                {
                    int c = (alphabit.IndexOf(symbol) + alphabit.IndexOf(key[keyindex])) % alphabit.Length;
                    result += alphabit[c];
                    keyindex++;
                    if ((keyindex) == key.Length)
                        keyindex = 0;
                }
            }
            return result;
        }

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        static string OwnCrypt(int key, string crypt)
        {
            string result = string.Empty;
            string alphabit = "abcdefghijklmnopqrstuvwxyz";
            int val = key % alphabit.Length;

            string newalphabit = Reverse(alphabit);
            string low = crypt.ToLower();

            for (int i = 0; i < low.Length; i++)
            {
                char letter = low[i];
                int el = newalphabit.IndexOf(letter);
                el += val;
                el = el % alphabit.Length;
                result += newalphabit.ElementAt(el);
            }
            
            return result;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Result of Crypt Cesare = " + Cesare(50, "Gusakova Alexandra Nikolaevna"));
            Console.WriteLine("Result of Crypt Vigenere = " + Vigenere("Alex", "Gusakova Alexandra Nikolaevna"));
            Console.WriteLine("Result of OWN Reshuffle Crypt = " + OwnCrypt(9, "Gusakova Alexandra Nikolaevna"));
        }
    }
}
