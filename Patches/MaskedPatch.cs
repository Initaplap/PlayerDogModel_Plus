
using HarmonyLib;
using MoreCompany;

namespace PlayerDogModel_Plus.Patches
{
    public static class MaskedPatch
    {
        [HarmonyPatch(typeof(CosmeticPatches), nameof(CosmeticPatches.SetEnemyOutside))]
        class SetEnemyOutsidePatch
        {
            static void Prefix(MaskedPlayerEnemy instance, ref bool result)
            {
                if (instance.mimickingPlayer.GetComponent<PlayerModelReplacer>().IsDog)
                {
                    var playerModelReplacer = instance.GetComponent<PlayerModelReplacer>();
                    playerModelReplacer.EnableDogModel(false);
                }
            }
        }
    }
    
}