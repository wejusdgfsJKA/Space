using UnityEngine;
namespace Utilities
{
    public static class Extensions
    {
        public static Vector3 Perpendicular(this Vector3 inputVector, bool normalize = true)
        {
            if (inputVector == Vector3.zero) return Vector3.zero;
            Vector3 v;
            if (inputVector.z != 0)
            {
                v = new Vector3(1, 1, -(inputVector.x + inputVector.y) / inputVector.z);
            }
            else if (inputVector.x != 0)
            {
                v = new Vector3(-(inputVector.y + inputVector.z) / inputVector.x, 1, 1);
            }
            else
            {
                v = new Vector3(1, -(inputVector.x + inputVector.z) / inputVector.y, 1);
            }
            return normalize ? v.normalized : v;
        }

        #region Transform
        public static T GetComponentViaRegister<T>(this Transform transform) where T : Component
        {
            return ComponentRegister<T>.Get(transform);
        }
        public static T GetOrAddComponent<T>(this Transform transform) where T : Component
        {
            T component = ComponentRegister<T>.Get(transform);
            if (component == null)
            {
                component = transform.GetComponent<T>();
                if (component == null)
                {
                    component = transform.gameObject.AddComponent<T>();
                }
                ComponentRegister<T>.Register(transform, component);
            }
            return component;
        }
        public static Vector3 GetLinearVelocity(this Transform transform)
        {
            IVelocityProvider velocityProvider = ComponentRegister<IVelocityProvider>.Get(transform);
            return velocityProvider != null ? velocityProvider.LinearVelocity : Vector3.zero;
        }
        public static Vector3 GetAngularVelocity(this Transform transform)
        {
            IVelocityProvider velocityProvider = ComponentRegister<IVelocityProvider>.Get(transform);
            return velocityProvider != null ? velocityProvider.AngularVelocity : Vector3.zero;
        }
        public static (Vector3 linear, Vector3 angular) GetVelocities(this Transform transform)
        {
            IVelocityProvider velocityProvider = ComponentRegister<IVelocityProvider>.Get(transform);
            return velocityProvider != null ? (velocityProvider.LinearVelocity, velocityProvider.AngularVelocity) : (Vector3.zero, Vector3.zero);
        }
        #endregion
    }
}