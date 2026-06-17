using UnityEngine;
namespace Weapons
{
    [CreateAssetMenu(menuName = "Weapons/Missile Data", fileName = "Missile Data")]
    public class MissileData : BulletData
    {
        public float Tracking, MaxTargetSignature, TopSpeed, Acceleration, HP = 1;
        protected override void OnValidate()
        {
            base.OnValidate();
            if (Tracking < 0)
            {
                Debug.LogError($"Missile tracking cannot be <0 for {this}. Setting to 0.");
                Tracking = 0;
            }
        }
    }
}