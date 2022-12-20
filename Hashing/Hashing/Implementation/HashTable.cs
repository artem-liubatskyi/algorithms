using Hashing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hashing.Implementation
{

    public class HashTable<TValue>
    {
        private KeyValue<string, TValue>[] Collecction { get; set; }

        public HashTable(long m)
        {
            Collecction = new KeyValue<string, TValue>[m];
        }

        public long CollisionsCount { get; private set; }

        public override string ToString()
        {
            StringBuilder returnString = new StringBuilder("---------- Index => Key => Hash => Value ----------\n");
            int i = 0;
            foreach (var value in Collecction)
            {
                if (value != null)
                {
                    returnString.Append($"[{ i }] => { value.Key} => { value.Hash.ToString()} => { value.Value} \n");
                }

                i++;
            }
            return returnString.ToString();
        }

        public (TValue, int) Get(string key)
        {
            var (locationIndex, collisions) = FindLocation(GetHash(key), key);
            if (locationIndex != -1)
            {
                return (Collecction[locationIndex].Value, collisions);
            }
            return (default, 0);
        }

        public void Add(string key, TValue value)
        {
            var hash = GetHash(key);
            var locationIndex = FindNextLocation(hash);

            if (locationIndex != -1)
            {
                Collecction[locationIndex] = new KeyValue<string, TValue>(key, value, hash);
            }
        }

        public long GetHash(string key)
        {
            var keyBytes = Encoding.Unicode.GetBytes(key.Substring(0, Math.Min(12, key.Length)));

            var devidedBytes = new List<byte>();
            for (int i = 0; i < keyBytes.Length; i++)
            {
                devidedBytes.Add((byte)(keyBytes[i] % Collecction.Length));
            }

            return BitConverter.ToInt64(devidedBytes.ToArray()) % Collecction.Length;
        }

        private long FindNextLocation(long index)
        {
            bool found = false;
            long i = 0;
            while (!found && i < Collecction.Length)
            {
                if (Collecction[index] == null)
                {
                    found = true;
                }
                else
                {
                    index = (long)((index + 0.5 * i + 0.5 * Math.Pow(i, 2)) % Collecction.Length);
                    CollisionsCount++;
                    i++;
                }
            }

            return found ? index : -1;
        }

        private (long, int) FindLocation(long index, string key)
        {
            bool found = false;
            long i = 0;
            int collisionsCount = 0;
            while (!found && Collecction[index] != null && i < Collecction.Length)
            {
                if (Collecction[index] != null && key == Collecction[index].Key)
                {
                    found = true;
                }
                else
                {
                    index = (long)((index + 0.5 * i + 0.5 * Math.Pow(i, 2)) % Collecction.Length);
                    collisionsCount++;
                    i++;
                }
            }

            return found ? (index, collisionsCount) : (-1, collisionsCount);
        }
    }
}
