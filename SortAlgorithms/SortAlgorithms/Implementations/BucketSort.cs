using SortAlgorithms.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SortAlgorithms.Implementations
{
    public class BucketSort : ISort
    {
        public IEnumerable<int> Sort(IEnumerable<int> source)
        {
            var array = source.ToArray();
            var sortedArray = new List<int>();
            var innerSort = new InsertSort();
            var minValue = array[0];
            var maxValue = array[0];
            var bucketsCount = array.Length / 100;

            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] > maxValue)
                    maxValue = array[i];
                if (array[i] < minValue)
                    minValue = array[i];
            }

            var buckets = new List<int>[bucketsCount];

            for (int i = 0; i < buckets.Length; i++)
            {
                buckets[i] = new List<int>();
            }

            for (int i = 0; i < array.Length; i++)
            {
                var index = Math.Min(bucketsCount - 1, Math.Abs(bucketsCount * (array[i] - minValue) / (maxValue - minValue)));
                buckets[index].Add(array[i]);
            }

            for (int i = 0; i < buckets.Length; i++)
            {
                var temp = innerSort.Sort(buckets[i]);
                sortedArray.AddRange(temp);
            }

            return sortedArray;
        }
    }
}
