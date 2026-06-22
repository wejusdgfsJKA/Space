using System.Collections.Generic;
using UnityEngine;

public static class GlobalConfig
{
    public static readonly int MaxProjectiles = 1024, MaxMissileCheckBufferSize = 50;
    static readonly Dictionary<int, LayerMask> bulletCollisionMasks = new()
    {
        {7, 1 << 0 | 1 << 8}, {9,1<<0|1<<6},{10, 1<<0|1<<6|1<<8}
    };
    static readonly Dictionary<int, int> bulletLayerCorrespondence = new()
    {
        { 6, 7 }, { 8, 9 },{0,10}
    };
    static readonly Dictionary<int, LayerMask> bulletCheckMask = new()
    {
        {6, 1 << 9 | 1 << 10}, {8,1<<7|1<<10},{0, 1<<10|1<<7|1<<9}
    };
    /// <summary>
    /// Returns the layer mask that an object should use to check for enemy bullets.
    /// </summary>
    /// <param name="checkerLayer"></param>
    /// <returns></returns>
    public static LayerMask GetEnemyBulletCheckMask(int checkerLayer)
    {
        return bulletCheckMask[checkerLayer];
    }
    /// <summary>
    /// Returns the collision mask that a bullet should use, based on its own layer.
    /// </summary>
    /// <param name="bulletLayer"></param>
    /// <returns></returns>
    public static LayerMask GetBulletCollisionMask(int bulletLayer)
    {
        return bulletCollisionMasks[bulletLayer];
    }
    /// <summary>
    /// Returns a layer that a bullet should use, given the layer of its owner.
    /// </summary>
    /// <param name="ownerLayer"></param>
    /// <returns></returns>
    public static int GetBulletLayer(int ownerLayer)
    {
        return bulletLayerCorrespondence[ownerLayer];
    }
}
