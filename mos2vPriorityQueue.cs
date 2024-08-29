using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Burrows_Wheelerv2
{
    public class Node
    {
        public int Value { get; set; }
        public int Frequency { get; set; }

        public Node left;
        public Node right;
        //public Node(int value, int priority)
        //{
        //    Value = value;
        //    Frequency = priority;
        //}
    }




    public class mos2vPriorityQueue
    {
        private List<Node> heap;

        public mos2vPriorityQueue()
        {
            heap = new List<Node>();
        }

        public void Enqueue(Node node)
        {
            heap.Add(node);
            HeapifyUp(heap.Count - 1);
        }

        public Node Dequeue()
        {
            if (heap.Count == 0)
            {
                throw new InvalidOperationException("The priority queue is empty.");
            }

            Node root = heap[0];
            heap[0] = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);
            HeapifyDown(0);

            return root;
        }

        public Node Peek()
        {
            if (heap.Count == 0)
            {
                throw new InvalidOperationException("The priority queue is empty.");
            }

            return heap[0];
        }

        public int Count => heap.Count;

        private void HeapifyUp(int index)
        {
            int parentIndex = (index - 1) / 2;
            if (index > 0 && heap[index].Frequency < heap[parentIndex].Frequency)
            {
                Swap(index, parentIndex);
                HeapifyUp(parentIndex);
            }
        }

        private void HeapifyDown(int index)
        {
            int smallest = index;
            int leftChild = 2 * index + 1;
            int rightChild = 2 * index + 2;

            if (leftChild < heap.Count && heap[leftChild].Frequency < heap[smallest].Frequency)
            {
                smallest = leftChild;
            }

            if (rightChild < heap.Count && heap[rightChild].Frequency < heap[smallest].Frequency)
            {
                smallest = rightChild;
            }

            if (smallest != index)
            {
                Swap(index, smallest);
                HeapifyDown(smallest);
            }
        }

        private void Swap(int index1, int index2)
        {
            Node temp = heap[index1];
            heap[index1] = heap[index2];
            heap[index2] = temp;
        }
    }
    
}
