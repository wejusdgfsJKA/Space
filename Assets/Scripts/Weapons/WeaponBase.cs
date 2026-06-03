using Timers;
using UnityEngine;

namespace Weapons
{
    public abstract class WeaponBase : MonoBehaviour, IResettable
    {
        #region Utils
        public bool Firing { get; protected set; }
        #endregion

        #region Weapon parameters
        [SerializeField] protected WeaponParameters @params;
        protected CountdownTimer shotTimer;
        #endregion

        #region Setup
        protected virtual void Awake()
        {
            Debug.Assert(@params != null, $"{transform} has no weapon parameters!");
            Debug.Assert(@params.Cooldown >= 0, $"{transform} has negative cooldown!");
            shotTimer = new(@params.Cooldown);
        }
        protected virtual void OnEnable()
        {
            PerformReset();
        }
        /// <summary>
        /// Reset the ammo, cooldown and cast block index of the weapon.
        /// </summary>
        public virtual void PerformReset()
        {
            shotTimer.Reset();
            Firing = false;
        }
        protected virtual void OnDestroy()
        {
            shotTimer.Dispose();
        }
        #endregion

        #region Functionality
        public virtual void AimAt(Transform target)
        {
            transform.LookAt(target);
        }
        public virtual bool CanShoot()
        {
            return !shotTimer.IsRunning;
        }
        public Bullet Shoot()
        {
            if (!CanShoot()) return null;
            var b = Fire();
            if (b == null) return null;
            shotTimer.Start();
            return b;
        }
        /// <summary>
        /// Fire once.
        /// </summary>
        public virtual Bullet Fire()
        {
            return @params.BulletData.GetInstance(transform);
        }
        #endregion
    }
}