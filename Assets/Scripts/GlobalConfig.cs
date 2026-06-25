using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// This class has a bunch of global parameters and shit.
/// </summary>
public static class GlobalConfig
{
    public static string GetSaveFilePath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }
    /// <summary>
    /// This is the size that the buffer is initialized with when checking for missiles.
    /// Might be worth expanding to 100.
    /// </summary>
    public static readonly int MaxMissileCheckBufferSize = 50;
    public static readonly int EndMissionSceneIndex = 2;

    #region Layers and layer masks
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
    #endregion

    #region Colors
    static readonly Dictionary<int, Color> teamColors = new()
    {
        { 0,Color.white},{6,Color.blue},{8,Color.red},
        { 10,Color.white},{7,Color.blue},{9,Color.red}
    };

    /// <summary>
    /// Returns what color should an object appear with on the radar. Default is white.
    /// </summary>
    /// <param name="ownerLayer">The game object layer of an object.</param>
    /// <returns></returns>
    public static Color GetColor(int ownerLayer)
    {
        if (teamColors.TryGetValue(ownerLayer, out var color)) return color;
        return Color.white;
    }
    #endregion
}
