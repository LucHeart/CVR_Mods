using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine.LowLevel;

namespace LucHeart.Spout.KlakSpout.Internal;

//
// "Memory pool" class without actual memory pool functionality
// At the moment, it only provides the delayed destruction method.
//
internal static class MemoryPool
{
    #region Public method

    public static void FreeOnEndOfFrame(GCHandle gch)
        => ToBeFreed.Push(gch);

    #endregion

    #region Delayed destruction

    private static readonly Stack<GCHandle> ToBeFreed = new Stack<GCHandle>();

    private static void OnEndOfFrame()
    {
        while (ToBeFreed.Count > 0) ToBeFreed.Pop().Free();
    }

    #endregion

    #region PlayerLoopSystem implementation

    static MemoryPool()
    {
        InsertPlayerLoopSystem();

#if UNITY_EDITOR
        // We use not only PlayerLoopSystem but also the
        // EditorApplication.update callback because the PlayerLoop events are
        // not invoked in the edit mode.
        UnityEditor.EditorApplication.update += OnEndOfFrame;
#endif
    }

    private static void InsertPlayerLoopSystem()
    {
        var customSystem = new PlayerLoopSystem()
            { type = typeof(MemoryPool), updateDelegate = OnEndOfFrame };

        var playerLoop = PlayerLoop.GetCurrentPlayerLoop();

        for (var i = 0; i < playerLoop.subSystemList.Length; i++)
        {
            ref var phase = ref playerLoop.subSystemList[i];
            if (phase.type == typeof(UnityEngine.PlayerLoop.PostLateUpdate))
            {
                phase.subSystemList = phase.subSystemList
                    .Concat(new [] { customSystem }).ToArray();
                break;
            }
        }

        PlayerLoop.SetPlayerLoop(playerLoop);
    }

    #endregion
}

// namespace Klak.Spout
