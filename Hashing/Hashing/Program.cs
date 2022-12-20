using Hashing.Implementation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hashing
{
    public class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            var hashTable = new HashTable<string>(29);

            foreach (var student in TestData)
            {
                hashTable.Add(student, student);
            }

            Console.WriteLine($"\nCollision count: { hashTable.CollisionsCount }\n");
            Console.WriteLine(hashTable.ToString());
            Console.Write($"======== Key: Search result ======== \n");

            TestData.AddRange(new string[] { "Missing", "Test" });

            foreach (var key in TestData)
            {
                var (value, collisions) = hashTable.Get(key);

                Console.WriteLine($"{ key }: Value => { value ?? "not found"} - Collisions => {collisions}");
            }
            Console.ReadKey();
        }

        private static readonly List<string> TestData = new List<string>() { };
    }
}
