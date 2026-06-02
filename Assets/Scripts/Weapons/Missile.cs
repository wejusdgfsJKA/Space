using Pooling;
using UnityEngine;
namespace Weapons
{
    public class Missile : Bullet
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
        protected float tracking, speed;
        protected LayerMask collisionMask = 1 << 0 | 1 << 6;
        public int Index { get; set; }
        public override void Initialize<T>(MonoPoolableData<T> poolableData)
        {
            base.Initialize(poolableData);
            if (poolableData is not MissileData missileData)
            {
                throw new System.ArgumentException($"Expected {typeof(MissileData)}, got {poolableData.GetType()}");
            }
            tracking = missileData.Tracking;
            speed = missileData.Speed;
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

            if (Physics.Raycast(tr.position, tr.forward, out RaycastHit hit, speed * dt, collisionMask))
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
                    tr.rotation = Quaternion.LookRotation(newDirection);
                }
                tr.position += speed * dt * tr.forward;
            }
        }
    }
}