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
    Vector3 AngularVelocity { get; }
    Vector3 LinearVelocity { get; }
}
public struct DamageInfo : IEvent
{
    public float Amount { get; set; }
    public Transform Source { get; set; }
}

public readonly struct UnitDestroyed : IEvent
{

}

public readonly struct PoolableRecycled : IEvent
{

}

public interface IResettable
{
    void PerformReset();
}

public interface IOwnable : IRegisterableComponent
{
    Transform Owner { get; }
}