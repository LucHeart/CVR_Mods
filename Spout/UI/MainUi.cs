using System.Reflection;
using BTKUILib;
using BTKUILib.UIObjects;

namespace LucHeart.Spout.UI;

public static class MainUi
{
    private static Page _rootPage;
    
    public static void Start()
    {
        QuickMenuAPI.PrepareIcon("LucHeartSpout", "SpoutIcon",
            Assembly.GetExecutingAssembly().GetManifestResourceStream("LucHeart.Spout.Resources.logo.png"));
        
        _rootPage = new Page("LucHeartSpout", "LucHeartSpout", true, "SpoutIcon")
        {
            MenuTitle = "LucHeart Spout",
            MenuSubtitle = "Spout output for ChilloutVR"
        };

        var portableCameraCategory = _rootPage.AddCategory("Portable Camera");
        var pct = portableCameraCategory.AddToggle("Enable Portable Camera", "Enable the portable camera to output via spout", MelonConfig.EnablePortableCamera.Value);
        pct.OnValueUpdated += b =>
        {
            MelonConfig.EnablePortableCamera.Value = b;
            
            PortableCameraSetup.UpdateSpoutSender();
        };
        
        var gameViewCategory = _rootPage.AddCategory("Game View");
        var gvt = gameViewCategory.AddToggle("Enable Game View", "Enable capturing the game view for spout output", MelonConfig.EnableGameView.Value);
        gvt.OnValueUpdated += b =>
        {
            MelonConfig.EnableGameView.Value = b;
            
            GameViewSetup.SetupGameView();
        };
    }
}