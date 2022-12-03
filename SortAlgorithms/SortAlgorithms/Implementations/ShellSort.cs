using SortAlgorithms.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SortAlgorithms.Implementations
{
    public class ShellSort : ISort
    {
        public IEnumerable<int> Sort(IEnumerable<int> source)
        {
            var array = source.ToArray();
            var intervals = GetTokudaIntervals(array.Length / 2).Reverse().ToArray();

            foreach (var interval in intervals)
            {
                for (int i = interval; i < array.Length; i++)
                {
                    var currentValue = array[i];
                    var j = i;
                    while (j >= interval && array[j - interval] > currentValue)
                    {
                        array[j] = array[j - interval];
                        j -= interval;
                    }
                    array[j] = currentValue;
                }
            }

            return array;
        }

        private IEnumerable<int> GetTokudaIntervals(int size)
        {
            var i = 0;
            double intervalSize;
            while (true)
            {
                intervalSize = Math.Ceiling((9 * Math.Pow(9.0 / 4.0, i) - 4) / 5);
                i++;
                if (intervalSize < size)
                {
                    yield return (int)intervalSize;
                }
                else
                {
                    break;
                }

            }
        }
    }
}
