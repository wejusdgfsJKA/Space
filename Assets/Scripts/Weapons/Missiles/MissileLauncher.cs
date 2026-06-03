using UnityEngine;

namespace Weapons
{
    public class MissileLauncher : WeaponBase
    {
        public Missile Shoot(Transform target)
        {
            var missile = Shoot() as Missile;
            if (missile != null)
            {
                missile.Target = target;
                missile.gameObject.SetActive(true);
            }
            return missile;
        }
    }
}