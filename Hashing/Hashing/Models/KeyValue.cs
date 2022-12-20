namespace Hashing.Models
{
    public class KeyValue<TKey, TValue>
    {
        public KeyValue(TKey key, TValue value, long hash)
        {
            Key = key;
            Value = value;
            Hash = hash;
        }

        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public long Hash { get; set; }
    }
}
