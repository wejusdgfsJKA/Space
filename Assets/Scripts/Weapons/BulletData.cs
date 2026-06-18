using AudioSystem;
using Pooling;
using UnityEngine;
namespace Weapons
{
    [CreateAssetMenu(menuName = "Weapons/BulletData", fileName = "BulletData")]
    public class BulletData : MonoPoolableData<Bullet.Type, Bullet>
    {
        public bool DisableOnHit = true;
        public int Damage;
        public float LifeTime;
        public SoundData SoundData;
        protected override Bullet GetInstance()
        {
            if (!Bullet.Get(Key, out Bullet instance))
            {
                instance = Instantiate(Prefab);
            }
            instance.Initialize(this);
            return instance;
        }
        protected virtual void OnValidate()
        {
            if (LifeTime <= 0)
            {
                Debug.LogError($"Lifetime <= 0 for {this}!");
                LifeTime = 1;
            }
        }
    }
}