using System;
using UnityEngine;
namespace Utilities
{
    public sealed class GlobalUpdater : MonoBehaviour
    {
        Action<float> update = delegate { }, lateUpdate = delegate { }, fixedUpdate = delegate { };
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
        private void OnDestroy()
        {
            update = fixedUpdate = lateUpdate = null;
            Instance = null;
        }
        public void RegisterUpdate(Action<float> action) => update += action;
        public void UnregisterUpdate(Action<float> action) => update -= action;
        public void RegisterLateUpdate(Action<float> action) => lateUpdate += action;
        public void UnregisterLateUpdate(Action<float> action) => lateUpdate -= action;
        public void RegisterFixedUpdate(Action<float> action) => fixedUpdate += action;
        public void UnregisterFixedUpdate(Action<float> action) => fixedUpdate -= action;

        private void Update()
        {
            update(Time.deltaTime);
        }
        private void LateUpdate()
        {
            lateUpdate(Time.deltaTime);
        }
        private void FixedUpdate()
        {
            fixedUpdate(Time.fixedDeltaTime);
        }
    }
}