using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public static class ShipManager
{
    readonly static Dictionary<int, List<Ship>> ships = new();
    public static List<Ship> GetShips(int team)
    {
        return ships.ContainsKey(team) ? ships[team].ToList() : null;
    }
    public static List<Ship> GetTargets(int myTeam)
    {
        return ships.Where(kv => kv.Key != myTeam).SelectMany(kv => kv.Value).ToList();
    }
    public static void Register(Ship ship)
    {
        if (ship == null)
        {
            Debug.LogError("Cannot register null ship");
            return;
        }
        int team = ship.Team;
        if (!ships.ContainsKey(team))
        {
            ships[team] = new();
        }
        ships[team].Add(ship);
        ship.Index = ships[team].Count - 1;
    }
    public static void Unregister(Ship ship)
    {
        if (ship == null) return;
        int team = ship.Team;
        int index = ship.Index;
        if (ships.ContainsKey(team))
        {
            ships[team].RemoveAtSwapBack(ship.Index);
            if (ships[team].Count > 0) ships[team][index].Index = index;
            ship.Index = -1;
        }
    }
}
