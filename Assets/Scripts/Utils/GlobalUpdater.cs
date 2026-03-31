using System;
using UnityEngine;
namespace Utilities
{

    public sealed class GlobalUpdater : MonoBehaviour
    {
        public interface IUpdatable
        {
            void PerformUpdate(float deltaTime);
        }
        Action<float> update = delegate { };
        public static GlobalUpdater Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        public void Register(IUpdatable updatable) => update += updatable.PerformUpdate;
        public void Unregister(IUpdatable updatable) => update -= updatable.PerformUpdate;
        private void Update()
        {
            update(Time.deltaTime);
        }
    }
}