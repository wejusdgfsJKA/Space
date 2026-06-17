using HP;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Unit : MonoBehaviour, IRegisterableComponent
{
    public int UnitManagerIndex { get; set; }
    [field: SerializeField] public int Team { get; set; }
    [SerializeField] protected float signature, defaultSignature;
    public float Signature => signature;
    public Transform Transform { get; protected set; }
    public Vector3 Position => Transform.position;
    protected HPComponent hpComponent;
    public float CurrentHP => hpComponent != null ? hpComponent.CurrentHP : 0;
    public float CurrentHPPercentage => hpComponent != null ? hpComponent.CurrentHP / hpComponent.MaxHP : 0;
    protected virtual void Awake()
    {
        Transform = transform;
        if (hpComponent != null) hpComponent = GetComponent<HPComponent>();
        ComponentRegister<Unit>.Register(Transform, this);
    }
    protected virtual void OnEnable() => UnitManager.Register(this);
    protected virtual void OnDisable() => UnitManager.Unregister(this);
    protected virtual void OnDestroy()
    {
        ComponentRegister<Unit>.Unregister(transform);
    }
    public List<Unit> GetTargets() => UnitManager.GetTargets(Team);
}
