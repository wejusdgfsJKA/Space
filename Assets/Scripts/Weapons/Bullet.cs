using AudioSystem;
using HP;
using Pooling;
using UnityEngine;
using Utilities;
namespace Weapons
{
    public class Bullet : MonoPoolable, IOwnable
    {
        protected DamageInfo damageInfo = new();
        public Transform Owner
        {
            get
            {
                if (damageInfo.Source == null)
                {
                    return null;
                }
                return damageInfo.Source;
            }
            set
            {
                damageInfo.Source = value;
            }
        }
        protected Transform tr;
        protected SoundData soundData;
        protected bool disableOnHit = true;
        protected float lifeTime;
        protected virtual void Awake()
        {
            tr = transform;
            ComponentRegister<IOwnable>.Register(tr, this);
        }
        protected virtual void OnEnable()
        {
            if (soundData != null && soundData.clip != null) SoundManager.Instance.Play(soundData, tr.position);
            else Debug.LogWarning($"{transform} has no sound data!");
            GlobalUpdater.Instance.RegisterUpdate(PerformUpdate);
        }
        public override void Initialize<T>(MonoPoolableData<T> poolableData)
        {
            base.Initialize(poolableData);
            if (poolableData is not BulletData bulletData)
            {
                throw new System.ArgumentException($"Expected {typeof(BulletData)}, got {poolableData.GetType()}");
            }
            disableOnHit = bulletData.DisableOnHit;
            damageInfo.Amount = bulletData.Damage;
            lifeTime = bulletData.LifeTime;
            soundData = bulletData.SoundData;
        }
        protected virtual void PerformUpdate(float dt)
        {
            lifeTime -= dt;
            if (lifeTime <= 0)
            {
                gameObject.SetActive(false);
            }
        }
        protected virtual void OnDestroy()
        {
            ComponentRegister<IOwnable>.Unregister(tr);
        }
        protected override void OnDisable()
        {
            GlobalUpdater.Instance.UnregisterUpdate(PerformUpdate);
            base.OnDisable();
        }
        protected virtual void OnHit(Collider collider)
        {
            HPComponent.TakeDamage(collider.transform.root, damageInfo);
            if (disableOnHit) gameObject.SetActive(false);
        }
    }
}