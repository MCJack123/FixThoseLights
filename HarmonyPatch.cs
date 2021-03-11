using HarmonyLib;
using UnityEngine;

namespace FixThoseLights {
    [HarmonyPatch(typeof(FlickeringNeonSign), "OnEnable")]
    class Patch {
        static AccessTools.FieldRef<FlickeringNeonSign, RandomObjectPicker<AudioClip>> clipPickerRef = AccessTools.FieldRefAccess<FlickeringNeonSign, RandomObjectPicker<AudioClip>>("_sparksAudioClipPicker");
        static AccessTools.FieldRef<FlickeringNeonSign, AudioClip[]> clipsRef = AccessTools.FieldRefAccess<FlickeringNeonSign, AudioClip[]>("_sparksAudioClips");
        static bool Prefix(FlickeringNeonSign __instance) {
            clipPickerRef(__instance) = new CustomAudioPicker(clipsRef(__instance), 0.1f);
            return !(Configuration.PluginConfig.Instance.enabled && Configuration.PluginConfig.Instance.disableFlicker);
        }
    }

    internal class CustomAudioPicker : RandomObjectPicker<AudioClip> {
        public CustomAudioPicker(AudioClip[] clips, float interval): base(clips, interval) {}
        public override AudioClip PickRandomObject() {
            if (Configuration.PluginConfig.Instance.enabled) return null;
            return base.PickRandomObject();
        }
    }
}
