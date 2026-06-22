using UnityEngine;
namespace Utilities
{
    public class PersistentSingleton<T> : MonoBehaviour where T : Component
    {
        public bool AutoUnparentOnAwake = true;

        protected static T instance;

        public static bool HasInstance => instance != null;
        /// <summary>
        /// Returns the singleton instance. If null and createOnMissing is true, 
        /// will create a new instance. Should be called with false in OnDisable/OnDestroy, 
        /// otherwise we create a new object from OnDestroy, and that's cringe.
        /// </summary>
        /// <param name="createOnMissing">Iftrue, a new instance will be automatically 
        /// generated if none is found.</param>
        /// <returns>The singleton instance, if it exists or was generated.</returns>
        public static T TryGetInstance(bool createOnMissing = false)
        {
            if (HasInstance) return instance;
            if (!createOnMissing) return null;

            instance = FindAnyObjectByType<T>();
            if (instance == null)
            {
                var go = new GameObject(typeof(T).Name + " Auto-Generated");
                instance = go.AddComponent<T>();
            }
            return instance;
        }

        /// <summary>
        /// Make sure to call base.Awake() in override if you need awake.
        /// </summary>
        protected virtual void Awake()
        {
            InitializeSingleton();
        }

        protected virtual void InitializeSingleton()
        {
            if (!Application.isPlaying) return;

            if (AutoUnparentOnAwake)
            {
                transform.SetParent(null);
            }

            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                if (instance != this)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}