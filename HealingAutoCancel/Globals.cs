using EFT;
using EFT.HealthSystem;

namespace HealingAutoCancel;

public static class Globals
{
    public static Player? player { get; private set; }
    public static ActiveHealthController? activeHealthController { get; private set; }
    public static void SetPlayer(Player p) => player = p;
    public static void SetPlayerHealthController(ActiveHealthController activeHealthController) => Globals.activeHealthController = activeHealthController;
}