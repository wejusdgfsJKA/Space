using Unity.VisualScripting;
using UnityEngine;
using Utilities;
[RequireComponent(typeof(Rigidbody))]
public class Navigation : MonoBehaviour, IVelocityProvider
{
    [field: SerializeField] public bool DampenVelocityToZero { get; set; } = true;
    public void SetDestination(Vector3? destination) => Destination = destination;
    public Vector3? Destination { get; protected set; }
    public bool HasDestination => Destination != null;
    [Header("Parameters")]
    [SerializeField] protected Transform navPivot;
    public float topSpeed = 5f, acceleration = 1;
    [field: SerializeField] public float StopDistance { get; set; } = 0.5f;
    public float rotationSpeed = 5;
    public float thrust = 1;
    public float angleToForward = 10;
    protected float radAngleToForward;
    protected Rigidbody rb;
    [SerializeField] protected ObstacleChecker checker;
    [field: SerializeField] public bool UpdateRotation { get; set; } = true;


    protected Vector3 angularVelocity;
    public Vector3 AngularVelocity => angularVelocity;

    public Vector3 LinearVelocity => rb != null ? rb.linearVelocity : Vector3.zero;

    protected void Awake()
    {
        ComponentRegister<IVelocityProvider>.Register(transform, this);
        rb = gameObject.GetOrAddComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.linearDamping = 0;

        if (navPivot == null)
        {
            navPivot = new GameObject().transform;
            navPivot.SetPositionAndRotation(transform.position, transform.rotation);
            navPivot.parent = transform;
            Debug.LogWarning($"Created new nav pivot for {transform}.");
        }
        radAngleToForward = Mathf.Rad2Deg * angleToForward / 2;
    }
    private void OnEnable()
    {
        angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
    }
    protected void OnDestroy()
    {
        ComponentRegister<IVelocityProvider>.Unregister(transform);
    }
    void ApplyDampeners(Vector3 intendedVelocity, float deltaTime)
    {
        rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, intendedVelocity, thrust * deltaTime);
    }
    private void Update()
    {
        if (Destination == null)
        {
            if (DampenVelocityToZero) ApplyDampeners(Vector3.zero, Time.deltaTime);
            return;
        }
        navPivot.LookAt(Destination.Value);
        Vector3 direction = Destination.Value - transform.position;
        if (direction.magnitude <= StopDistance || Vector3.Distance(transform.position, Destination.Value) <= StopDistance)
        {
            Destination = null;
            return;
        }
        if (!checker.CheckForward(transform, direction.normalized))
        {
            MoveTowards(direction, Time.deltaTime);
        }
        else
        {
            var point = checker.RunCheck(navPivot);
            MoveTowards(point, Time.deltaTime);
        }
    }
    public void Face(Vector3 direction, float deltaTime)
    {
        if (direction == Vector3.zero) return;

        Quaternion rot = Quaternion.LookRotation(direction, direction + transform.up);
        Quaternion previousRotation = transform.rotation;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotationSpeed * deltaTime);

        Quaternion rotationDelta = (transform.rotation * Quaternion.Inverse(previousRotation)).normalized;
        rotationDelta.ToAngleAxis(out float angleInDegrees, out Vector3 axis);
        if (angleInDegrees > 180f) angleInDegrees -= 360f;

        angularVelocity = axis * (angleInDegrees * Mathf.Deg2Rad) / deltaTime;
    }
    void MoveTowards(Vector3 direction, float deltaTime)
    {
        if (UpdateRotation) Face(direction, deltaTime);

        ApplyDampeners(direction, deltaTime);
        if (Vector3.Dot(transform.forward, direction) >= Mathf.Cos(Mathf.Deg2Rad * angleToForward / 2))
        {
            rb.AddForce(acceleration * Time.deltaTime * direction.normalized, ForceMode.Acceleration);
        }
        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, topSpeed);
    }
    protected void OnDrawGizmos()
    {
        if (Destination != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, Destination.Value);
        }
        if (checker != null)
        {
            Gizmos.color = Color.white;
            checker.Draw(navPivot != null ? navPivot : transform);
        }
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, LinearVelocity);
    }
}
