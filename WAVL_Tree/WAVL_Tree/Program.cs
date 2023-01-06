using System;
using System.Collections.Generic;
using System.Linq;
using WAVL_Tree.Implementations;
using WAVL_Tree.Interfaces;
using WAVL_Tree.Models;

namespace WAVL_Tree
{
    public class Program
    {
        public static void Main()
        {
            IBinaryTree<string> avlTree;
            IBinaryTree<string> bstTree;

            double avlAverageSearchNodeCount = 0;
            double avlAverageInsertNodeCount = 0;
            double avlAverageDeleteNodeCount = 0;
            double avlAverageInsertRotationCount = 0;
            double avlAverageDeleteRotationCount = 0;

            double simpleAverageSearchNodeCount = 0;
            double simpleAverageInsertNodeCount = 0;
            double simpleAverageDeleteNodeCount = 0;
            double simpleAverageInsertRotationCount = 0;
            double simpleAverageDeleteRotationCount = 0;

            for (int i = 0; i < 10; i++)
            {
                avlTree = new WAVLTree<string>();
                bstTree = new BSTree<string>();

                Console.WriteLine($"-- WAVL Tree - {i + 1} --");
                var avlTreeResult = GetResult(avlTree, false);

                avlAverageSearchNodeCount += avlTreeResult.Item1;
                avlAverageInsertNodeCount += avlTreeResult.Item2;
                avlAverageDeleteNodeCount += avlTreeResult.Item3;
                avlAverageInsertRotationCount += avlTreeResult.Item4;
                avlAverageDeleteRotationCount += avlTreeResult.Item5;

                Console.WriteLine($"-- BS Tree - {i + 1} --");
                var simpleTreeResult = GetResult(bstTree, false);

                simpleAverageSearchNodeCount += simpleTreeResult.Item1;
                simpleAverageInsertNodeCount += simpleTreeResult.Item2;
                simpleAverageDeleteNodeCount += simpleTreeResult.Item3;
                simpleAverageInsertRotationCount += simpleTreeResult.Item4;
                simpleAverageDeleteRotationCount += simpleTreeResult.Item5;
            }

            Console.WriteLine($"-- WAVL Tree --");
            Console.WriteLine($"Average search node count: {avlAverageSearchNodeCount / 10}");
            Console.WriteLine($"Average insert node count: {avlAverageInsertNodeCount / 10}");
            Console.WriteLine($"Average delete node count: {avlAverageDeleteNodeCount / 10}");
            Console.WriteLine($"Average insert rotation count: {avlAverageInsertRotationCount / 10}");
            Console.WriteLine($"Average delete rotation count: {avlAverageDeleteRotationCount / 10}");

            Console.WriteLine($"-- BS Tree --");
            Console.WriteLine($"Average search node count: {simpleAverageSearchNodeCount / 10}");
            Console.WriteLine($"Average insert node count: {simpleAverageInsertNodeCount / 10}");
            Console.WriteLine($"Average delete node count: {simpleAverageDeleteNodeCount / 10}");
            Console.WriteLine($"Average insert rotation count: {simpleAverageInsertRotationCount / 10}");
            Console.WriteLine($"Average delete rotation count: {simpleAverageDeleteRotationCount / 10}");

        }

        public static (double, double, double, double, double) GetResult(IBinaryTree<string> tree, bool canOutput)
        {
            int rndKey = 0;

            var rand = new Random();
            var memory = new List<int>();
            for (int i = 0; i < 1000000; i++)
            {
                rndKey = rand.Next();
                memory.Add(rndKey);
                tree.Insert(rndKey, "");
            }

            int searchNodeCount = 0;
            int insertNodeCount = 0;
            int deleteNodeCount = 0;
            int insertRotationCount = 0;
            int deleteRotationCount = 0;

            int insertCount = 0;
            int deleteCount = 0;
            int searchCount = 0;

            int rndOperation = -1;

            tree.NodeCounter += () =>
            {
                switch (rndOperation)
                {
                    case 1:
                        insertNodeCount++;
                        break;
                    case 2:
                        deleteNodeCount++;
                        break;
                    case 3:
                        searchNodeCount++;
                        break;
                    default:
                        break;
                }
            };

            tree.RotationCounter += (count) =>
            {
                switch (rndOperation)
                {
                    case 1:
                        insertRotationCount += count;
                        break;
                    case 2:
                        deleteRotationCount += count;
                        break;
                    default:
                        break;
                }
            };

            for (int i = 0; i < 200000; i++)
            {
                rndOperation = rand.Next(1, 4);
                switch (rndOperation)
                {
                    case 1:
                        rndKey = rand.Next();
                        tree.Insert(rndKey, "");
                        insertCount++;
                        break;
                    case 2:
                        var rndIndex = rand.Next(0, memory.Count);
                        tree.Delete(memory[rndIndex]);
                        memory.RemoveAt(rndIndex);
                        deleteCount++;
                        break;
                    case 3:
                        rndKey = rand.Next();
                        tree.Search(rndKey);
                        searchCount++;
                        break;
                }
            }

            if (canOutput)
            {
                Console.WriteLine($"Search node count: {searchNodeCount} searches count: {searchCount}");
                Console.WriteLine($"Insert node count: {insertNodeCount} inserts count: {insertCount}");
                Console.WriteLine($"Delete node count: {deleteNodeCount} deletes count: {deleteCount}");
                Console.WriteLine($"Insert rotation count: {insertRotationCount} inserts count: {insertCount}");
                Console.WriteLine($"Delete rotation count: {deleteRotationCount} deletes count: {deleteCount}");

                Console.WriteLine($"Average search node count: {(double)searchNodeCount / searchCount}");
                Console.WriteLine($"Average insert node count: {(double)insertNodeCount / insertCount}");
                Console.WriteLine($"Average delete node Count: {(double)deleteNodeCount / deleteCount}");
                Console.WriteLine($"Average insert rotation count: {(double)insertRotationCount / insertCount}");
                Console.WriteLine($"Average delete rotation count: {(double)deleteRotationCount / deleteCount}");
            }

            return ((double)searchNodeCount / searchCount,
                (double)insertNodeCount / insertCount,
                (double)deleteNodeCount / deleteCount,
                (double)insertRotationCount / insertCount,
                (double)deleteRotationCount / deleteCount);
        }

        static bool ValidateBinaryStructure(Node<string> n)
        {
            if (n == null)
            {
                return true;
            }
            if (n.Right != null)
            {
                foreach (WAVLNode<string> d in GetDecendents(n.Right))
                {
                    if (d.Key <= n.Key)
                    {
                        Console.WriteLine("Invalid place for key " + d.Key + " <= " + n.Key);
                        return false;
                    }
                }
            }
            if (n.Left != null)
            {
                foreach (WAVLNode<string> d in GetDecendents(n.Left))
                {
                    if (d.Key >= n.Key)
                    {
                        Console.WriteLine("Invalid place for key " + d.Key + " >= " + n.Key);
                        return false;
                    }
                }
            }

            return (ValidateBinaryStructure(n.Left) &&
                ValidateBinaryStructure(n.Right));
        }

        static List<Node<string>> GetDecendents(Node<string> n)
        {
            List<Node<string>> nodes = new List<Node<string>>();
            if (n.Left != null)
            {
                nodes.Add(n.Left);
                nodes.AddRange(GetDecendents(n.Left));
            }
            if (n.Right != null)
            {
                nodes.Add(n.Right);
                nodes.AddRange(GetDecendents(n.Right));
            }
            return nodes;
        }
    }
}