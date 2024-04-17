using UnityEngine;
using UnityEngine.Rendering;
using RTID = UnityEngine.Rendering.RenderTargetIdentifier;

namespace LucHeart.Spout.KlakSpout.Internal;

internal static class SpoutBlitter
{
    public static void Blit
        (Texture src, RenderTexture dst, bool alpha)
        => Graphics.Blit(src, dst, BlitMaterial, alpha ? 0 : 1);

    public static void BlitVFlip
        (Texture src, RenderTexture dst, bool alpha)
        => Graphics.Blit(src, dst, BlitMaterial, alpha ? 2 : 3);

    public static void Blit
        (CommandBuffer cb, RTID src, RTID dst, bool alpha)
        => cb.Blit(src, dst, BlitMaterial, alpha ? 0 : 1);

    public static void BlitFromSrgb
        (Texture src, RenderTexture dst)
        => Graphics.Blit(src, dst, BlitMaterial, 4);

    private static Material BlitMaterial =>
        _material ??= GetNewBlitMaterial();

    private static Material? _material;

    private static Material GetNewBlitMaterial() => new(SpoutMod.BlitShader)
    {
        hideFlags = HideFlags.DontSave
    };
}