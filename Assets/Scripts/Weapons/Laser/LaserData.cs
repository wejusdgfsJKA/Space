using UnityEngine;
namespace Weapons
{
    [CreateAssetMenu(menuName = "Weapons/LaserData", fileName = "LaserData")]
    public class LaserData : BulletData
    {
        public Color Color = Color.red;
        public float Range = 10, Width = 0.1f;
    }
}