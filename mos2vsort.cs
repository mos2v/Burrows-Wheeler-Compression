using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Burrows_Wheeler
{
    public class mos2vsort
    {
        public static void QuickSort(int[] indices, string txt)
        {

            Stack<int> stack = new Stack<int>();
            stack.Push(0);
            stack.Push(indices.Length - 1);

            while (stack.Count > 0) 
            {
                int high = stack.Pop();
                int low = stack.Pop();

                if (low < high)
                {
                    int pivot_indx = Partition(indices, txt, low, high);
                    
                    if (pivot_indx - 1 > low)
                    {
                        stack.Push(low);
                        stack.Push(pivot_indx - 1);
                    }

                    if (pivot_indx + 1 < high) 
                    {
                        stack.Push(pivot_indx + 1);
                        stack.Push(high);
                    }
                }
                

            }
        }

        public static int Partition(int[] indices, string txt, int low, int high)
        {
            int pivot = indices[high];

            int k = low - 1;
            for (int i = low; i < high; i++) 
            {
                if (SuffixesCompare(txt, indices[i], pivot) <= 0)
                {
                    k++;
                    if (k != i)
                    {
                        Swap(indices, k, i);
                    }
                }
            }

            Swap(indices, k + 1, high);

            return k + 1;
        }
        public static int SuffixesCompare(string txt, int a, int b) 
        {
            int length = txt.Length;
            for (int i = 0; i < length; i++)
            {
                char charA = txt[(a + i) % length];
                char charB = txt[(b + i) % length];
                if (charA != charB)
                {
                    return charA.CompareTo(charB);
                }
            }
            return 0;
        }

        public static void Swap(int[] suffix, int k, int i)
        {
            int tmp = suffix[k];
            suffix[k] = suffix[i];
            suffix[i] = tmp;
        }
    }
}
