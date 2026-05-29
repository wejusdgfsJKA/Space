using HybridBT2;
using UnityEngine;

public class BanditTree : BT
{
    public Transform target;
    public void SetTarget(Transform target) => blackboard.SetData(Blackboard.Keys.Target, target);

    protected override void SetupBlackboard()
    {
        SetTarget(target);
    }
}
