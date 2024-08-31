using System;
using System.IO;
using Burrows_Wheeler;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Burrows_Wheelerv2
{
    internal class Program
    {
        public static (string, int) Burrows(string txt)
        {
            int length = txt.Length;
            //Console.WriteLine("Original Text: " + txt);
            int[] suffixies = new int[txt.Length];
            for (int i = 0; i < length; i++)
            {
                suffixies[i] = i;
            }
            //Array.Sort(suffixies);

            mos2vsort.QuickSort(suffixies, txt);
            char[] transform = new char[txt.Length];
            int originalIndex = 0;
            for (int i = 0; i < txt.Length; i++)
            {
                int suffix_indx = suffixies[i];
                transform[i] = txt[(suffix_indx + length - 1) % length];

                if (suffix_indx == 0)
                {
                    originalIndex = i;
                }
            }

            string bwt = new string(transform);


            //Console.WriteLine("Decoded Text: " + bwt);
            return (bwt, originalIndex);
        }
        public static string Burrows_Inverse(string txt, int index)
        {

            int length = txt.Length;
            char[] original = new char[length];
            int[] next = new int[length];

            // Step 1: Count occurrences of each character
            int[] count = new int[256];
            foreach (char c in txt)
            {
                count[c]++;
            }

            // Step 2: Calculate starting position for each character in the sorted array
            int[] startingPositions = new int[256];
            int sum = 0;
            for (int i = 0; i < 256; i++)
            {
                startingPositions[i] = sum;
                sum += count[i];
            }

            // Step 3: Build the 'next' array
            int[] characterPositions = new int[256];
            Array.Copy(startingPositions, characterPositions, 256);

            for (int i = 0; i < length; i++)
            {
                char c = txt[i];
                int sortedIndex = characterPositions[c];
                next[sortedIndex] = i;
                characterPositions[c]++;
            }

            // Step 4: Reconstruct the original string
            int current = index;
            for (int i = 0; i < length; i++)
            {
                current = next[current];
                original[i] = txt[current];
            }

            return new string(original);

        }
        public static byte[] moveToFront(string text)
        {

            // Initialize the list with all ASCII characters (0-255)
            List<char> symbols = new List<char>(256);
            for (int i = 0; i < 256; i++)
            {
                symbols.Add((char)i);
            }

            // List to store the encoded output
            List<byte> encoded = new List<byte>();

            // Iterate over each character in the input string
            foreach (char c in text)
            {
                // Find the index of the current character
                int index = symbols.IndexOf(c);

                // Add the index to the encoded output as a byte
                encoded.Add((byte)index);

                // Move the current character to the front of the list
                
                symbols.RemoveAt(index);
                
                symbols.Insert(0, c);
            }

            // Convert the List<byte> to an array of bytes and return
            return encoded.ToArray();
        }
        public static string moveToFront_inverse(byte[] pos)
        {
            char[] alphabet = new char[256];
            for (int i = 0; i < 256; i++)
                alphabet[i] = (char)i;

            char[] decoded = new char[pos.Length];

            for (int i = 0; i < pos.Length; i++)
            {
                int index = pos[i];
                char c = alphabet[index];
                decoded[i] = c;

                if (index > 0)
                {
                    Array.Copy(alphabet, 0, alphabet, 1, index);
                    alphabet[0] = c;
                }
            }

            return new string(decoded);
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

            byte[] unique_val = integers.Distinct().ToArray();

            int[] frequency = new int[unique_val.Length];

            for (int i = 0; i < unique_val.Length; i++)
            {
                var result = integers.Count(x => x == unique_val[i]);
                frequency[i] = result;

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
        public static Dictionary<byte, string> GenerateHuffmanCodes(Node root)
        {
            Dictionary<byte, string> huffmanCodes = new Dictionary<byte, string>();

            if (root == null)
                return huffmanCodes;

            TraverseTree(root, string.Empty, huffmanCodes);
            return huffmanCodes;
        }

        public static void TraverseTree(Node node, string code, Dictionary<byte, string> huffmanCodes)
        {
            if (node == null)
                return;

            if (node.Value != -1)
            {
                huffmanCodes[(byte)node.Value] = code;
            }

            TraverseTree(node.left, code + "0", huffmanCodes);
            TraverseTree(node.right, code + "1", huffmanCodes);
        }

        public static (byte[] encodedData, int paddingBits) EncodeData(byte[] data, Dictionary<byte, string> huffmanCodes)
        {
            StringBuilder encodedStringBuilder = new StringBuilder();

            foreach (byte b in data)
            {
                encodedStringBuilder.Append(huffmanCodes[b]);
            }

            // Convert the binary string to a byte array
            string binaryString = encodedStringBuilder.ToString();
            int paddingBits = (8 - (binaryString.Length % 8)) % 8; // Calculate padding bits needed
            binaryString = binaryString.PadRight(binaryString.Length + paddingBits, '0'); // Add padding zeros

            int numBytes = binaryString.Length / 8;
            byte[] encodedData = new byte[numBytes];

            for (int i = 0; i < binaryString.Length; i++)
            {
                if (binaryString[i] == '1')
                {
                    encodedData[i / 8] |= (byte)(1 << (7 - (i % 8)));
                }
            }

            return (encodedData, paddingBits);
        }
        public static void SaveToBinaryFile(string filePath, byte[] data)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(data, 0, data.Length);
            }
        }
        public static byte[] ReadFromBinaryFile(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                return data;
            }
        }
        public static byte[] DecodeData(byte[] encodedData, int paddingBits, Dictionary<byte, string> huffmanCodes)
        {
            // Create a reverse lookup dictionary for decoding
            var reverseHuffmanCodes = huffmanCodes.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

            // Convert the byte array to a binary string
            StringBuilder binaryStringBuilder = new StringBuilder();

            foreach (byte b in encodedData)
            {
                for (int i = 0; i < 8; i++)
                {
                    binaryStringBuilder.Append((b & (1 << (7 - i))) != 0 ? '1' : '0');
                }
            }

            string binaryString = binaryStringBuilder.ToString();

            // Remove the padding bits from the end
            binaryString = binaryString.Substring(0, binaryString.Length - paddingBits);

            // Decode the binary string
            List<byte> decodedBytes = new List<byte>();
            string currentCode = "";

            foreach (char bit in binaryString)
            {
                currentCode += bit;

                if (reverseHuffmanCodes.ContainsKey(currentCode))
                {
                    decodedBytes.Add(reverseHuffmanCodes[currentCode]);
                    currentCode = "";
                }
            }

            return decodedBytes.ToArray();
        }
        static void Main(string[] args)
        {
            string filePath = "C:\\Users\\mos3060ti\\Desktop\\Burrows-Wheeler-Compression\\Test Files\\Large Cases\\Small\\aesop-4copies.txt";
            string text = File.ReadAllText(filePath, Encoding.Latin1);
         

            var (bwt, originalRow) = Burrows(text);
            byte[] integers = moveToFront(bwt);
            Node root = Huffman_encoding(integers);

            Dictionary<byte, string> huffmanCodes = GenerateHuffmanCodes(root);

            var (encoded, paddingBits) = EncodeData(integers, huffmanCodes);

            SaveToBinaryFile("C:\\Users\\mos3060ti\\Desktop\\Burrows-Wheeler-Compression\\Output Files\\encodedData.bin", encoded);

            byte[] decodedData = DecodeData(encoded, paddingBits, huffmanCodes);
            string a = moveToFront_inverse(decodedData);
            string original_txt = Burrows_Inverse(a, originalRow);
            string outputFilePath = "C:\\Users\\mos3060ti\\Desktop\\Burrows-Wheeler-Compression\\Output Files\\decodedOutput.txt";
            File.WriteAllText(outputFilePath, original_txt, Encoding.Latin1);

        }

    }
}
