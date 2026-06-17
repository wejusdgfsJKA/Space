using Unity.VisualScripting;
using UnityEngine;
using Utilities;
public class Ship : Unit, IRegisterableComponent, IVelocityProvider
{
    [SerializeField] protected float topSpeed = 5f, acceleration = 1, rotationSpeed = 5, thrust = 1;
    protected Rigidbody rb;
    protected Vector3 angularVelocity;
    public Vector3 AngularVelocity => angularVelocity;
    public Vector3 LinearVelocity => rb != null ? rb.linearVelocity : Vector3.zero;
    protected override void Awake()
    {
        base.Awake();
        ComponentRegister<IVelocityProvider>.Register(transform, this);
        rb = gameObject.GetOrAddComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.linearDamping = 0;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
    }
    protected override void OnDestroy()
    {
        base.Awake();
        ComponentRegister<IVelocityProvider>.Unregister(transform);
    }
    protected void ApplyDampeners(Vector3 intendedVelocity, float deltaTime)
    {
        rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity,
            intendedVelocity, thrust * deltaTime);
    }
}
