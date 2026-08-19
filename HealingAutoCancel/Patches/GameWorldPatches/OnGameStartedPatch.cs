using System.Reflection;
using Comfort.Common;
using EFT;
using EFT.Ballistics;
using EFT.InventoryLogic;
using HarmonyLib;
using SPT.Reflection.Patching;

namespace HealingAutoCancel.Patches.GameWorldPatches;

public class OnGameStartedPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return AccessTools.Method(typeof(GameWorld), nameof(GameWorld.OnGameStarted));
    }

    [PatchPostfix]
    private static void Postfix()
    {
        var gameWorld = Singleton<GameWorld>.Instance;
        
        Globals.SetPlayer(gameWorld.MainPlayer);
        Globals.SetPlayerHealthController(gameWorld.MainPlayer.ActiveHealthController);
        Globals.activeHealthController!.HealthChangedEvent += AppendHealthChangedEvent;
    }

    private static void AppendHealthChangedEvent(EBodyPart part, float amount, DamageInfo damageInfo)
    {
        if (damageInfo.DamageType != EDamageType.Medicine) { return; }

        var medkitInHands = Globals.player!.TryGetItemInHands<Meds>();

        if (medkitInHands == null || Globals.activeHealthController!.IsBodyPartBroken(part)) { return; }

        var bodyPartHealth = Globals.activeHealthController.GetBodyPartHealth(part);

        var effects = Globals.activeHealthController.BodyPartEffects.Effects[part];

        var isBleeding = effects.ContainsKey("LightBleeding") || effects.ContainsKey("HeavyBleeding");

        var healingItemDepleted = medkitInHands.MedKitComponent.HpResource < 1;

        if ((bodyPartHealth.AtMaximum && !isBleeding) || healingItemDepleted)
        {
            Globals.activeHealthController.RemoveMedEffect();
        }
    }
}