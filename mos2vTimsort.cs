using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Burrows_Wheeler
{
    

    public class mos2vTimsort
    {
        private const int RUN = 32;

        public static void Sort(List<string> arr)
        {
            int n = arr.Count;
            for (int i = 0; i < n; i += RUN)
            {
                InsertionSort(arr, i, Math.Min((i + RUN - 1), (n - 1)));
            }

            for (int size = RUN; size < n; size = 2 * size)
            {
                for (int left = 0; left < n; left += 2 * size)
                {
                    int mid = left + size - 1;
                    int right = Math.Min((left + 2 * size - 1), (n - 1));
                    if (mid < right)
                    {
                        Merge(arr, left, mid, right);
                    }
                }
            }
        }

        private static void InsertionSort(List<string> arr, int left, int right)
        {
            for (int i = left + 1; i <= right; i++)
            {
                string temp = arr[i];
                int j = i - 1;
                while (j >= left && arr[j].CompareTo(temp) > 0)
                {
                    arr[j + 1] = arr[j];
                    j--;
                }
                arr[j + 1] = temp;
            }
        }

        private static void Merge(List<string> arr, int left, int mid, int right)
        {
            int len1 = mid - left + 1;
            int len2 = right - mid;

            string[] leftArray = new string[len1];
            string[] rightArray = new string[len2];

            for (int x = 0; x < len1; x++)
            {
                leftArray[x] = arr[left + x];
            }
            for (int x = 0; x < len2; x++)
            {
                rightArray[x] = arr[mid + 1 + x];
            }

            int i = 0, j = 0;
            int k = left;

            while (i < len1 && j < len2)
            {
                if (leftArray[i].CompareTo(rightArray[j]) <= 0)
                {
                    arr[k] = leftArray[i];
                    i++;
                }
                else
                {
                    arr[k] = rightArray[j];
                    j++;
                }
                k++;
            }

            while (i < len1)
            {
                arr[k] = leftArray[i];
                k++;
                i++;
            }

            while (j < len2)
            {
                arr[k] = rightArray[j];
                k++;
                j++;
            }
        }
    }
}
