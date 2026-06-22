using HP;
using UnityEngine;
namespace Weapons
{
    [RequireComponent(typeof(HPComponent))]
    public class Missile : Projectile
    {
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
        protected float initialTracking, maxTargetSignature;
        public int Index { get; set; }
        protected HPComponent hpComponent;
        public float EffectiveTracking { get; protected set; }
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

            if (hpComponent == null) hpComponent = GetComponent<HPComponent>();
            hpComponent.MaxHP = missileData.HP;

            maxTargetSignature = missileData.MaxTargetSignature;
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
        protected override void RestOfPerformUpdate(float dt)
        {
            if (target != null && target.Transform.gameObject.activeInHierarchy == false)
            {
                Target = null;
                EffectiveTracking = 0;
            }
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
            base.PerformUpdate(dt);
            linearVelocity += acceleration * dt * tr.forward;
            linearVelocity = Vector3.ClampMagnitude(linearVelocity, topSpeed);
        }
    }
}