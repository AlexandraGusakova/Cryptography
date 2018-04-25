using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace Lab_2
{
    class Program
    {
        static void Main(string[] args)
        {
            string arr_str = null;
            string name;
            double entr = 0, entrname = 0, ver = 0, verp = 0, hart = 0;

            using (StreamReader rfile = new StreamReader("text.txt"))
            {
                arr_str = rfile.ReadToEnd().ToLower(); 
            }
            using (StreamReader nfile = new StreamReader("name.txt"))
            {
                name = nfile.ReadLine();
            }
            var count = from p in arr_str
                        where (p >= 'A') && (p <= 'z') || (p == 'ą' || p == 'ć' || p == 'ś' || p == 'ł' || p == 'ż' || p == 'ę' || p == 'ó' || p == 'ż')
                        group p by p
                        into charGroup
                        orderby charGroup.Key ascending
                        select charGroup;

            foreach (var el in count)
            {
               
                ver = Convert.ToDouble(el.Count()) / arr_str.Length;
                entr += -(ver) * Math.Log(ver, 2);
                Console.WriteLine("{0} - {1} - {2:f2}% ", el.Key, el.Count(), ver*100);
            }
            hart = Math.Log(32, 2);
            entrname = entr * name.Length;
            Console.WriteLine("Всего символов:\n {0}", arr_str.Length);
            Console.WriteLine("Энтропия Шеннона:\n {0:f2}", entr);
            Console.WriteLine("Энтропия Хартли:\n {0:f2}", hart);
            Console.WriteLine("ФИО:\n {0}", name);
            Console.WriteLine("Информационная энтропия ФИО:\n {0:f2}", entrname);
            Console.ReadLine();

        }
    }
}
