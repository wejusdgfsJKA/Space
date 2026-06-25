using EventBus;
using UnityEngine;
using Utilities;

/// <summary>
/// Provides linear and angular velocity.
/// </summary>
public interface IObject : IRegisterableComponent
{
    float Signature { get; }
    Transform Transform { get; }
    public Vector3 Position => Transform.position;
    Vector3 AngularVelocity { get; }
    Vector3 LinearVelocity { get; }
}
public struct DamageInfo : IEvent
{
    public float Amount { get; set; }
    public Transform Source { get; set; }
}

public readonly struct MissionOver : IEvent
{
    public readonly bool Failed;
    public MissionOver(bool failed) => Failed = failed;
}

public readonly struct UnitDestroyed : IEvent { }

public readonly struct PoolableRecycled : IEvent { }

public interface IResettable
{
    void PerformReset();
}

public interface IOwnable : IRegisterableComponent
{
    Transform Owner { get; }
}