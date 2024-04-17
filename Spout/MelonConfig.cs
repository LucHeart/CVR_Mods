using MelonLoader;

namespace LucHeart.Spout;

public static class MelonConfig
{
    public static MelonPreferences_Entry<bool> EnablePortableCamera { get; private set; } = null!;

    public static void Register()
    {
        var category = MelonPreferences.CreateCategory("LucHeartSpout");
        
        EnablePortableCamera = category.CreateEntry("EnablePortableCamera", true, "Enable Portable Camera", "Enable the portable camera feature");
    }
}