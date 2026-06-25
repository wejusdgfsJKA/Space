using System.Collections.Generic;


namespace Spawning
{
    public class ObjectPool<Key, Object>
    {
        protected readonly Dictionary<Key, Queue<Object>> pool = new();

        public void Release(Key key, Object obj)
        {
            if (!pool.ContainsKey(key)) pool[key] = new Queue<Object>();
            pool[key].Enqueue(obj);
        }

        /// <summary>
        /// Tries to get an object out of the pool. Will NOT create a new object if the 
        /// pool is empty, use MonoPoolableData for that.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
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