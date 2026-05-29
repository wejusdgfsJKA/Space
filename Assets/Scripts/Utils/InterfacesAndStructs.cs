using UnityEngine;

/// <summary>
/// Provides linear and angular velocity.
/// </summary>
public interface IVelocityProvider
{
    Vector3 AngularVelocity { get; }
    Vector3 LinearVelocity { get; }
}
public struct DamageInfo
{
    public float Amount { get; }
    public Vector3 HitPoint { get; }
    public Vector3 HitNormal { get; }
    public Transform Source { get; }
    public DamageInfo(float amount, Vector3 hitPoint, Vector3 hitNormal, Transform source)
    {
        Amount = amount;
        HitPoint = hitPoint;
        HitNormal = hitNormal;
        Source = source;
    }
}
public interface IHP
{
    float CurrentHP { get; }
    float MaxHP { get; }
    bool IsAlive { get; }
    void TakeDamage(DamageInfo damageInfo);
}
public interface IResettable
{
    void PerformReset();
}
public interface IOwnable
{
    Transform Owner { get; }
}