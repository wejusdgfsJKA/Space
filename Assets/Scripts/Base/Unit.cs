using EventBus;
using HP;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Weapons;

public class Unit : MonoBehaviour, IRegisterableComponent, IObject
{
    #region Fields
    [Tooltip("If true, when this unit is destroyed all its inflight missiles " +
        "will have their owner set to null.")]
    [SerializeField] protected bool deleteOwnershipOnDeath = true;
    public int UnitManagerIndex { get; set; }
    [field: SerializeField] public int Team { get; set; }
    public virtual Vector3 LinearVelocity => Vector3.zero;
    public virtual Vector3 AngularVelocity => Vector3.zero;
    public float Signature => signature;
    public Transform Transform { get; protected set; }
    public Vector3 Position => Transform.position;
    [SerializeField] protected float signature, defaultSignature;
    [field: SerializeField] public float ScanRange { get; protected set; }
    protected HPComponent hpComponent;
    public float CurrentHP => hpComponent != null ? hpComponent.CurrentHP : 0;
    public float CurrentHPPercentage => hpComponent != null ? hpComponent.CurrentHP / hpComponent.MaxHP : 0;

    [field: SerializeField] public List<Turret> Turrets { get; protected set; } = new();
    protected Action onTick = delegate { };
    protected Collider[] buffer;
    protected int missileCount;
    [SerializeField] protected float tickInterval = 0.5f;
    protected WaitForSeconds wait;
    protected Coroutine coroutine;
    #endregion

    #region Setup
    protected virtual void Awake()
    {
        wait = new(tickInterval);
        Transform = transform;
        if (hpComponent != null) hpComponent = GetComponent<HPComponent>();
        ComponentRegister<Unit>.Register(Transform, this);
        foreach (var t in Turrets) onTick += t.UpdateTargets;
    }
    protected virtual void OnEnable()
    {
        coroutine = StartCoroutine(Tick());
        buffer = new Collider[GlobalConfig.MaxMissileCheckBufferSize];
        signature = defaultSignature;
        UnitManager.Register(this);
    }
    protected virtual void OnDisable()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        UnitManager.Unregister(this);
        buffer = null;
        EventBus<UnitDestroyed>.Raise(Transform.GetInstanceID(), new(deleteOwnershipOnDeath));
    }
    protected virtual void OnDestroy()
    {
        onTick = null;
        ComponentRegister<Unit>.Unregister(transform);
    }
    #endregion

    #region Functionality
    protected IEnumerator Tick()
    {
        yield return wait;
        GetSurroundingMissiles();
        onTick?.Invoke();
    }
    protected virtual void GetSurroundingMissiles()
    {
        missileCount = Physics.OverlapSphereNonAlloc(Transform.position, ScanRange, buffer, GlobalConfig.GetEnemyBulletCheckMask(gameObject.layer));
    }
    public (int, Collider[]) GetMissiles() => (missileCount, buffer);
    public List<Unit> GetTargets() => UnitManager.GetTargets(Team);
    #endregion
}