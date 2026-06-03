using AudioSystem;
using Pooling;
using UnityEngine;
namespace Weapons
{
    [CreateAssetMenu(menuName = "Weapons/BulletData", fileName = "BulletData")]
    public class BulletData : MonoPoolableData<Bullet>
    {
        public bool DisableOnHit = true;
        public int Damage;
        public float LifeTime;
        public SoundData SoundData;
    }
}