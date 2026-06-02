using EventBus;
using UnityEngine;
namespace HP
{
    public class HPComponent : MonoBehaviour
    {
        public float MaxHP;
        float CurrentHP;
        private void Awake()
        {
            EventBus<DamageInfo>.AddActions(transform.root.GetInstanceID(), TakeDamage);
        }
        public static bool TakeDamage(Transform transform, DamageInfo damageInfo)
        {
            return EventBus<DamageInfo>.Raise(transform.GetInstanceID(), damageInfo);
        }
        private void OnEnable()
        {
            CurrentHP = MaxHP;
        }
        public void TakeDamage(DamageInfo damageInfo)
        {
            if (damageInfo.Source == transform)
            {
                return;
            }
            CurrentHP -= damageInfo.Amount;
            if (CurrentHP <= 0)
            {
                Destroy(gameObject);
            }
        }
        private void OnDestroy()
        {
            EventBus<DamageInfo>.RemoveBinding(transform.root.GetInstanceID());
        }
    }
}