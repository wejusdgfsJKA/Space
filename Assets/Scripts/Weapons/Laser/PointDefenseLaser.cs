using UnityEngine;
using Utilities;
namespace Weapons
{
    public class PointDefenseLaser : WeaponBase
    {
        [SerializeField] protected float Range = 10, Angle = 90;
        protected override void OnEnable()
        {
            base.OnEnable();
            GlobalUpdater.Instance.RegisterUpdate(PerformUpdate);
        }
        protected virtual void OnDisable()
        {
            GlobalUpdater.Instance.UnregisterUpdate(PerformUpdate);
        }
        protected Laser Shoot(Transform missile)
        {
            if (!transform.gameObject.activeSelf) return null;
            var laser = Shoot() as Laser;
            if (laser != null)
            {
                laser.Target = missile;
                laser.gameObject.SetActive(true);
            }
            return laser;
        }
        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Range);
            Vector3 leftBoundary = Quaternion.Euler(0, -Angle / 2, 0) * transform.forward;
            Vector3 rightBoundary = Quaternion.Euler(0, Angle / 2, 0) * transform.forward;
            Gizmos.DrawLine(transform.position, transform.position + leftBoundary * Range);
            Gizmos.DrawLine(transform.position, transform.position + rightBoundary * Range);
        }
        public void PerformUpdate(float deltaTime)
        {
            if (!CanShoot()) return;
            var missiles = MissileManager.GetMissiles(transform.root.GetInstanceID());
            if (missiles == null || missiles.Count == 0) return;
            float closestDistance = float.MaxValue;
            int closestIndex = -1;
            for (int i = 0; i < missiles.Count; i++)
            {
                var missile = missiles[i];
                if (missile == null || !missile.gameObject.activeSelf) continue;
                var directionToMissile = missile.transform.position - transform.position;
                if (directionToMissile.magnitude > Range) continue;
                if (Vector3.Angle(transform.forward, directionToMissile) > Angle / 2) continue;
                if (directionToMissile.magnitude < closestDistance)
                {
                    closestDistance = directionToMissile.magnitude;
                    closestIndex = i;
                }
            }
            if (closestIndex != -1)
            {
                Shoot(missiles[closestIndex].transform);
            }
        }
    }
}