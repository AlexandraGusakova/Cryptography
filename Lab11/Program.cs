using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Lab11.LZ;

namespace Lab11
{
    class Program
    {
        static void Main()
        {
            LzClass.Do();
            Shf.Do();
            Haffman.Do();
            Console.ReadLine();
        }
    }

    public class LzClass
    {
        static string _fileToCompress = "E:\\Study\\3-2\\Криптографические основы защиты информации\\Лабы\\Crypto_9_13\\lz.txt";
        static string _encodedFile = "E:\\Study\\3-2\\Криптографические основы защиты информации\\Лабы\\Crypto_9_13\\TestOutput.txt";
        static string _decodedFile = "E:\\Study\\3-2\\Криптографические основы защиты информации\\Лабы\\Crypto_9_13\\TestDecodedOutput.txt";

        public static void Do()
        {
            Console.WriteLine("Generate ANSI table ...");
            var ascii = new Ansi();
            ascii.WriteToFile();
            Console.WriteLine("ANSI table generated.");
            Console.WriteLine("Start encoding " + _fileToCompress + " ...");
            var text = File.ReadAllText(_fileToCompress);
            var encoder = new LZWEncoder();
            var b = encoder.EncodeToByteList(text);
            File.WriteAllBytes(_encodedFile, b);
            Console.WriteLine("File " + _fileToCompress + " encoded to " + _encodedFile);
            Console.WriteLine("Start decoding " + _encodedFile);
            var decoder = new LZWDecoder();
            var bo = File.ReadAllBytes(_encodedFile);
            var decodedOutput = decoder.DecodeFromCodes(bo);
            File.WriteAllText(_decodedFile, decodedOutput, Encoding.Default);
            Console.WriteLine("File " + _encodedFile + " decoded to " + _decodedFile);
        }
    }

    public class Shf
    {
        public class BitStream
        {
            public byte[] BytePointer;
            public uint BitPosition;
            public uint Index;
        }

        public struct Symbol
        {
            public uint Sym;
            public uint Count;
            public uint Code;
            public uint Bits;
        }

        private static void InitBitStream(ref BitStream stream, byte[] buffer)
        {
            stream.BytePointer = buffer;
            stream.BitPosition = 0;
        }

        private static void WriteBits(ref BitStream stream, uint x, uint bits)
        {
            var buffer = stream.BytePointer;
            var bit = stream.BitPosition;
            var mask = (uint)(1 << (int)(bits - 1));

            for (uint count = 0; count < bits; ++count)
            {
                buffer[stream.Index] = (byte)((buffer[stream.Index] & (0xff ^ (1 << (int)(7 - bit)))) + ((Convert.ToBoolean(x & mask) ? 1 : 0) << (int)(7 - bit)));
                x <<= 1;
                bit = (bit + 1) & 7;

                if (!Convert.ToBoolean(bit))
                {
                    ++stream.Index;
                }
            }

            stream.BytePointer = buffer;
            stream.BitPosition = bit;
        }

        private static void Histogram(byte[] input, Symbol[] sym, uint size)
        {
            Symbol temp;
            int i, swaps;
            var index = 0;

            for (i = 0; i < 256; ++i)
            {
                sym[i].Sym = (uint)i;
                sym[i].Count = 0;
                sym[i].Code = 0;
                sym[i].Bits = 0;
            }

            for (i = (int)size; Convert.ToBoolean(i); --i, ++index)
            {
                sym[input[index]].Count++;
            }

            do
            {
                swaps = 0;

                for (i = 0; i < 255; ++i)
                {
                    if (sym[i].Count < sym[i + 1].Count)
                    {
                        temp = sym[i];
                        sym[i] = sym[i + 1];
                        sym[i + 1] = temp;
                        swaps = 1;
                    }
                }
            } while (Convert.ToBoolean(swaps));
        }

        private static void MakeTree(Symbol[] sym, ref BitStream stream, uint code, uint bits, uint first, uint last)
        {
            uint i, size, sizeA, sizeB, lastA, firstB;

            if (first == last)
            {
                WriteBits(ref stream, 1, 1);
                WriteBits(ref stream, sym[first].Sym, 8);
                sym[first].Code = code;
                sym[first].Bits = bits;
                return;
            }
            else
            {
                WriteBits(ref stream, 0, 1);
            }

            size = 0;

            for (i = first; i <= last; ++i)
            {
                size += sym[i].Count;
            }

            sizeA = 0;

            for (i = first; sizeA < ((size + 1) >> 1) && i < last; ++i)
            {
                sizeA += sym[i].Count;
            }

            if (sizeA > 0)
            {
                WriteBits(ref stream, 1, 1);

                lastA = i - 1;

                MakeTree(sym, ref stream, (code << 1) + 0, bits + 1, first, lastA);
            }
            else
            {
                WriteBits(ref stream, 0, 1);
            }

            sizeB = size - sizeA;

            if (sizeB > 0)
            {
                WriteBits(ref stream, 1, 1);

                firstB = i;

                MakeTree(sym, ref stream, (code << 1) + 1, bits + 1, firstB, last);
            }
            else
            {
                WriteBits(ref stream, 0, 1);
            }
        }

