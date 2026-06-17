using UnityEngine;
using UnityEngine.Events;
using Utilities;

public class EnemyDetectionBasic : MonoBehaviour
{
    [SerializeField] protected UnityEvent<Unit> OnEnemyDetected;
    [SerializeField] protected float detectionRadius = 10;
    protected Unit target = null;
    public Unit Target
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
    private void OnDisable()
    {
        Target = null;
    }
    private void Update()
    {
        var ship = ComponentRegister<Unit>.Get(transform.root);
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
            Target = closestDistance <= detectionRadius ? closestTarget : null;
        }
    }
    private void OnDrawGizmos()
    {
        if (Target == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Target.Position);
    }
}
