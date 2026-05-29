using UnityEngine;
namespace Utilities
{
    public class RetainPositionAndRotation : MonoBehaviour
    {
        Vector3 initialPosition;
        Quaternion initialRotation;
        private void OnEnable()
        {
            transform.SetPositionAndRotation(initialPosition, initialRotation);
            GlobalUpdater.Instance.RegisterUpdate(Retain);
        }
        private void OnDisable()
        {
            GlobalUpdater.Instance.UnregisterUpdate(Retain);
        }
        void Retain(float _)
        {
            transform.SetPositionAndRotation(initialPosition, initialRotation);
        }
        public void UpdateValues()
        {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }
    }
}