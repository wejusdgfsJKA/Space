using System.Collections.Generic;

namespace Pooling
{
    public class ObjectPool<Key, Object>
    {
        protected readonly Dictionary<Key, Queue<Object>> pool = new();
        public void Release(Key key, Object obj)
        {
            if (!pool.ContainsKey(key)) pool[key] = new Queue<Object>();
            pool[key].Enqueue(obj);
        }
        public bool Get(Key key, out Object obj)
        {
            if (!pool.ContainsKey(key) || pool[key].Count == 0)
            {
                obj = default;
                return false;
            }
            obj = pool[key].Dequeue();
            return true;
        }
        public void Clear()
        {
            pool.Clear();
        }
    }
}