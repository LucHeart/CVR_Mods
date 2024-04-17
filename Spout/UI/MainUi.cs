using BTKUILib.UIObjects;

namespace LucHeart.Spout.UI;

public static class MainUi
{
    public static void Start()
    {
        var rootPage = new Page("LucHeartSpout", "LucHeartSpout", true)
        {
            MenuTitle = "LucHeart Spout",
            MenuSubtitle = "Spout output for ChilloutVR"
        };

        var portableCameraCategory = rootPage.AddCategory("Portable Camera");
        var pct = portableCameraCategory.AddToggle("Enable Portable Camera", "Enable the portable camera to output via spout", MelonConfig.EnablePortableCamera.Value);
        pct.OnValueUpdated += b => MelonConfig.EnablePortableCamera.Value = b;
    }
}