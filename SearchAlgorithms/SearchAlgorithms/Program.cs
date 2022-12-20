using SearchAlgorithms.Implementations;
using SearchAlgorithms.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SearchAlgorithms
{
    public class Program
    {
        public static void Main()
        {
            RunSearching();
        }

        private static IEnumerable<int> BuildRandomArray(int count)
        {
            var random = new Random();

            return Enumerable
                .Repeat(0, count)
                .Select(x => random.Next(0, int.MaxValue)).OrderBy(x => x);
        }

        private static void RunSearching()
        {
            var searches = new List<ISearch>
            {
                new BinarySearch(),
                new InterpolationSearch()
            };


            foreach (var size in new int[] { 100, 1000, 10000, 100000 })
            {
                var array = BuildRandomArray(size).ToArray();

                var sw = new Stopwatch();

                foreach (var search in searches)
                {
                    int iterationCountSum = 0;
                    int comparisonCountSum = 0;
                    long elapsedTicksSum = 0;
                    long millisecondsSum = 0;

                    var arrayCopy = new int[array.Length];
                    array.CopyTo(arrayCopy, 0);

                    foreach (var element in arrayCopy.Reverse())
                    {

                        sw.Start();
                        var result = search.Search(array, element);
                        sw.Stop();


                        iterationCountSum += result.IterationCount;
                        comparisonCountSum += result.ComparisonCount;
                        elapsedTicksSum += sw.ElapsedTicks;
                        millisecondsSum += sw.ElapsedMilliseconds;

                        sw.Reset();
                    }

                    Console.WriteLine($"{search.GetType().Name}:");
                    Console.WriteLine($"Array length: {array.Length}");
                    Console.WriteLine($"Average iteration count: {(double)iterationCountSum / array.Length}");
                    Console.WriteLine($"Average comparison count: {(double)comparisonCountSum / array.Length}");
                    Console.WriteLine($"Average time: {(double)elapsedTicksSum / array.Length} ticks / {(double)millisecondsSum / array.Length} ms");

                    sw.Start();
                    var individualResult = search.Search(array, 190);
                    sw.Stop();

                    Console.WriteLine($"Key: {190}");
                    Console.WriteLine($"Index: {individualResult.Index}");
                    Console.WriteLine($"Iteration count: {individualResult.IterationCount}");
                    Console.WriteLine($"Comparison count: {individualResult.ComparisonCount}");
                    Console.WriteLine($"Time: {sw.ElapsedTicks} ticks / {sw.ElapsedMilliseconds} ms.\n");

                    sw.Reset();
                }
            }
        }
    }
}
