using UnityEngine;

public static class GlobalConfig
{
    public static readonly int MaxProjectiles = 1024;
    public static readonly LayerMask BulletCollisionMask = 1 << 0 | 1 << 6 | 1 << 7;
}
