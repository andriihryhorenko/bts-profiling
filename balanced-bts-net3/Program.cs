using balanced_bts.BTree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace balanced_bts
{
    internal class Program
    {

       

        static void Main(string[] args)
        {

            Thread.Sleep(3);
          
            //default
            var numberOfDatasets = 100;
            var step = 1000;
            var datasetL = 100;

            var arguments = new Dictionary<string, string>();

            foreach (string argument in args)
            {
                string[] splitted = argument.Split('=');

                if (splitted.Length == 2)
                {
                    arguments[splitted[0]] = splitted[1];
                }
            }

            var value = Environment.GetEnvironmentVariable("DATASETSN");
            if (!string.IsNullOrEmpty(value))
            {
                int.TryParse(value, out numberOfDatasets);
            }

            value = Environment.GetEnvironmentVariable("STEP");
            if (!string.IsNullOrEmpty(value))
            {
                int.TryParse(value, out step);
            }

            value = Environment.GetEnvironmentVariable("FIRSTLEN");
            if (!string.IsNullOrEmpty(value))
            {
                int.TryParse(value, out datasetL);
            }

            if (arguments.Any())
            {
                if (arguments.ContainsKey("-n"))
                {
                    int.TryParse(arguments["-n"], out numberOfDatasets);
                }

                if (arguments.ContainsKey("-s"))
                {
                    int.TryParse(arguments["-s"], out step);
                }

                if (arguments.ContainsKey("-l"))
                {
                    int.TryParse(arguments["-l"], out datasetL);
                }
            }
            

            //insert to binary tree
            var insertSw = new Stopwatch();
            var timeResults = new List<TimeProfilingResult>();
            BinaryTree btree = new BinaryTree();
            for (var i=0;i<numberOfDatasets;i++)
            {
                var dataset = GenerateDataset(datasetL);
                insertSw.Start();
                btree = InsertToBalancedTree(btree, dataset);
                var inOrderTravert = new List<int>();
                btree.TraversePreOrder(btree.Root, ref inOrderTravert);
                insertSw.Stop();
                var timeResult = new TimeProfilingResult(inOrderTravert.Count);
                timeResult.Insert = insertSw.Elapsed;
                timeResults.Add(timeResult);
            }

          ;

            //find random 100 elements
            foreach (var timeResult in timeResults.OrderByDescending(x=>x.Length)) 
            {

                var findSw = new Stopwatch();
                var inOrderTravert = new List<int>();
                btree.TraversePreOrder(btree.Root, ref inOrderTravert);
                var fullDatasetArray = inOrderTravert.ToArray();

                var random100 = RandomElements(fullDatasetArray, 100);

                findSw.Start();
                for (var i = 0; i < random100.Length; i++)
                {
                    var findResult = btree.Find(random100[i]);
                }
                findSw.Stop();
                timeResult.Search = findSw.Elapsed;

            }



            //delete random 100 elements
            foreach (var timeResult in timeResults.OrderByDescending(x => x.Length))
            {
                var deleteSw = new Stopwatch();
                var inOrderTravert = new List<int>();
                btree.TraversePreOrder(btree.Root, ref inOrderTravert);
                var fullDatasetArray = inOrderTravert.ToArray();

                var random100 = RandomElements(fullDatasetArray, 100);



                deleteSw.Start();
                RemoveFromBalancedTree(btree, random100);
                deleteSw.Stop();
                timeResult.Delete = deleteSw.Elapsed;


            }



            foreach (var result in timeResults)
            {
                result.ToPrint();
            }

            Console.ReadKey();
            
        }


        static BinaryTree InsertToBalancedTree(BinaryTree binaryTree, int[] dataset, bool printTree = false)
        {
            foreach (var item in dataset)
            {
                binaryTree.Add(item);
                var inOrderTravert = new List<Node>();
                binaryTree.TraverseInOrder(binaryTree.Root, ref inOrderTravert);

                binaryTree.BuildBalancedTree(inOrderTravert.Select(x => x.Data).ToList());
            }
            if (printTree)
                binaryTree.Print();
            return binaryTree;

        }

        static BinaryTree RemoveFromBalancedTree(BinaryTree binaryTree, int[] dataset, bool printTree = false)
        {
            foreach (var item in dataset)
            {
                binaryTree.Remove(item);
                var inOrderTravert = new List<Node>();
                binaryTree.TraverseInOrder(binaryTree.Root, ref inOrderTravert);

                binaryTree.BuildBalancedTree(inOrderTravert.Select(x => x.Data).ToList());
            }
            if (printTree)
                binaryTree.Print();
            return binaryTree;

        }


        static int[] RandomElements(int[] allelements, int length)
        {
            Random randomGenerator = new Random();
            if (allelements.Length < length)
                return allelements;

            int[] result = new int[length];

            for (var i = 0; i < length; i++)
            {
                var index = randomGenerator.Next(0, length-1);
                result[i] = allelements[index];
            }

            return result;
        }

        static int[] GenerateDataset(int length)
        {
            Random randomGenerator = new Random();

            int[] result = new int[length];

            for (var i= 0; i < length; i++){
                result[i] = randomGenerator.Next(1, length*10000);
            }

            return result;
        }


        public static int[] CountingSort(int[] array)
        {
            var size = array.Length;
            var maxElement = GetMaxVal(array, size);
            var occurrences = new int[maxElement + 1];
            for (int i = 0; i < maxElement + 1; i++)
            {
                occurrences[i] = 0;
            }
            for (int i = 0; i < size; i++)
            {
                occurrences[array[i]]++;
            }
            for (int i = 0, j = 0; i <= maxElement; i++)
            {
                while (occurrences[i] > 0)
                {
                    array[j] = i;
                    j++;
                    occurrences[i]--;
                }
            }
            return array;
        }

        public static int GetMaxVal(int[] array, int size)
        {
            var maxVal = array[0];
            for (int i = 1; i < size; i++)
                if (array[i] > maxVal)
                    maxVal = array[i];
            return maxVal;
        }




        public class TimeProfilingResult
        {
            public int Length;
            public TimeSpan Insert { get; set; }
            public TimeSpan Search { get; set; }
            public TimeSpan Delete { get; set; }

            public TimeProfilingResult(int length)
            {
                Length = length;
            }


            public void ToPrint()
            {
                Console.WriteLine($"[L:{this.Length}] Insert:{this.Insert}, Search:{this.Search}, Delete:{this.Delete}");
            }
        }
    }
}
