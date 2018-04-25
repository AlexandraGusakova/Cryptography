using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csezar
{
    class Program
    {
     public static  string alphabit = " абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        static int  key = 1;
        static void Main(string[] args)
        {
            //зашифрование
            Console.Write("Введите сообщение для шифрования: ");
            string message1 = Console.ReadLine();

            Console.Write("Введите ключ: ");
             key = Convert.ToInt32(Console.ReadLine());


             string encryptMessage1 = Encrypt(message1.ToLower(), key);

            Console.WriteLine("Зашифрованное сообщение: " + encryptMessage1);


            //расшифрование
            Console.Write("Введите сообщение для расшифрования: ");
            string message2 = Console.ReadLine();

            Console.Write("Введите ключ: ");
            key = Convert.ToInt32(Console.ReadLine());


            string encryptMessage2 = Encrypt2(message2.ToLower(), key); //расшифрованное сообщение

            Console.WriteLine("Зашифрованное сообщение: " + encryptMessage2);
         
        }
        public static string Encrypt(string message, int key) //шифрование
        {
            string encryptMessage = "";
            foreach (var item in message)
            {
                int newIndex = (alphabit.IndexOf(item) + key) % 34;
                char newSymbol = alphabit[newIndex];
                encryptMessage += newSymbol;
            }
            return encryptMessage;
        }

        public static string Encrypt2(string message, int key) //расшифрование
        {
            string encryptMessage = "";
            foreach (var item in message)
            {
                int newIndex = (alphabit.IndexOf(item) - key + 34) % 34;
                char newSymbol = alphabit[newIndex];
                encryptMessage += newSymbol;
            }
            return encryptMessage;
        }
      
    }
	 system('pause');
}
