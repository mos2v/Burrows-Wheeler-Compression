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
        public static string Burrows(string txt)
        {
            int length = txt.Length;
            Console.WriteLine("Original Text: " + txt);
            int[] suffixies = new int[txt.Length];
            for (int i = 0; i < length; i++)
            {
                suffixies[i] = i;
            }
            //Array.Sort(suffixies);
            mos2vsort.QuickSort(suffixies, txt);
            char[] transform = new char[txt.Length];
            for (int i = 0; i < txt.Length; i++)
            {
                int suffix_indx = suffixies[i];
                transform[i] = txt[(suffix_indx + length - 1) % length];
            }

            string bwt = new string(transform);

         
            Console.WriteLine("Decoded Text: " + bwt);
            return bwt;
        }
        public static string Burrows_Inverse(string txt) 
        {
            string bwt = "fdafcaaaabb";

            int length = bwt.Length;
            // Step 1: Create the first column by sorting the BWT string
            int[] indices = new int[length];
            for (int i = 0; i < length; i++)
            {
                indices[i] = i;
            }

            mos2vsort.QuickSort(indices, bwt);

            // Create the first column from the sorted indices
            char[] firstColumn = new char[length];
            for (int i = 0; i < length; i++)
            {
                firstColumn[i] = bwt[indices[i]];
            }

            // Step 2: Build the LF mapping
            int[] lfMapping = new int[length];
            Dictionary<char, Queue<int>> charToPositions = new Dictionary<char, Queue<int>>();

            for (int i = 0; i < length; i++)
            {
                if (!charToPositions.ContainsKey(firstColumn[i]))
                {
                    charToPositions[firstColumn[i]] = new Queue<int>();
                }
                charToPositions[firstColumn[i]].Enqueue(i);
            }

            for (int i = 0; i < length; i++)
            {
                char c = bwt[i];
                lfMapping[i] = charToPositions[c].Dequeue();
            }

            // Step 3: Reconstruct the original string
            char[] original = new char[length];
            int row = 2; // Start at the smallest index if '$' is not present

            // Find the row with '$' or use the smallest index if no '$' is present
            if (bwt.Contains('$'))
            {
                for (int i = 0; i < length; i++)
                {
                    if (bwt[i] == '$')
                    {
                        row = i;
                        break;
                    }
                }
            }

            // Reconstruction
            for (int i = length - 1; i >= 0; i--)
            {
                original[i] = bwt[row];
                row = lfMapping[row];
            }


            return new string(original);

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


            return position;
        }
        public static string moveToFront_inverse(byte[] pos)
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
                    Node X = pq.Dequeue();
                    Node Y = pq.Dequeue();

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
            public Dictionary<int, string> GenerateHuffmanCodes(Node root)
            {
            Dictionary <int, string> huffmanCodes = new Dictionary<int, string>();
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

                    //huffmanCodes[node.Value] = code;
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
           

        static void Main(string[] args)
        {
            string filePath = "C:\\Users\\mos3060ti\\Desktop\\ASU\\Y3\\2nd Term\\Algorithms\\project\\[2] Burrow-Wheeler Compression\\Test Files\\Large Cases\\Large\\dickens.txt";
            string text = File.ReadAllText(filePath);
            //string a = Burrows(text);

            Console.WriteLine(Burrows_Inverse("a"));
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
     
    }
}
