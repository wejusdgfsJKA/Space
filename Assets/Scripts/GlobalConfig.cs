using System.Collections.Generic;
using UnityEngine;

public static class GlobalConfig
{
    public static readonly int MaxProjectiles = 1024;
    static readonly Dictionary<int, LayerMask> BulletCollisionMasks = new()
    {
        {7, 1 << 0 | 1 << 8}, {9,1<<0|1<<6},{10, 1<<0|1<<6|1<<8}
    };
    static readonly Dictionary<int, int> BulletLayerCorrespondence = new()
    {
        { 6, 7 }, { 8, 9 },{0,10}
    };
    public static LayerMask GetBulletCollisionMask(int bulletLayer)
    {
        return BulletCollisionMasks[bulletLayer];
    }
    public static int GetBulletLayer(int ownerLayer)
    {
        return BulletLayerCorrespondence[ownerLayer];
    }
}
