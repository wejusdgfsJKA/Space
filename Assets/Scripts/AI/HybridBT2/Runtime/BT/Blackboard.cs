using System.Collections.Generic;
using UnityEngine;
namespace HybridBT2
{
    public class Blackboard : IResettable
    {
        public enum Keys
        {
            Target,
            Launchers
        }
        public readonly Transform Transform;
        public readonly Navigation Navigation;
        /// <summary>
        /// Time since last tree tick.
        /// </summary>
        public float DeltaTime { get; protected set; }
        protected Dictionary<Keys, object> data = new();
        protected float lastTime;
        public void PerformReset()
        {
            data.Clear();
            lastTime = Time.time;
            DeltaTime = 0;
        }
        public void Tick()
        {
            DeltaTime = Time.time - lastTime;
            lastTime = Time.time;
        }
        public Blackboard(Transform transform)
        {
            Transform = transform;
            Navigation = transform.GetComponent<Navigation>();
            DeltaTime = 0;
            lastTime = Time.time;
        }
        public void SetData<R>(Keys key, R value) => data[key] = value;
        public R GetData<R>(Keys key)
        {
            return data.TryGetValue(key, out var value) ? (R)value : default;
        }
    }
}