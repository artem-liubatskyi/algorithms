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

            foreach (var student in testData)
            {
                hashTable.Add(student, student);
            }

            Console.WriteLine($"\nCollision count: { hashTable.CollisionsCount }\n");
            Console.WriteLine(hashTable.ToString());
            Console.Write($"======== Key: Search result ======== \n");

            testData.AddRange(new string[] { "АБВГДЖ", "АААЯЯЯ" });

            foreach (var key in testData)
            {
                var (value, collisions) = hashTable.Get(key);

                Console.WriteLine($"{ key }: Value => { value ?? "not found"} - Collisions => {collisions}");
            }
            Console.ReadKey();
        }

        private static List<string> testData = new List<string>(){};
    }
}
