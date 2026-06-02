using UnityEngine;

namespace Pooling
{
    public class MonoPoolable : MonoBehaviour
    {
        public MonoPoolableManager.Keys Key;
        public virtual void Initialize<T>(MonoPoolableData<T> data) where T : MonoPoolable
        {
            Key = data.Key;
        }
        protected virtual void OnDisable()
        {
            MonoPoolableManager.Instance.Release(Key, this);
        }
    }
}