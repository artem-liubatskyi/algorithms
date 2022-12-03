using SortAlgorithms.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SortAlgorithms.Implementations
{
    public class ShellSort : ISort
    {
        public IEnumerable<int> Sort(IEnumerable<int> source)
        {
            var array = source.ToArray();

            for (int interval = array.Length / 2; interval > 0; interval /= 2)
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
    }
}
