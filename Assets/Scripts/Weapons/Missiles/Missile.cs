using HP;
using UnityEngine;
using Utilities;
namespace Weapons
{
    [RequireComponent(typeof(HPComponent))]
    public class Missile : Bullet, IObject
    {
        public float Signature => 0;
        protected IObject target;
        public IObject Target
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
        protected float initialTracking, topSpeed, acceleration, maxTargetSignature;
        protected LayerMask collisionMask = 1 << 0 | 1 << 6;
        public int Index { get; set; }
        protected Vector3 linearVelocity;
        public Vector3 LinearVelocity => linearVelocity;
        protected Vector3 angularVelocity;
        public Vector3 AngularVelocity => Vector3.zero;
        protected HPComponent hpComponent;
        public float EffectiveTracking { get; protected set; }
        protected override void Awake()
        {
            base.Awake();
        }
        protected float Telemetry()
        {
            if (Owner == null) return 0;
            return Mathf.Max(0, Vector3.Dot(Owner.forward, (target.Transform.position -
                Owner.position).normalized));
        }
        public override void Initialize(BulletData poolableData)
        {
            base.Initialize(poolableData);
            if (poolableData is not MissileData missileData)
            {
                throw new System.ArgumentException($"Expected {typeof(MissileData)}, got {poolableData.GetType()}");
            }
            initialTracking = missileData.Tracking;
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

            maxTargetSignature = missileData.MaxTargetSignature;
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            linearVelocity = Owner.GetLinearVelocity();

            if (Owner != null) gameObject.layer = GlobalConfig.
                    GetBulletLayer(Owner.gameObject.layer);
            else gameObject.layer = GlobalConfig.GetBulletLayer(0);
            collisionMask = GlobalConfig.GetBulletCollisionMask(gameObject.layer);

            angularVelocity = Vector3.zero;
        }
        protected override void OnDisable()
        {
            Target = null;
            base.OnDisable();
        }
        protected void CalculateEffectiveTracking()
        {
            if (Target == null)
            {
                EffectiveTracking = 0;
                return;
            }

            EffectiveTracking = initialTracking * (1 + Telemetry()) *
                Mathf.Min(1, target.Signature / maxTargetSignature);
        }
        protected override void PerformUpdate(float dt)
        {
            base.PerformUpdate(dt);
            if (target != null && target.Transform.gameObject.activeInHierarchy == false)
            {
                Target = null;
                EffectiveTracking = 0;
            }

            if (Physics.Raycast(tr.position, linearVelocity, out RaycastHit hit, linearVelocity.magnitude * dt, collisionMask))
            {
                OnHit(hit.collider);
            }
            else
            {
                if (Target != null)
                {
                    CalculateEffectiveTracking();

                    Vector3 directionToTarget = (Target.Transform.position - tr.position).normalized;
                    Vector3 newDirection = Vector3.RotateTowards(tr.forward, directionToTarget,
                        EffectiveTracking * dt, 0);

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