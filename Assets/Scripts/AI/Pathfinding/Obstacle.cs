using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Obstacle : MonoBehaviour
{
    public readonly static List<Obstacle> Obstacles = new();
    [field: SerializeField] public float Radius { get; protected set; } = 1f;
    protected Transform tr;
    public Transform Transform => tr == null ? transform : tr;
    protected virtual void Awake()
    {
        tr = transform;
        ComponentRegister<Obstacle>.Register(tr, this);
    }
    protected virtual void OnDestroy()
    {
        ComponentRegister<Obstacle>.Unregister(tr);
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Transform.position, Radius);
    }
    public Vector3 AvoidNode(Vector3 direction, float incomingRadius)
    {
        return AvoidNode(direction, incomingRadius, Transform.position, Radius);
    }
    public static Vector3 AvoidNode(Vector3 direction, float incomingRadius, Vector3 obstaclePosition, float obstacleRadius)
    {
        Debug.DrawLine(obstaclePosition, obstaclePosition + direction.Perpendicular() * (incomingRadius + obstacleRadius), Color.green);
        return obstaclePosition + direction.Perpendicular() * (incomingRadius + obstacleRadius);
    }
}
