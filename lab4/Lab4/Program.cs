using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    class Program
    {
        public static string ShifrCrypt(string input)
        {
            string result = string.Empty;
            string doubstring = string.Empty;
            string nodoubstring = string.Empty;

            string lower = input.ToLower();
            for(int i=0; i<lower.Length; i++)
            {
                if(i % 2 == 0)
                {
                    doubstring += lower[i];
                }
                else
                {
                    nodoubstring += lower[i];
                }
            }

            result = doubstring + nodoubstring;
            return result;
        }

        public static string ShifrEncrypt(string input)
        {
            string result = string.Empty;
            double val = Convert.ToInt64(input.Length / 2.0);
            int scaled = (int)((double)Math.Ceiling(input.Length / 2.0));
            string fistmas = string.Empty;
            string secondmas = string.Empty;

            for (int i=0; i<scaled; i++)
            {
                fistmas += input[i];
            }

            for(int i = scaled; i<input.Length; i++)
            {
                secondmas += input[i];
            }

            if(fistmas.Length > secondmas.Length)
            {
                secondmas += '!';
            }

            for(int i=0; i<fistmas.Length; i++)
            {
                result += fistmas[i];
                result += secondmas[i];
            }
           
            if(result.Contains('!'))
            {
                result.Remove(result.Length - 1, 1);
            }

            return result;
        }

        static void Main(string[] args)
        {
            string coded = ShifrCrypt("Panasik Gleb Alexandrovich");
            Console.WriteLine(coded);
            Console.WriteLine(ShifrEncrypt(coded));
        }
    }
}