        public static int Compress(byte[] input, byte[] output, uint inputSize)
        {
            var sym = new Symbol[256];
            Symbol temp;
            var stream = new BitStream();
            uint i, totalBytes, swaps, symbol, lastSymbol;

            if (inputSize < 1)
                return 0;

            InitBitStream(ref stream, output);
            Histogram(input, sym, inputSize);

            for (lastSymbol = 255; sym[lastSymbol].Count == 0; --lastSymbol) ;

            if (lastSymbol == 0)
                ++lastSymbol;

            MakeTree(sym, ref stream, 0, 0, 0, lastSymbol);

            do
            {
                swaps = 0;

                for (i = 0; i < 255; ++i)
                {
                    if (sym[i].Sym > sym[i + 1].Sym)
                    {
                        temp = sym[i];
                        sym[i] = sym[i + 1];
                        sym[i + 1] = temp;
                        swaps = 1;
                    }
                }
            } while (Convert.ToBoolean(swaps));

            for (i = 0; i < inputSize; ++i)
            {
                symbol = input[i];
                WriteBits(ref stream, sym[symbol].Code, sym[symbol].Bits);
            }

            totalBytes = stream.Index;

            if (stream.BitPosition > 0)
            {
                ++totalBytes;
            }

            return (int)totalBytes;
        }
        private const int MaxTreeNodes = 511;


        public class TreeNode
        {
            public TreeNode ChildA;
            public TreeNode ChildB;
            public int Symbol;
        }

        private static uint ReadBit(ref BitStream stream)
        {
            var buffer = stream.BytePointer;
            var bit = stream.BitPosition;
            var x = (uint)(Convert.ToBoolean(buffer[stream.Index] & (1 << (int)(7 - bit))) ? 1 : 0);
            bit = (bit + 1) & 7;

            if (!Convert.ToBoolean(bit))
            {
                ++stream.Index;
            }

            stream.BitPosition = bit;

            return x;
        }

        private static uint Read8Bits(ref BitStream stream)
        {
            var buffer = stream.BytePointer;
            var bit = stream.BitPosition;
            var x = (uint)((buffer[stream.Index] << (int)bit) | (buffer[stream.Index + 1] >> (int)(8 - bit)));
            ++stream.Index;

            return x;
        }

        private static TreeNode RecoverTree(TreeNode[] nodes, ref BitStream stream, ref uint nodeNumber)
        {
            TreeNode thisNode;

            thisNode = nodes[nodeNumber];
            nodeNumber = nodeNumber + 1;

            thisNode.Symbol = -1;
            thisNode.ChildA = null;
            thisNode.ChildB = null;

            if (Convert.ToBoolean(ReadBit(ref stream)))
            {
                thisNode.Symbol = (int)Read8Bits(ref stream);
                return thisNode;
            }

            if (Convert.ToBoolean(ReadBit(ref stream)))
            {
                thisNode.ChildA = RecoverTree(nodes, ref stream, ref nodeNumber);
            }

            if (Convert.ToBoolean(ReadBit(ref stream)))
            {
                thisNode.ChildB = RecoverTree(nodes, ref stream, ref nodeNumber);
            }

            return thisNode;
        }

        public static void Decompress(byte[] input, byte[] output, uint inputSize, uint outputSize)
        {
            var nodes = new TreeNode[MaxTreeNodes];

            for (var counter = 0; counter < nodes.Length; counter++)
            {
                nodes[counter] = new TreeNode();
            }

            var stream = new BitStream();
            uint i;

            if (inputSize < 1) return;

            InitBitStream(ref stream, input);

            uint nodeCount = 0;
            var root = RecoverTree(nodes, ref stream, ref nodeCount);
            var buffer = output;

            for (i = 0; i < outputSize; ++i)
            {
                var node = root;

                while (node.Symbol < 0)
                {
                    node = Convert.ToBoolean(ReadBit(ref stream)) ? node.ChildB : node.ChildA;
                }

                buffer[i] = (byte)node.Symbol;
            }
        }
        public static void Do()
        {
            var str = "This is an example for Shannon–Fano coding";
            var originalData = Encoding.Default.GetBytes(str);
            var originalDataSize = (uint)str.Length;
            var compressedData = new byte[originalDataSize * (101 / 100) + 384];

            var compressedDataSize = Compress(originalData, compressedData, originalDataSize);
            Console.WriteLine("size = " + compressedDataSize);

            var decompressedData = new byte[originalDataSize];

            Decompress(compressedData, decompressedData, (uint)compressedDataSize, originalDataSize);

            Console.WriteLine("Decompressed = " + Encoding.Default.GetString(decompressedData));
        }
    }


