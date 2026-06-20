using UnityEngine;
namespace Weapons
{
    [CreateAssetMenu(fileName = "ClosestTargetEvaluation", menuName = "Weapons/Turret/ClosestTargetEvaluation")]
    public class TargetDistanceEvaluation : TargetEvaluation
    {
        public override float Evaluate(Turret shooter, IObject target)
        {
            if (target == null) return 0;

            if (!shooter.IsInAngle(target.Transform.position)) return 0;

            var dir = target.Transform.position - shooter.Transform.position;

            if (dir.sqrMagnitude > shooter.MaxRangeSquared) return 0;

            return Mathf.Max(shooter.MaxRangeSquared - dir.sqrMagnitude, 0) / shooter.MaxRangeSquared;
        }
    }
}