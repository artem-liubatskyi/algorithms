using SearchAlgorithms.Interfaces;
using SearchAlgorithms.Models;

namespace SearchAlgorithms.Implementations
{
    public class BinarySearch : ISearch
    {
        public SearchResult Search(int[] source, int key)
        {
            int left = 0;
            int right = source.Length - 1;
            int iterationCount = 0;
            int comparisonCount = 0;

            while (left <= right && source[left] <= key && key <= source[right])
            {
                iterationCount++;

                comparisonCount++;
                if (left == right || source[left] == source[right])
                {
                    return new SearchResult { Index = left, IterationCount = iterationCount, ComparisonCount = comparisonCount };
                }

                int mid = left + (right - left) / 2;

                comparisonCount++;
                if (source[mid] == key)
                {
                    return new SearchResult { Index = mid, IterationCount = iterationCount, ComparisonCount = comparisonCount };
                }

                comparisonCount++;
                if (source[mid] < key)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }
            return new SearchResult { Index = left, IterationCount = iterationCount, ComparisonCount = comparisonCount };
        }
    }
}
