using ABI_RC.Systems.Camera;
using HarmonyLib;

namespace LucHeart.Spout.Patches;

public static class PortableCameraPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PortableCamera), "Start")]
    public static void StartPatch(PortableCamera __instance)
    {
        PortableCameraSetup.UpdateSpoutSender();
    }
}