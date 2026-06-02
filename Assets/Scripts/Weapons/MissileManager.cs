using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
namespace Weapons
{
    public static class MissileManager
    {
        static readonly Dictionary<int, List<Missile>> missiles = new();
        public static List<Missile> GetMissiles(int targetId)
        {
            return missiles.ContainsKey(targetId) ? missiles[targetId].ToList() : null;
        }
        public static void Register(Missile missile)
        {
            if (missile == null)
            {
                Debug.LogError("Cannot register null missile");
                return;
            }
            if (missile.Target == null)
            {
                Debug.LogError($"Cannot register missile {missile.transform} with null target");
                return;
            }
            int id = missile.Target.GetInstanceID();
            if (!missiles.ContainsKey(id))
            {
                missiles[id] = new List<Missile>();
            }
            missiles[id].Add(missile);
            missile.Index = missiles[id].Count - 1;
        }
        public static void Unregister(int targetId)
        {
            missiles.Remove(targetId);
        }
        public static void Unregister(Missile missile)
        {
            if (missile == null || missile.Target == null) return;
            int id = missile.Target.GetInstanceID();
            int index = missile.Index;
            if (missiles.ContainsKey(id))
            {
                missiles[id].RemoveAtSwapBack(missile.Index);
                if (missiles[id].Count > index) missiles[id][index].Index = index;
            }
        }
    }
}