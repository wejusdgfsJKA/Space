using HybridBT2;
using System;
using UnityEngine;
using Utilities;
/// <summary>
/// This node handles basic strafing. It does NOT handle ship facing.
/// </summary>
[CreateAssetMenu(menuName = "HybridBT2/Leaves/BasicStrafe", fileName = "BasicStrafe")]
public class BasicStrafeNode : LeafNodeData
{
    protected readonly struct Triangle
    {
        public readonly Vector3 PrevPoint1, PrevPoint2;
        public Triangle(Vector3 prevPoint1, Vector3 prevPoint2)
        {
            PrevPoint1 = prevPoint1;
            PrevPoint2 = prevPoint2;
        }
    }
    protected enum Keys
    {
        PrevTargetPos,
        PrevPoint
    }
    [Tooltip("Object to circle around. Must be a Transform.")]
    public Blackboard.Keys Key;
    public float Radius = 10;
    protected override Action<Node, Blackboard> onEnter => (n, ctx) =>
    {
        ctx.Navigation.UpdateRotation = false;
        ctx.Navigation.SetDestination(null);
        ((LeafNode)n).SetData<Vector3?>((int)Keys.PrevPoint, null);
    };
    protected override Func<LeafNode, Blackboard, NodeState> onEvaluate => (n, ctx) =>
    {
        var tr = ctx.GetData<Transform>(Key);
        if (tr == null) return NodeState.FAILURE;
        if (!ctx.Navigation.HasDestination)
        {
            //find a new point around the target to move to
            var t = n.GetData<Triangle?>((int)Keys.PrevPoint);
            if (t == null)
            {
                var p1 = (ctx.Transform.position - tr.position).normalized * Radius;
                var p2 = p1.Perpendicular() * Radius;
                Debug.DrawLine(p1 + tr.position, p2 + tr.position, Color.red, 10);
                var p3 = ((p1 + p2) / 2).normalized * Radius;
                ctx.Navigation.SetDestination(tr.position + p3);

                n.SetData<Triangle>((int)Keys.PrevPoint, new(p1, p3));
            }
            else
            {
                Debug.DrawLine(t.Value.PrevPoint1 + tr.position, t.Value.PrevPoint2 + tr.position, Color.yellow, 10);

                var v = Vector3.Project(t.Value.PrevPoint1 - t.Value.PrevPoint2, -t.Value.PrevPoint2);

                var point = v + t.Value.PrevPoint2;

                var p = point + point - t.Value.PrevPoint1;

                Debug.DrawLine(t.Value.PrevPoint2 + tr.position, tr.position + p, Color.green, 10);


                ctx.Navigation.SetDestination(tr.position + p);

                n.SetData<Triangle>((int)Keys.PrevPoint, new(t.Value.PrevPoint2, p));
            }

        }
        return NodeState.RUNNING;
    };
    protected override Action<Node, Blackboard> onExit => (n, ctx) =>
    {
        ctx.Navigation.SetDestination(null);
    };
}