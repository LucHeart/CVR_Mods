using UnityEngine;


namespace LucHeart.Spout.KlakSpout.Internal;

internal static class Utility
{
    public static void Destroy(Object obj)
    {
        if (obj == null) return;

        if (Application.isPlaying)
            Object.Destroy(obj);
        else
            Object.DestroyImmediate(obj);
    }
}