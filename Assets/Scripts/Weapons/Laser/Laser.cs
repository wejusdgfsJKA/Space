using HP;
using Pooling;
using UnityEngine;
namespace Weapons
{
    [RequireComponent(typeof(LineRenderer))]
    public class Laser : Bullet
    {
        protected float range = 10;
        public Transform Target { get; set; }
        protected LineRenderer lineRenderer;
        public override void Initialize<T>(MonoPoolableData<T> poolableData)
        {
            base.Initialize(poolableData);
            if (poolableData is not LaserData laserData)
            {
                Debug.LogError("Invalid poolable data for Laser.");
                return;
            }
            if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.startColor = laserData.Color;
            lineRenderer.endColor = laserData.Color;
            lineRenderer.startWidth = laserData.Width;
            lineRenderer.endWidth = laserData.Width;
            range = laserData.Range;
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            var line = GetComponent<LineRenderer>();
            if (line != null)
            {
                line.SetPosition(0, transform.position);
                if (Target == null) line.SetPosition(1, transform.position + transform.forward * range);
                else
                {
                    line.SetPosition(1, Target.position);
                    HPComponent.TakeDamage(Target, damageInfo);
                }
            }
        }
    }
}