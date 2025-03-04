﻿using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace PlayerDogModel_Plus.Patches
{
    internal class JetpackPatch
    {
        [HarmonyPatch(typeof(JetpackItem), nameof(JetpackItem.LateUpdate))]
        class LateUpdatePatch
        {
            static void Postfix(JetpackItem __instance, PlayerControllerB ___playerHeldBy, bool ___isHeld)
            {
                if (___playerHeldBy != null && ___isHeld)
                {
                    PlayerModelReplacer replacer = null;
                    foreach (GameObject player in StartOfRound.Instance.allPlayerObjects)
                    {
                        var currentReplacer = player.GetComponent<PlayerModelReplacer>();
                        if (currentReplacer != null && currentReplacer.PlayerClientId == ___playerHeldBy.playerClientId)
                        {
                            replacer = currentReplacer;
                            break;
                        }
                    }

                    if (replacer == null || !replacer.IsDog) return; // Nothing to do.

                    Transform dogTorso = replacer.GetDogGameObject().transform.Find("Armature").Find("torso");

                    // Need to adjust the arm component of this item to line up with the item anchor
                    __instance.transform.position = dogTorso.Find("head").Find("serverItem").position;
                    __instance.transform.Rotate(__instance.backPartRotationOffset);

                    __instance.backPart.rotation = dogTorso.rotation;
                    __instance.backPart.position = dogTorso.position;

                    Vector3 vector = __instance.backPartPositionOffset;
                    vector = dogTorso.rotation * vector;
                    __instance.backPart.position += vector;
                }
            }
        }
    }
}
