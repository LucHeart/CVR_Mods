using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

namespace LucHeart.Spout.KlakSpout.Internal;

// Render event IDs
// Should match with KlakSpout::EventID (Event.h)
internal enum EventID
{
    UpdateSender,
    UpdateReceiver,
    CloseSender,
    CloseReceiver
}

// Render event attachment data structure
// Should match with KlakSpout::EventData (Event.h)
[StructLayout(LayoutKind.Sequential)]
internal struct EventData
{
    public IntPtr instancePointer;
    public IntPtr texturePointer;

    public EventData(IntPtr instance, IntPtr texture)
    {
        instancePointer = instance;
        texturePointer = texture;
    }

    public EventData(IntPtr instance)
    {
        instancePointer = instance;
        texturePointer = IntPtr.Zero;
    }
}

internal class EventKicker : IDisposable
{
    public EventKicker(EventData data)
        => _dataMem = GCHandle.Alloc(data, GCHandleType.Pinned);

    public void Dispose()
        => MemoryPool.FreeOnEndOfFrame(_dataMem);

    public void IssuePluginEvent(EventID eventID)
    {
        if (_cmdBuffer == null)
            _cmdBuffer = new CommandBuffer();
        else
            _cmdBuffer.Clear();

        _cmdBuffer.IssuePluginEventAndData
        (Plugin.GetRenderEventCallback(),
            (int)eventID, _dataMem.AddrOfPinnedObject());

        Graphics.ExecuteCommandBuffer(_cmdBuffer);
    }

    private static CommandBuffer? _cmdBuffer;
    private GCHandle _dataMem;
}

// namespace Klak.Spout
