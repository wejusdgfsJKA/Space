using EventBus;
using UnityEngine;
using Utilities;
namespace HP
{
    public class HPComponent : MonoBehaviour, IResettable
    {
        [SerializeField] protected float maxHP;
        public float MaxHP
        {
            get => maxHP;
            set
            {
                if (value <= 0)
                {
                    Debug.LogError($"MaxHP <= 0 for {transform}. Setting to 1.");
                    value = 1;
                }
                maxHP = value;
            }
        }
        public float CurrentHP { get; protected set; }
        public void PerformReset()
        {
            CurrentHP = MaxHP;
        }
        protected virtual void Awake()
        {
            EventBus<DamageInfo>.AddActions(transform.root.GetInstanceID(), TakeDamage);
        }
        public static bool TakeDamage(Transform transform, DamageInfo damageInfo)
        {
            return EventBus<DamageInfo>.Raise(transform.GetInstanceID(), damageInfo);
        }
        private void OnEnable()
        {
            PerformReset();
        }
        public void TakeDamage(DamageInfo damageInfo)
        {
            if (damageInfo.Source.GetTeam(throwOnNullTransform: false) == transform.root.GetTeam())
            {
                return;
            }
            CurrentHP -= damageInfo.Amount;
            if (CurrentHP <= 0)
            {
                gameObject.SetActive(false);
            }
        }
        private void OnDestroy()
        {
            EventBus<DamageInfo>.RemoveBinding(transform.root.GetInstanceID());
        }
    }
}