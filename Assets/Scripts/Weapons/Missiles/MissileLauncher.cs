namespace Weapons
{
    public class MissileLauncher : WeaponBase
    {
        public void Shoot(Unit target)
        {
            if (target == null || !target.Transform.gameObject.activeSelf) return;
            var missile = Shoot() as Missile;
            if (missile != null)
            {
                missile.Target = target;
                missile.gameObject.SetActive(true);
            }
        }
    }
}