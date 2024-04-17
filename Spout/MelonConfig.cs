using MelonLoader;

namespace LucHeart.Spout;

public static class MelonConfig
{
    public static MelonPreferences_Entry<bool> EnablePortableCamera { get; private set; } = null!;
    public static MelonPreferences_Entry<bool> EnableGameView { get; private set; } = null!;

    public static void Register()
    {
        var category = MelonPreferences.CreateCategory("LucHeartSpout");
        
        EnablePortableCamera = category.CreateEntry("EnablePortableCamera", false, "Enable Portable Camera", 
            "Enable the portable camera feature");
        
        EnableGameView = category.CreateEntry("EnableGameView", false, "Enable Game View",
            "Enable capturing the game view for spout output");
    }
}