using UnityEngine;
using UnityEngine.Events;
using Utilities;

public class EnemyDetectionBasic : MonoBehaviour
{
    [SerializeField] protected UnityEvent<Transform> OnEnemyDetected;
    [SerializeField] protected float detectionRadius = 10;
    protected Transform target;
    public Transform Target
    {
        get => target;
        set
        {
            if (value == target) return;
            if (target != null)
            {
                OnEnemyDetected?.Invoke(null);
            }
            target = value;
            if (target != null)
            {
                OnEnemyDetected?.Invoke(target);
            }
        }
    }
    private void OnEnable()
    {
        Target = null;
    }
    private void OnDisable()
    {
        Target = null;
    }
    private void Update()
    {
        var ship = ComponentRegister<Ship>.Get(transform.root);
        if (ship == null) return;
        var targets = ship.GetTargets();
        if (targets == null || targets.Count == 0)
        {
            Target = null;
        }
        else
        {
            var closestTarget = targets[0];
            var closestDistance = Vector3.Distance(transform.position, closestTarget.Transform.position);
            for (int i = 1; i < targets.Count; i++)
            {
                var distance = Vector3.Distance(transform.position, targets[i].Transform.position);
                if (distance < closestDistance)
                {
                    closestTarget = targets[i];
                    closestDistance = distance;
                }
            }
            Target = closestDistance <= detectionRadius ? closestTarget.Transform : null;
        }
    }
}
