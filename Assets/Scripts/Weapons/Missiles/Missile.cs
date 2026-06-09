using HP;
using Pooling;
using UnityEngine;
using Utilities;
namespace Weapons
{
    [RequireComponent(typeof(HPComponent))]
    public class Missile : Bullet, IVelocityProvider
    {
        protected Transform target;
        public Transform Target
        {
            get
            {
                return target;
            }
            set
            {
                if (target != value)
                {
                    if (target != null) MissileManager.Unregister(this);
                    target = value;
                    if (target != null) MissileManager.Register(this);
                }
            }
        }
        protected float tracking, topSpeed, acceleration;
        protected LayerMask collisionMask = 1 << 0 | 1 << 6;
        public int Index { get; set; }
        protected Vector3 linearVelocity;
        public Vector3 LinearVelocity => linearVelocity;
        protected Vector3 angularVelocity;
        public Vector3 AngularVelocity => Vector3.zero;
        protected HPComponent hpComponent;
        public override void Initialize<T>(MonoPoolableData<T> poolableData)
        {
            base.Initialize(poolableData);
            if (poolableData is not MissileData missileData)
            {
                throw new System.ArgumentException($"Expected {typeof(MissileData)}, got {poolableData.GetType()}");
            }
            tracking = missileData.Tracking;
            topSpeed = missileData.TopSpeed;
            if (topSpeed <= 0)
            {
                Debug.LogWarning($"Missile top speed set to 0 or lower on {transform}");
                if (topSpeed < 0) topSpeed = 0;
            }
            acceleration = missileData.Acceleration;
            if (acceleration <= 0)
            {
                Debug.LogWarning($"Missile acceleration should not be 0. Setting acceleration to 10% of top speed on {transform}.");
                acceleration = topSpeed / 10;
            }
            if (hpComponent == null) hpComponent = GetComponent<HPComponent>();
            hpComponent.MaxHP = missileData.HP;
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            linearVelocity = Owner.GetLinearVelocity();
            angularVelocity = Vector3.zero;
        }
        protected override void OnDisable()
        {
            Target = null;
            base.OnDisable();
        }
        protected override void PerformUpdate(float dt)
        {
            base.PerformUpdate(dt);
            if (target != null && target.gameObject.activeInHierarchy == false)
            {
                Target = null;
            }

            if (Physics.Raycast(tr.position, linearVelocity, out RaycastHit hit, linearVelocity.magnitude * dt, collisionMask))
            {
                OnHit(hit.collider);
            }
            else
            {
                if (Target != null)
                {
                    Vector3 directionToTarget = (Target.position - tr.position).normalized;
                    Vector3 newDirection = Vector3.RotateTowards(tr.forward, directionToTarget,
                        tracking * dt, 0);

                    Quaternion prevRotation = tr.rotation;
                    tr.rotation = Quaternion.LookRotation(newDirection);

                    Quaternion rotationDelta = (transform.rotation * Quaternion.Inverse(prevRotation)).normalized;
                    rotationDelta.ToAngleAxis(out float angleInDegrees, out Vector3 axis);
                    if (angleInDegrees > 180f) angleInDegrees -= 360f;

                    angularVelocity = axis * (angleInDegrees * Mathf.Deg2Rad) / dt;
                }
                linearVelocity += acceleration * dt * tr.forward;
                linearVelocity = Vector3.ClampMagnitude(linearVelocity, topSpeed);
                tr.position += dt * linearVelocity;
            }
        }
    }
}