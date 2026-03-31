using UnityEngine;
using Utilities;
namespace Player
{
    public class PlayerShipController : MonoBehaviour, GlobalUpdater.IUpdatable
    {
        #region Fields
        float currentXAngle;
        float currentYAngle;
        [SerializeField] Transform shipBody, cam, camPivot;
        [SerializeField] float rotSpeed = 30;
        [SerializeField] private InputReader inputReader;
        [Header("Thrust")]
        [SerializeField] float forwardThrust = 1, backwardThrust = 1, strafeThrust = 1, maxVelocityMagnitude = 10;
        Quaternion rotation;
        Vector3 movementVector;
        bool freeLook;
        #endregion
        private void OnEnable()
        {
            freeLook = false;
            inputReader.EnablePlayerActions();
            inputReader.MoveEvent += OnMove;
            inputReader.RollEvent += OnRoll;
            inputReader.FreeLookToggleEvent += OnFreeLookToggle;
            GlobalUpdater.Instance.Register(this);
            currentXAngle = camPivot.localRotation.eulerAngles.x;
            currentYAngle = camPivot.localRotation.eulerAngles.y;
        }
        private void OnDisable()
        {
            inputReader.DisablePlayerActions();
            GlobalUpdater.Instance.Unregister(this);
        }
        private void OnMove(Vector3 movementVector)
        {
            this.movementVector = movementVector;
        }
        void OnRoll(float rollValue)
        {
            shipBody.Rotate(Vector3.forward, rollValue * rotSpeed * Time.deltaTime);
        }

        void OnFreeLookToggle(float value)
        {
            freeLook = value > 0.5f;
        }

        public void PerformUpdate(float deltaTime)
        {
            #region Movement
            Vector3 velocity = deltaTime * (movementVector.z * (movementVector.z > 0 ? forwardThrust : backwardThrust) * shipBody.forward +
                movementVector.x * strafeThrust * shipBody.right +
                movementVector.y * strafeThrust * shipBody.up);

            Debug.Log($"Velocity: {movementVector}");
            #endregion

            #region Rotation

            #endregion
        }
    }
}
