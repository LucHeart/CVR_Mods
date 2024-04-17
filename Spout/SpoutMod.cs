using System.IO;
using System.Reflection;
using LucHeart.Spout;
using MelonLoader;
using MelonLoader.Utils;
using UnityEngine;

[assembly: MelonInfo(typeof(SpoutMod), "LucHeart.Spout", "1.0.0", "LucHeart")]
[assembly: MelonGame("Alpha Blend Interactive", "ChilloutVR")]

namespace LucHeart.Spout;

public sealed class SpoutMod : MelonMod
{
    public static Shader? BlitShader { get; private set; }
    private AssetBundle? _assetsBundle;

    private static readonly string GamePluginsFolder = Path.Combine(MelonEnvironment.UnityGameDataDirectory, "Plugins");
    private static readonly string NativeLibKlakSpoutPath = Path.Combine(GamePluginsFolder, "KlakSpout.dll");

    public override void OnInitializeMelon()
    {
        MelonConfig.Register();
        
        ExtractNativeSpoutLib();
        LoadAssetBundle();
        LoadShader();
    }

    private void ExtractNativeSpoutLib()
    {
        using var assetStream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream("LucHeart.Spout.Resources.KlakSpout.dll");
        if (assetStream == null)
        {
            LoggerInstance.Error("Failed to load resource stream for native spout lib");
            return;
        }

        using var fileStream = new FileStream(NativeLibKlakSpoutPath, FileMode.Create);
        assetStream.CopyTo(fileStream);
        LoggerInstance.Msg("Extracted native spout lib");
    }

    private void LoadAssetBundle()
    {
        using var assetStream =
            Assembly.GetExecutingAssembly().GetManifestResourceStream("LucHeart.Spout.Resources.blit");
        if (assetStream == null)
        {
            LoggerInstance.Error("Failed to load asset bundle");
            return;
        }

        using var tempStream = new MemoryStream((int)assetStream.Length);
        assetStream.CopyTo(tempStream);

        _assetsBundle = AssetBundle.LoadFromMemory(tempStream.ToArray(), 0);
        _assetsBundle.hideFlags |= HideFlags.DontUnloadUnusedAsset;
    }

    private void LoadShader()
    {
        BlitShader = _assetsBundle?.LoadAsset<Shader>("blit");
        if (BlitShader == null) LoggerInstance.Error("Failed to load shader");
    }
}