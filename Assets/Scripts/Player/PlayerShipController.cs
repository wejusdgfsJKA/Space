using UnityEngine;
using Utilities;
namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerShipController : MonoBehaviour
    {
        #region Fields
        Vector3 currentShipBodyAngles;
        Quaternion baseRotation;
        float camXAngle, camYAngle;
        [SerializeField] float rotSpeed = 30;
        [SerializeField] private InputReader inputReader;
        [SerializeField] Transform shipBody;
        [Header("Camera")]
        [SerializeField] float xSens = 10;
        [SerializeField] float ySens = 10;
        [SerializeField] float freelookXSens = 10;
        [SerializeField] float freelookYSens = 10;
        [SerializeField] Vector3 defaultCamPos;
        [SerializeField] Transform cam;
        [SerializeField] Transform camPivot;
        [SerializeField] float zoomSpeed = 1;
        [Header("Thrust")]
        [SerializeField] float forwardThrust = 1;
        [SerializeField] float backwardThrust = 1;
        [SerializeField] float strafeThrust = 1;
        [SerializeField] float maxVelocityMagnitude = 10;
        Vector3 movementVector;
        bool freeLook;
        float rollInput;
        public bool FreeLook
        {
            get => freeLook;
            set
            {
                freeLook = value;
                //currentCameraAngles = CameraRotationPivot.localRotation.eulerAngles;
                if (!freeLook)
                {
                    camPivot.localRotation = Quaternion.identity;
                    currentShipBodyAngles = shipBody.localRotation.eulerAngles;
                }
                else
                {
                    camXAngle = camPivot.localRotation.eulerAngles.x;
                    camYAngle = camPivot.localRotation.eulerAngles.y;
                }
            }
        }
        public Vector3 LinearVelocity => rb != null ? rb.linearVelocity : Vector3.zero;
        protected float currentRoll;
        Rigidbody rb;
        #endregion

        #region Setup
        private void OnEnable()
        {
            baseRotation = shipBody.rotation;
            rb = GetComponent<Rigidbody>();
            rb.linearVelocity = Vector3.zero;
            currentRoll = 0;
            freeLook = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inputReader.EnablePlayerActions();
            inputReader.MoveEvent += OnMove;
            inputReader.RollEvent += OnRoll;
            inputReader.ZoomEvent += OnZoom;
            inputReader.ResetEvent += OnResetCamera;
            inputReader.FreeLookToggleEvent += OnFreeLookToggle;
            GlobalUpdater.TryGetInstance(true).RegisterFixedUpdate(PerformFixedUpdate);
            GlobalUpdater.TryGetInstance(true).RegisterUpdate(PerformUpdate);
            GlobalUpdater.TryGetInstance(true).RegisterLateUpdate(PerformLateUpdate);
            OnResetCamera();
            currentShipBodyAngles = shipBody.localRotation.eulerAngles;
        }
        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            inputReader.DisablePlayerActions();
            GlobalUpdater.TryGetInstance().UnregisterFixedUpdate(PerformFixedUpdate);
            GlobalUpdater.TryGetInstance().UnregisterUpdate(PerformUpdate);
            GlobalUpdater.TryGetInstance().UnregisterLateUpdate(PerformLateUpdate);
        }
        #endregion

        void OnZoom(float zoom)
        {
            var oldZ = cam.localPosition.z;
            cam.localPosition = new Vector3(cam.localPosition.x,
                cam.localPosition.y, oldZ + zoom * zoomSpeed);
        }
        void OnResetCamera()
        {
            cam.localPosition = defaultCamPos;
        }
        private void OnMove(Vector3 movementVector)
        {
            this.movementVector = movementVector;
        }
        void OnRoll(float rollValue)
        {
            rollInput = rollValue;
        }
        void OnFreeLookToggle(float value) => FreeLook = value > .5f;
        public void PerformFixedUpdate(float deltaTime)
        {
            Vector3 velocity = deltaTime * (movementVector.z * (movementVector.z > 0 ?
                forwardThrust : backwardThrust) * shipBody.forward +
                movementVector.x * strafeThrust * shipBody.right +
                movementVector.y * strafeThrust * shipBody.up);
            rb.AddForce(velocity, ForceMode.Acceleration);

            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxVelocityMagnitude);
            //Debug.Log(rb.linearVelocity);
        }
        public void PerformUpdate(float deltaTime)
        {
            if (freeLook)
            {
                currentShipBodyAngles += deltaTime * rotSpeed * new Vector3(0, 0, rollInput);
                shipBody.localRotation = Quaternion.Euler(currentShipBodyAngles);
            }
            else
            {
                currentShipBodyAngles += deltaTime * rotSpeed * new Vector3(inputReader.LookDelta.y * xSens, inputReader.LookDelta.x * ySens, rollInput);
                //var targetRotation = Quaternion.AngleAxis(deltaTime * rotSpeed * rollInput, shipBody.forward) *
                //    Quaternion.AngleAxis(inputReader.LookDelta.x * ySens * deltaTime * rotSpeed, shipBody.up) *
                //    Quaternion.AngleAxis(inputReader.LookDelta.y * xSens * deltaTime * rotSpeed, shipBody.right);
                //Debug.DrawLine(transform.position, transform.position + shipBody.right, Color.blue);
                //Debug.Log(inputReader.LookDelta.y * xSens * deltaTime * rotSpeed);
                //Debug.Log(currentShipBodyAngles);
                //shipBody.rotation = baseRotation * Quaternion.Euler(currentShipBodyAngles);
                shipBody.Rotate(inputReader.LookDelta.y * xSens * deltaTime * rotSpeed,
                    inputReader.LookDelta.x * ySens * deltaTime * rotSpeed,
                    rollInput * deltaTime * rotSpeed, Space.Self);
            }
        }
        public void PerformLateUpdate(float deltaTime)
        {
            if (freeLook)
            {
                camXAngle -= deltaTime * inputReader.LookDelta.y * freelookXSens;
                camYAngle += deltaTime * inputReader.LookDelta.x * freelookYSens;
                camPivot.localRotation = Quaternion.Euler(camXAngle, camYAngle, 0);
            }
        }
    }
}
