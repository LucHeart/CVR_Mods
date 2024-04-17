using System.Runtime.InteropServices;
using IntPtr = System.IntPtr;

namespace LucHeart.Spout.KlakSpout.Internal;

internal static class Plugin
{
    [DllImport("KlakSpout")]
    public static extern IntPtr GetRenderEventCallback();

    [DllImport("KlakSpout")]
    public static extern IntPtr CreateSender(string name, int width, int height);

}