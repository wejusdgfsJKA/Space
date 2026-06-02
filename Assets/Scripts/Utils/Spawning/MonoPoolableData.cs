using UnityEngine;

namespace Pooling
{
    public class MonoPoolableData<T> : ScriptableObject where T : MonoPoolable
    {
        public MonoPoolableManager.Keys Key;
        public T Prefab;
        public T GetInstance(Transform point)
        {
            var instance = GetInstance();
            instance.transform.SetPositionAndRotation(point.position, point.rotation);
            return instance;
        }
        public T GetInstance()
        {
            T instance;
            if (MonoPoolableManager.Instance.Get(Key, out var obj))
            {
                instance = obj as T;
            }
            else
            {
                instance = Instantiate(Prefab);
            }
            instance.Initialize(this);
            return instance;
        }
    }
}