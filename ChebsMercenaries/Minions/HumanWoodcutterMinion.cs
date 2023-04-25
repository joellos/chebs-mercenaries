using BepInEx;
using BepInEx.Configuration;
using ChebsValheimLibrary.Minions.AI;

namespace ChebsMercenaries.Minions
{
    public class HumanWoodcutterMinion : HumanMinion
    {
        public static ConfigEntry<float> UpdateDelay, LookRadius, RoamRange;

        public new static void CreateConfigs(BaseUnityPlugin plugin)
        {
            UpdateDelay = plugin.Config.Bind("HumanWoodcutter (Server Synced)", "UpdateDelay",
                6f, new ConfigDescription("The delay, in seconds, between wood searching attempts. Attention: small values may impact performance.", null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));
            LookRadius = plugin.Config.Bind("HumanWoodcutter (Server Synced)", "LookRadius",
                50f, new ConfigDescription("How far it can see wood. High values may damage performance.", null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));
            RoamRange = plugin.Config.Bind("HumanWoodcutter (Server Synced)", "RoamRange",
                50f, new ConfigDescription("How far it will randomly run to in search of wood.", null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));
            
            SyncInternalsWithConfigs();
        }

        public static void SyncInternalsWithConfigs()
        {
            // awful stuff. Is there a better way?
            WoodcutterAI.UpdateDelay = UpdateDelay.Value;
            WoodcutterAI.LookRadius = LookRadius.Value;
            WoodcutterAI.RoamRange = RoamRange.Value;
        }

        public void Awake()
        {
            canBeCommanded = false;

            if (!TryGetComponent(out WoodcutterAI _)) gameObject.AddComponent<WoodcutterAI>();
        }
    }
}