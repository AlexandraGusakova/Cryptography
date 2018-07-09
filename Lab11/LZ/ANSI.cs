/*

	This is a simple implementation of the well-known LZW algorithm. 
    Copyright (C) 2011  Stamen Petrov <stamen.petrov@gmail.com>

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA

*/


using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Lab11.Lz;

namespace Lab11.LZ
{
    public class Ansi
    {
        private readonly Dictionary<string, int> table = new Dictionary<string, int>();
        public Dictionary<string, int> Table => this.table;

        public Ansi()
        {
            for (int i = 0; i < 256; i++)
            {
                this.table.Add(Encoding.Default.GetString(new byte[1] { Convert.ToByte(i) }), i);
            }
        }

        public void WriteLine()
        {
            this.table.WriteLine();
        }

        public void WriteToFile()
        {
            File.WriteAllText("ANSI.txt", this.table.ToStringLines(), Encoding.Default);
        }
    }
}
