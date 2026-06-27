using EventBus;
using Spawning;
using Utilities;
public abstract class Mission
{
    public static Mission CurrentMission { get; set; }

    public void CheckIfMissionIsOver()
    {
        if (IsMissionOver()) MissionManager.TryGetInstance().EndMission();
    }

    public virtual void OnUnitDestroyed(ObjectDestroyed @event)
    {
        EventBus<ObjectDestroyed>.RemoveActions(@event.VictimID, OnUnitDestroyed);
        GameManager.UnitsDestroyed.Add(@event);
    }

    protected Unit SpawnUnit(MonoSpawnableData<Unit> data, int team)
    {
        var unit = data.CreateInstance();
        unit.Team = team;
        EventBus<ObjectDestroyed>.AddActions(Extensions.GetInstanceID(unit), OnUnitDestroyed);
        return unit;
    }

    public static Mission GetRandomMission() => null;

    public abstract void Initialize();

    public abstract bool IsMissionOver();
}
