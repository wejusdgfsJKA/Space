using UnityEngine;

namespace Weapons
{
    public class MissileLauncher : WeaponBase
    {
        public override void ProcessBullet(Bullet bullet, Unit target)
        {
            if (target != null) ((Missile)bullet).Target = target;
            else Debug.LogWarning($"Missile launcher {transform} was not passed a target!");
            base.ProcessBullet(bullet, target);
        }
    }
}