using SortAlgorithms.Implementations;
using SortAlgorithms.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SortAlgorithms
{
    public class Program
    {
        public static void Main()
        {
            RunSorting();
        }

        private static void RunSorting()
        {

            var sortArrayLengths = new int[]
            {
                10000, 30000, 90000, 270000, 810000
            };

            var sorts = new List<ISort>
            {
                new BucketSort(),
                new InsertSort(),
                new ShellSort()
            };

            foreach (var lengths in sortArrayLengths)
            {
                var array = BuildRandomArray(lengths);
                foreach (var sort in sorts)
                {
                    var tempArray = array.ToArray();
                    MeasureSortEfficiency(sort, tempArray);
                }
            }
        }

        private static IEnumerable<int> BuildRandomArray(int count)
        {
            var random = new Random();

            return Enumerable
                .Repeat(0, count)
                .Select(x => random.Next(0, int.MaxValue));
        }

        private static void MeasureSortEfficiency(ISort sort, IEnumerable<int> data, bool printArray = false)
        {
            var sw = new Stopwatch();

            sw.Start();
            var sortedArray = sort.Sort(data);
            sw.Stop();

            Console.WriteLine($"{sort.GetType().Name}:");
            Console.WriteLine($"Array length = {data.Count()} -> {sw.ElapsedTicks} ticks / {sw.ElapsedMilliseconds} ms\n");

            if (printArray)
            {
                Console.WriteLine(string.Join(", ", sortedArray));
            }
        }
    }
}
