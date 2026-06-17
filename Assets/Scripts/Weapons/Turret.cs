using UnityEngine;
using Utilities;
namespace Weapons
{
    public class Turret : MonoBehaviour
    {
        public Transform Target;
        protected Transform tr;
        public Transform Transform => tr != null ? tr : transform;
        [field: SerializeField] public Transform RotationPivot { get; protected set; }
        [field: SerializeField] public Transform ElevationPivot { get; protected set; }
        [SerializeField] protected float rotSpeed = 50;
        [SerializeField] protected float minElevation = 0, maxElevation = 90, minRot = -190, maxRot = 190;
        [SerializeField] protected float maxErrorAngle = 1;
        protected void Awake()
        {
            tr = transform;
            if (RotationPivot == null) RotationPivot = tr;
            if (ElevationPivot == null) ElevationPivot = tr;
        }
        private void Update()
        {
            Tick(Time.deltaTime);
        }
        public void Tick(float deltaTime)
        {
            if (Target != null)
            {
                LookAt(Target.position, deltaTime);
                if (Vector3.Angle(ElevationPivot.forward, (Target.position -
                    Transform.position)) < maxErrorAngle)
                {
                    IncreaseReadiness(deltaTime);
                    return;
                }
            }
            DecreaseReadiness(deltaTime);
        }
        protected void IncreaseReadiness(float deltaTime)
        {

        }
        protected void DecreaseReadiness(float deltaTime)
        {

        }
        protected void LookAt(Vector3 position, float deltaTime)
        {
            Vector3 direction = position - Transform.position;
            if (direction.sqrMagnitude > 0.01)
            {
                if (RotationPivot == ElevationPivot)
                {
                    RotateUnified(direction.normalized, deltaTime);
                }
                else RotateSeparate(direction.normalized, deltaTime);
            }
        }
        protected void RotateUnified(Vector3 direction, float deltaTime)
        {
            var newRotation = Quaternion.LookRotation(direction, Transform.up);
            var targetRot = Quaternion.RotateTowards(RotationPivot.rotation,
                newRotation, deltaTime * rotSpeed);

            RotationPivot.rotation = targetRot;
            var targetYaw = Mathf.Clamp(Extensions.NormalizeAngle(RotationPivot.
                localRotation.eulerAngles.y), minRot, maxRot);
            var targetPitch = Mathf.Clamp(Extensions.NormalizeAngle(RotationPivot.
                localRotation.eulerAngles.x), -maxElevation, -minElevation);

            RotationPivot.localRotation = Quaternion.Euler(targetPitch, targetYaw, 0);
        }
        protected void RotateSeparate(Vector3 direction, float deltaTime)
        {
            var newRotation = Quaternion.LookRotation(direction, Transform.up);

            var horizTargetRot = Quaternion.RotateTowards(RotationPivot.rotation,
                newRotation, deltaTime * rotSpeed);
            RotationPivot.rotation = horizTargetRot;
            var targetYaw = Mathf.Clamp(Extensions.NormalizeAngle(RotationPivot.
                localRotation.eulerAngles.y), minRot, maxRot);
            RotationPivot.localRotation = Quaternion.Euler(0, targetYaw, 0);

            //newRotation = Quaternion.LookRotation(direction, Transform.up);
            var elevTargetRot = Quaternion.RotateTowards(ElevationPivot.rotation,
                newRotation, deltaTime * rotSpeed);
            ElevationPivot.rotation = elevTargetRot;
            var targetPitch = Mathf.Clamp(Extensions.NormalizeAngle(ElevationPivot.localRotation.eulerAngles.x), -maxElevation, -minElevation);
            //var targetPitch=Mathf.Clamp(ElevationPivot.localRotation.x,)
            ElevationPivot.localRotation = Quaternion.Euler(targetPitch, 0, 0);
        }
    }
}