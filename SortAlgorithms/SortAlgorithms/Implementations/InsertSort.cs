using SortAlgorithms.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SortAlgorithms.Implementations
{
    public class InsertSort : ISort
    {
        public IEnumerable<int> Sort(IEnumerable<int> source)
        {
            var array = source.ToArray();

            for (int i = 1; i < array.Length; i++)
            {
                var currentValue = array[i];
                int pointer = i - 1;

                while (pointer >= 0 && array[pointer] > currentValue)
                {
                    array[pointer + 1] = array[pointer];
                    pointer -= 1;
                }
                array[pointer + 1] = currentValue;
            }

            return array;
        }
    }
}
