using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Burrows_Wheeler
{
    public class mos2vsort
    {
        public void QuickSort(string[] suffix, int low, int high)
        {
            if (low < high) 
            {
                int pivot_indx = Partition(suffix, low, high);

                QuickSort(suffix, low, pivot_indx - 1);

                QuickSort(suffix, pivot_indx + 1, high);
            }
        }

        public int Partition(string[] suffix, int low, int high)
        {
            string pivot = suffix[high];

            int k = low - 1;
            for (int i = low; i < high; i++) 
            {
                if (String.Compare(suffix[i], pivot) <= 0)
                {
                    k++;
                    if (k != i)
                    {
                        Swap(suffix, k, i);
                    }
                }
            }

            Swap(suffix, k + 1, high);

            return k + 1;
        }

        public void Swap(string[] suffix, int k, int i)
        {
            string tmp = suffix[k];
            suffix[k] = suffix[i];
            suffix[i] = tmp;
        }
    }
}
