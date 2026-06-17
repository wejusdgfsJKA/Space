using HybridBT2;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "HybridBT2/Leaves/FaceTarget", fileName = "FaceTarget")]
public class FaceTargetNode : LeafNodeData
{
    protected override Func<LeafNode, Blackboard, NodeState> onEvaluate => (n, ctx) =>
    {
        var target = ctx.GetData<Unit>(Blackboard.Keys.Target);
        if (target == null) return NodeState.FAILURE;
        if (!target.gameObject.activeSelf)
        {
            ctx.SetData<Unit>(Blackboard.Keys.Target, null);
            return NodeState.FAILURE;
        }
        ctx.Navigation.Face(target.Position - ctx.Transform.position, ctx.DeltaTime);
        return NodeState.RUNNING;
    };
}
