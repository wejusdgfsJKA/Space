using EventBus;
using UnityEngine;

namespace Pooling
{
    public abstract class MonoPoolableData<K, T> : ScriptableObject where T : MonoBehaviour
    {
        public K Key;
        public T Prefab;

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