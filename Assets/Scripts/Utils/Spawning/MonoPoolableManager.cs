using UnityEngine;
using Utilities;

namespace Pooling
{
    public class MonoPoolableManager : Singleton<MonoPoolableManager>
    {
        public enum Keys
        {
            MissileBasic,
            Bandit1
        }
        readonly ObjectPool<Keys, MonoBehaviour> pool = new();
        public void Release(Keys key, MonoBehaviour obj)
        {
            pool.Release(key, obj);
        }
        public bool Get(Keys key, out MonoBehaviour obj)
        {
            return pool.Get(key, out obj);
        }
    }
}