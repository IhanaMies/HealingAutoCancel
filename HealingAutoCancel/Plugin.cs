using BepInEx;
using BepInEx.Configuration;
using HealingAutoCancel.Patches.GameWorldPatches;

namespace HealingAutoCancel;

[BepInPlugin("com.ihanamies.healingautocancel", "HealingAutoCancel", "6.0.0")]
public class Plugin : BaseUnityPlugin
{
    internal static ConfigEntry<bool>? EnableHealingAutoCancel;
    
    private void Awake()
    {
        Config.SaveOnConfigSet = true;
        SetupConfig();
        
        new OnGameStartedPatch().Enable();
    }

    private void SetupConfig()
    {
        EnableHealingAutoCancel = Config.Bind("General", "Enable automatic heal canceling", true);
    }
}