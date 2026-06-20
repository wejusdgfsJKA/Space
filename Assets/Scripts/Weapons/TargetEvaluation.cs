using UnityEngine;
namespace Weapons
{
    public abstract class TargetEvaluation : ScriptableObject
    {
        public abstract float Evaluate(Turret shooter, IObject target);
    }
}