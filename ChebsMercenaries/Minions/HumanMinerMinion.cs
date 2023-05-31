using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using ChebsValheimLibrary.Common;
using ChebsValheimLibrary.Minions.AI;

namespace ChebsMercenaries.Minions
{
    public class HumanMinerMinion : HumanMinion
    {
        public static ConfigEntry<float> UpdateDelay, LookRadius, RoamRange;
        public static ConfigEntry<string> RockInternalIDsList;
        public static MemoryConfigEntry<string, List<string>> ItemsCost;

        private const string DefaultOresList =
            "rock1_mistlands,rock1_mountain,rock1_mountain_frac,rock2_heath,rock2_heath_frac,rock2_mountain,rock2_mountain_frac,Rock_3,Rock_3_frac,rock3_mountain,rock3_mountain_frac,rock3_silver,rock3_silver_frac,Rock_4,Rock_4_plains,rock4_coast,rock4_coast_frac,rock4_copper,rock4_copper_frac,rock4_forest,rock4_forest_frac,rock4_heath,rock4_heath_frac,Rock_7,Rock_destructible,rock_mistlands1,rock_mistlands1_frac,rock_mistlands2,RockDolmen_1,RockDolmen_2,RockDolmen_3,silvervein,silvervein_frac,MineRock_Tin,MineRock_Obsidian";

        public new static void CreateConfigs(BasePlugin plugin)
        {
            const string serverSynced = "HumanMiner (Server Synced)";
            UpdateDelay = plugin.Config.Bind(serverSynced, "UpdateDelay",
                6f, new ConfigDescription(
                    "The delay, in seconds, between rock/ore searching attempts. Attention: small values may impact performance.",
                    null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));
            LookRadius = plugin.Config.Bind(serverSynced, "LookRadius",
                50f, new ConfigDescription("How far it can see rock/ore. High values may damage performance.", null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));
            RoamRange = plugin.Config.Bind(serverSynced, "RoamRange",
                50f, new ConfigDescription("How far it will randomly run to in search of rock/ore.", null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));
            RockInternalIDsList = plugin.Config.Bind(serverSynced, "RockInternalIDsList",
                DefaultOresList, new ConfigDescription(
                    "The types of rock the miner will attempt to mine. Internal IDs only.", null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));
            var itemsCost = plugin.ModConfig(serverSynced, "ItemsCost", "CookedMeat|Coins:5,HardAntler:1",
                "The items that are consumed when creating a minion. Please use a comma-delimited list of prefab names with a : and integer for amount. Alternative items can be specified with a | eg. Wood|Coal:5 to mean wood and/or coal.",
                null, true);
            ItemsCost = new MemoryConfigEntry<string, List<string>>(itemsCost, s => s?.Split(',').ToList());

            SyncInternalsWithConfigs();
        }

        public static void SyncInternalsWithConfigs()
        {
            // awful stuff. Is there a better way?
            MinerAI.UpdateDelay = UpdateDelay.Value;
            MinerAI.LookRadius = LookRadius.Value;
            MinerAI.RoamRange = RoamRange.Value;
            MinerAI.RockInternalIDsList = RockInternalIDsList.Value;
        }

        public void Awake()
        {
            canBeCommanded = false;

            if (!TryGetComponent(out MinerAI _)) gameObject.AddComponent<MinerAI>();
        }
    }
}