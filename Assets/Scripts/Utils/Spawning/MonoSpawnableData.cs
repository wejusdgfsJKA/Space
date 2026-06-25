using UnityEngine;

namespace Spawning
{
    public abstract class MonoSpawnableData<T>: ScriptableObject where T : MonoBehaviour
    {
        public T Prefab;
        public virtual T CreateInstance()
        {
            return Instantiate(Prefab);
        }
    }
}