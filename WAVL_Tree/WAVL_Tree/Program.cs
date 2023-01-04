using System;
using System.Collections.Generic;
using WAVL_Tree.Implementations;
using WAVL_Tree.Models;

namespace WAVL_Tree
{
    class Program
    {
        public static void Main(string[] args)
        {
            testWAVL();
        }

        public static void testWAVL()
        {
            WAVLTree t = new WAVLTree();
            t.Insert(1, "test1");
            t.Insert(1, "test1");
            t.Insert(-1, "test-1");
            t.Insert(7, "test7");
            if ("test7" != t.Max())
            {
                return;
            }
            if (t.Min() != "test-1")
            {
                return;
            }
            if (t.Max() != "test7")
            {
                return;
            }
            t.Delete(1);
            t.Delete(-1);
            t.Delete(7);
            t.Delete(7);
            if (t.Min() != null)
            {
                return;
            }
            if (t.Max() != null)
            {
                return;
            }
            if (t.InfoToArray().Count != 0)
            {
                return;
            }


            fuzz(10000, true);
        }

        private static void fuzz(int count, bool validateStructure)
        {
            WAVLTree t = new WAVLTree();
            Random r = new Random();

            int maxOpsForInsert = 0;
            int totalOpsForInsert = 0;
            for (int i = 0; i < count; i++)
            {
                int randomKey = r.Next();
                int curr = t.Insert(randomKey, randomKey.ToString());

                if (curr > maxOpsForInsert)
                    maxOpsForInsert = curr;
                totalOpsForInsert += curr;

                if (validateStructure && !ValidateBinaryStructure(t.Root))
                {
                    Console.WriteLine("Tree failed validation during insert");
                    return;
                }
            }

            var keys = t.KeysToArray();
            int maxOpsForDelete = 0;
            int totalOpsForDelete = 0;

            foreach (int k in keys)
            {
                int curr = t.Delete(k);
                if (curr > maxOpsForDelete)
                    maxOpsForDelete = curr;
                totalOpsForDelete += curr;
            }

            float averageInsert = totalOpsForInsert / count;
            float averageDelete = totalOpsForDelete / count;

            Console.WriteLine("" + count + ", " + maxOpsForInsert + ", " + averageInsert
                                + ", " + maxOpsForDelete + ", " + averageDelete);

        }

        static bool ValidateBinaryStructure(WAVLNode n)
        {
            if (n == null)
            {
                return true;
            }
            if (n.Right != null)
            {
                foreach (WAVLNode d in GetDecendents(n.Right))
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
                foreach (WAVLNode d in GetDecendents(n.Left))
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

        static List<WAVLNode> GetDecendents(WAVLNode n)
        {
            List<WAVLNode> nodes = new List<WAVLNode>();
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
