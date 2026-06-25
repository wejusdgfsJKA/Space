using EventBus;
using UnityEngine;

namespace Spawning
{
    public abstract class MonoPoolableData<K, T> : MonoSpawnableData<T> where T : MonoBehaviour
    {
        public K Key;

        public T GetInstance(Transform point) => GetInstance(point.position, point.rotation);

        public T GetInstance(Vector3 position, Quaternion rotation)
        {
            var instance = GetInstance();
            instance.transform.SetPositionAndRotation(position, rotation);
            EventBus<PoolableRecycled>.Raise(new());
            return instance;
        }

        protected abstract T GetInstance();
    }
}