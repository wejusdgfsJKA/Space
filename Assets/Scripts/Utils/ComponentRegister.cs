using System.Collections.Generic;
using UnityEngine;
namespace Utilities
{
    public interface IRegisterableComponent
    {

    }
    public static class ComponentRegister<T> where T : class, IRegisterableComponent
    {
        static readonly Dictionary<Transform, T> storage = new();
        public static T Get(Transform transform)
        {
            if (transform == null)
            {
                Debug.LogError("Transform is null. Cannot get component.");
                return null;
            }
            return storage.TryGetValue(transform, out T instance) ? instance : null;
        }
        public static bool Register(Transform transform, T component)
        {
            if (transform == null || component == null)
            {
                Debug.LogError("Transform or component is null. Cannot register.");
                return false;
            }
            return storage.TryAdd(transform, component);
        }
        public static void Unregister(Transform transform)
        {
            if (transform == null)
            {
                Debug.LogError("Transform is null. Cannot unregister component.");
                return;
            }
            storage.Remove(transform);
        }
    }
}