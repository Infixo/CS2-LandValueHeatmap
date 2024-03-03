using HarmonyLib;

namespace LandValueHeatMap;

[HarmonyPatch]
class Patches
{
    [HarmonyPatch(typeof(Game.Common.SystemOrder), "Initialize")]
    [HarmonyPostfix]
    public static void Initialize_Postfix(UpdateSystem updateSystem)
    {
        updateSystem.UpdateAt<AgingSystem_RealPop>(SystemUpdatePhase.GameSimulation);
        updateSystem.UpdateAt<ApplyToSchoolSystem_RealPop>(SystemUpdatePhase.GameSimulation);
        updateSystem.UpdateAt<BirthSystem_RealPop>(SystemUpdatePhase.GameSimulation);
        updateSystem.UpdateAt<CountHomesSystem>(SystemUpdatePhase.GameSimulation);
        updateSystem.UpdateAt<GraduationSystem_RealPop>(SystemUpdatePhase.GameSimulation);
        updateSystem.UpdateAt<SchoolAISystem_RealPop>(SystemUpdatePhase.GameSimulation);
        updateSystem.UpdateAt<CitizenInitializeSystem_RealPop>(SystemUpdatePhase.Modification5);
        if (Plugin.DeathChanceIncrease.Value > 0)
            updateSystem.UpdateAt<DeathCheckSystem_RealPop>(SystemUpdatePhase.GameSimulation);
        else
            Plugin.Log("Using original DeathCheckSystem.");
    }

    [HarmonyPatch(typeof(Game.Simulation.AgingSystem), "OnUpdate")]
    [HarmonyPrefix]
    static bool AgingSystem_OnUpdate()
    {
        //RealPop.Debug.Log("Original AgingSystem disabled.");
        //__instance.World.GetOrCreateSystemManaged<RealPop.Systems.AgingSystem_RealPop>().Update();
        return false; // don't execute the original system
    }
}