    public static class Haffman
    {
        class Node
        {
            private char[] _chars;
            private int _value;
            private Node _a;
            private Node _b;


            public char[] Chars
            {
                get { return this._chars; }
                set { this._chars = value; }
            }

            public int Value
            {
                get { return this._value; }
                set { this._value = value; }
            }

            public Node A
            {
                get { return this._a; }
                set { this._a = value; }
            }

            public Node B
            {
                get { return this._b; }
                set { this._b = value; }
            }

            public bool IsEnd
            {
                get { return this._chars.Length == 1; }
            }


            public Node(char chr, int value)
            {
                this._chars = new char[1] { chr };
                this._value = value;
            }

            public Node(Node a, Node b)
            {
                this._a = a;
                this._b = b;
                this._value = a.Value + b.Value;
                this.Chars = new char[a.Chars.Length + b.Chars.Length];
                Array.Copy(a.Chars, 0, this._chars, 0, a.Chars.Length);
                Array.Copy(b.Chars, 0, this._chars, a.Chars.Length, b.Chars.Length);
            }
        }

        static int min_node(Node[] nodes, int ignore)
        {
            Node node = null;
            int index = 0;
            for (int i = 0; i < nodes.Length; i++)
                if (nodes[i] != null && i != ignore)
                {
                    node = nodes[i];
                    index = i;
                    break;
                }
            for (int i = 0; i < nodes.Length; i++)
                if (nodes[i] != null && i != ignore && nodes[i].Value < node.Value)
                {
                    node = nodes[i];
                    index = i;
                }
            return index;
        }

        static void Calc(string text, out char[] chars, out int[] counts)
        {
            HashSet<char> charsSet = new HashSet<char>();

            foreach (char c in text)
                charsSet.Add(c);

            chars = charsSet.ToArray();
            counts = new int[chars.Length];

            foreach (char c in text)
                for (int i = 0; i < chars.Length; i++)
                    if (chars[i] == c)
                        counts[i]++;
        }

        static void Sort(char[] chars, int[] counts)
        {
            for (int i = chars.Length; i > 0; i--)
                for (int j = 0; j < i - 1; j++)
                    if (counts[j] > counts[j + 1])
                    {
                        char chr = chars[j];
                        chars[j] = chars[j + 1];
                        chars[j + 1] = chr;
                        int n = counts[j];
                        counts[j] = counts[j + 1];
                        counts[j + 1] = n;
                    }
        }

        static Node Tree(char[] chars, int[] counts)
        {
            Node[] nodes = new Node[chars.Length];
            for (int i = 0; i < nodes.Length; i++)
                nodes[i] = new Node(chars[i], counts[i]);

            do
            {
                int aIndex = min_node(nodes, -1);
                int bIndex = min_node(nodes, aIndex);
                Node c = new Node(nodes[aIndex], nodes[bIndex]);
                nodes[aIndex] = c;
                nodes[bIndex] = null;

                int count = 0;
                foreach (var t in nodes)
                {
                    if (t != null)
                    {
                        count++;
                    }
                }

                if (count == 1)
                    break;
            }
            while (true);

            Node node = null;
            foreach (var t in nodes)
            {
                if (t != null)
                {
                    node = t;
                    break;
                }
            }

            return node;
        }

        static bool[] calc_code(Node node, char c)
        {
            bool[] code = new bool[0];

            while (true)
            {
                if (node.IsEnd)
                    break;
                Array.Resize(ref code, code.Length + 1);
                if (node.A.Chars.Contains(c))
                {
                    node = node.A;
                    code[code.Length - 1] = false;
                }
                else
                {
                    node = node.B;
                    code[code.Length - 1] = true;
                }
            }
            return code;
        }

        static bool[] Encode(string text, out Node node)
        {
            char[] chars;
            int[] counts;
            Calc(text, out chars, out counts);
            Sort(chars, counts);
            node = Tree(chars, counts);

            bool[] encodedText = new bool[0];
            for (int i = 0; i < text.Length; i++)
            {
                bool[] code = calc_code(node, text[i]);
                int length = encodedText.Length;
                Array.Resize(ref encodedText, length + code.Length);
                Array.Copy(code, 0, encodedText, length, code.Length);
            }

            return encodedText;
        }

        static string Decode(bool[] encodedText, Node node)
        {
            Node nodeA = node;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < encodedText.Length;)
            {
                while (!node.IsEnd)
                {
                    if (encodedText[i++])
                        node = node.B;
                    else
                        node = node.A;
                }
                builder.Append(node.Chars[0]);
                node = nodeA;
            }
            return builder.ToString();
        }

        public static void Do()
        {
            Console.Write("Enter text: ");
            string text = Console.ReadLine();
            bool[] encodedText = Encode(text, out var node);
            foreach (bool b in encodedText)
                Console.Write(b ? "1" : "0");
            Console.WriteLine();
            string decodedText = Decode(encodedText, node);
            Console.WriteLine(decodedText);
            Console.ReadKey();
        }
    }
}