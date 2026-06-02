using Pooling;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Weapons;
public class Ship : MonoPoolable, IRegisterableComponent
{
    [field: SerializeField] public int Team { get; set; }
    public int Index { get; set; }
    public Transform Transform { get; protected set; }
    private void Awake()
    {
        ComponentRegister<Ship>.Register(transform, this);
        Transform = transform;
    }
    private void OnDestroy()
    {
        ComponentRegister<Ship>.Unregister(transform);
    }
    public List<Ship> GetTargets() => ShipManager.GetTargets(Team);
    protected virtual void OnEnable()
    {
        ShipManager.Register(this);
    }
    protected override void OnDisable()
    {
        MissileManager.Unregister(transform.GetInstanceID());
        ShipManager.Unregister(this);
        base.OnDisable();
    }
}
