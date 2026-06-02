using UnityEngine;
namespace Weapons
{
    [CreateAssetMenu(menuName = "Weapons/Missile Data", fileName = "Missile Data")]
    public class MissileData : BulletData
    {
        public float Tracking, Speed;
    }
}