using HybridBT2;
using UnityEngine;
using Weapons;
public class BanditTree : BT
{
    public void SetTarget(Transform target) => blackboard.SetData(Blackboard.Keys.Target, target);

    protected override void SetupBlackboard()
    {
        var launchers = GetComponentsInChildren<MissileLauncher>();
        blackboard.SetData(Blackboard.Keys.Launchers, launchers);
    }
}
