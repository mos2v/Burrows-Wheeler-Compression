using Burrows_Wheeler;
using BurrowsWheeler;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Burrows_Wheelerv2
{
    internal class Program
    {
        public static string Burrows(string txt, char a)
        {
            int length = txt.Length;
            Console.WriteLine("Original Text: " + txt);
            List<string> suffixies = GenerateSuffixies(txt);
            string[] suffix = new string[length];
            mos2vsort.QuickSort(suffix, 0, length - 1);
            char[] transform = new char[txt.Length];
            for (int i = 0; i < txt.Length; i++)
            {
                transform[i] = suffixies[i][txt.Length - 1];
            }

            string bwt = new string(transform);
            Console.WriteLine("BWT: " + bwt);

            int[] next = ConstructNextArray(bwt);

            int originalIndex = suffixies.IndexOf(txt);

            string decodedText = Decode(bwt, next, originalIndex);
            //Console.WriteLine("Decoded Text: " + decodedText);
            
            if (a == 'E')
            {
                return new string(transform);
            }
            else
            {
                return decodedText;
            }
        }

        public static List<string> GenerateSuffixies(string txt)
        {
            List<string> rotations = new List<string>();
            for (int i = 0; i < txt.Length; i++)
            {
                string rotation = txt.Substring(i) + txt.Substring(0, i);
                rotations.Add(rotation);
            }
            return rotations;
        }

        public static int[] ConstructNextArray(string bwt)
        {
            int length = bwt.Length;
            int[] next = new int[length];
            int[] count = new int[256];
            int[] rank = new int[length];

            foreach (char c in bwt)
            {
                count[c]++;
            }

            int sum = 0;
            for (int i = 0; i < 256; i++)
            {
                int temp = count[i];
                count[i] = sum;
                sum += temp;
            }

            for (int i = 0; i < length; i++)
            {
                rank[i] = count[bwt[i]];
                count[bwt[i]]++;
            }

            for (int i = 0; i < length; i++)
            {
                next[rank[i]] = i;
            }

            return next;
        }

        public static string Decode(string bwt, int[] next, int originalIndex)
        {
            int length = bwt.Length;
            char[] originalOrder = new char[length];
            int index = originalIndex;

            for (int i = 0; i < length; i++)
            {
                originalOrder[i] = bwt[index];
                index = next[index];
            }

            string decodedText = new string(originalOrder);
            decodedText = decodedText.Substring(1) + decodedText[0];
            return decodedText;
        }


        public static byte[] moveToFront(string text)
        {
            
            List<char> extendedAsciiSymbols = new List<char>(getAsciiSymbols());



            byte[] position = new byte[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                int index = extendedAsciiSymbols.IndexOf(text[i]);
                position[i] = (byte)index;
                char a = extendedAsciiSymbols[index];
                extendedAsciiSymbols.RemoveAt(index);
                extendedAsciiSymbols.Insert(0, a);
            }

            //for (int i = 0; i < position.Length; i++)
            //    Console.WriteLine(position[i]);

            return position;
        }
        public static string decode(byte[] pos)
        {
            int length = pos.Length;
            StringBuilder decoded = new StringBuilder();
            List<char> extendedAsciiSymbols1 = new List<char>(getAsciiSymbols());
 
            foreach (int index in pos)
            {
                char a = extendedAsciiSymbols1[index];
                decoded.Append(a);
                extendedAsciiSymbols1.RemoveAt(index);
                extendedAsciiSymbols1.Insert(0, a);
            }

            string d = decoded.ToString();
            return d;
        }

        public static List<char> getAsciiSymbols()
        {
            List<char> extendedAsciiSymbols = new List<char>();
            for (int i = 0; i < 256; i++)
            {
                extendedAsciiSymbols.Add((char)i);
            }
            return extendedAsciiSymbols;
        }

        public class HuffmanCoding
        {
            public static Node Huffman_encoding(byte[] integers)
            {
                //byte[] integers = { 97, 98, 0, 0, 1, 0, 1, 0, 0, 0, 1, 99, 0, 1, 2, 0, 1, 0, 0, 1, 2 };

                byte[] unique_val = integers.Distinct().ToArray();

                int[] frequency = new int[unique_val.Length];

                for (int i = 0; i < unique_val.Length; i++)
                {
                    var result = integers.Count(x => x == unique_val[i]);
                    frequency[i] = result;

                    Console.WriteLine(unique_val[i]);
                    Console.WriteLine(result);

                }


                mos2vPriorityQueue pq = new mos2vPriorityQueue();


                for (int i = 0; i < unique_val.Length; ++i)
                {
                    Node input = new Node();
                    input.Value = unique_val[i];

                    input.Frequency = frequency[i];
                    input.left = input.right = null;

                    pq.Enqueue(input);
                }
                while (pq.Count > 1)
                {
                    Node Y = pq.Dequeue();
                    Node X = pq.Dequeue();

                    Node mergednode = new Node();
                    mergednode.Value = -1;
                    mergednode.left = X;
                    mergednode.right = Y;
                    mergednode.Frequency = X.Frequency + Y.Frequency;
                    pq.Enqueue(mergednode);
                }
                Node root = pq.Dequeue();
                return root;

            }
            private Dictionary<int, string> huffmanCodes;

            public HuffmanCoding()
            {
                huffmanCodes = new Dictionary<int, string>();
            }

            public Dictionary<int, string> GenerateHuffmanCodes(Node root)
            {
                if (root == null)
                    return huffmanCodes;

                TraverseTree(root, string.Empty);
                return huffmanCodes;
            }

            public void TraverseTree(Node node, string code)
            {
                if (node == null)
                    return;



                if (node.Value != -1)
                {

                    huffmanCodes[node.Value] = code;
                }
                //Console.WriteLine("node val");
                //Console.WriteLine(node.Value);
                //Console.WriteLine("node freq");

                //Console.WriteLine(node.Frequency);
                //Console.WriteLine("node code");

                //Console.WriteLine(code);
                TraverseTree(node.left, code + "0");
                TraverseTree(node.right, code + "1");
            }
            public string encode(byte[] a)
            {
                StringBuilder e = new StringBuilder();
                foreach (int c in a) 
                {
                    e.Append(huffmanCodes[c]);
                }
                return e.ToString();
            }
            public void SerializeTree(Node node, StreamWriter writer)
            {
                if (node == null)
                    return;

                if (node.left == null && node.right == null)
                {
                    writer.WriteLine($"L{node.Value}");
                }
                else
                {
                    writer.WriteLine("I");
                }

                SerializeTree(node.left, writer);
                SerializeTree(node.right, writer);
            }

            public void WriteToFile(Node root, string encodedData, string filePath)
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    SerializeTree(root, writer);
                    writer.WriteLine("DATA");
                    writer.WriteLine(encodedData);
                }
            }
            public Node DeserializeTree(StreamReader reader)
            {
                if (reader.EndOfStream)
                    return null;

                string line = reader.ReadLine();
                if (line.StartsWith("L"))
                {
                    return new Node { Value = int.Parse(line.Substring(1)) };
                }
                else
                {
                    Node node = new Node { Value = -1 };
                    node.left = DeserializeTree(reader);
                    node.right = DeserializeTree(reader);
                    return node;
                }
            }

            public byte[] Decode(string encodedData, Node root)
            {
                List<byte> decoded = new List<byte>();
                Node current = root;
                foreach (char bit in encodedData)
                {
                    current = (bit == '0') ? current.left : current.right;

                    if (current.left == null && current.right == null)
                    {
                        decoded.Add((byte) current.Value);
                        current = root;
                    }
                }
                return decoded.ToArray();
            }
            public string huffman1(byte[] integers)
        {
            //HuffmanCoding huff = new HuffmanCoding();
            Node root = Huffman_encoding(integers);
            Dictionary<int, string> codes = GenerateHuffmanCodes(root);
            foreach(var code in codes)
            {
                Console.WriteLine($"Character: {code.Key}, Code: {code.Value}");
            }
            string s = encode(integers);
            Console.WriteLine(s);
            

            string filepath = "D:\\FCIS-2024\\FCIS-3rd-Year\\2nd term\\Algorithm\\Project\\huffman.txt";
            WriteToFile(root, s, filepath);

            return s;
        }
        }



        static void Main(string[] args)
        {

        }
        public static void ProcessFileContent(string input)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            
            /*The logic of the whole project goes from here as the input is the content of the textFile chosen for our main function*/
            stopwatch.Stop();
            Console.WriteLine("fffff");

            //SaveToBinaryFile.SaveToBinary(lastEncoding(input), newFilePath);
            
            TimeSpan elapsedTime = stopwatch.Elapsed;
            double seconds = elapsedTime.TotalSeconds;

            


            
            
            
        }
        public static string lastEncoding(string txt )
        {
            string burrowsEncoded = Burrows(txt , 'E');
            byte[] huffmaninput = moveToFront(burrowsEncoded);
            HuffmanCoding huff = new HuffmanCoding();
            string output = huff.huffman1(huffmaninput);
            return output;
        }
        public static string lastDecoding()
        {
            // decoding
            Node root;
            string data;

            HuffmanCoding huff = new HuffmanCoding();
            string filePath = "D:\\FCIS-2024\\FCIS-3rd-Year\\2nd term\\Algorithm\\Project\\huffman.txt";
            using (StreamReader reader = new StreamReader(filePath))
            {
                root = huff.DeserializeTree(reader);
                reader.ReadLine(); // Skip the "DATA" line
                data = reader.ReadLine();
            }
            byte[] decodedd = huff.Decode(data, root);
            
            string decoded = decode(decodedd);

            string orig = Burrows(decoded, 'D');

            return orig;
        }



    }
}
