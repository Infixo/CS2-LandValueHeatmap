using System;
using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Game;
using Game.Prefabs;
using Game.Rendering;
using Game.Simulation;
using HarmonyLib;

namespace LandValueHeatMap;

[HarmonyPatch]
class Patches
{
    [HarmonyPatch(typeof(Game.Prefabs.PrefabSystem), "AddPrefab")]
    [HarmonyPrefix]
    public static bool PrefabSystem_AddPrefab_Prefix(PrefabBase prefab)
    {
        if (prefab.GetType() == typeof(HeatmapInfomodePrefab) && prefab.name == "LandValueInfomode")
        {
            HeatmapInfomodePrefab comp = prefab as HeatmapInfomodePrefab;
            comp.m_Steps = 26;
            comp.m_Low = new Color(184f / 255f, 192f / 255f, 216f / 255f); // HSV 225-15-86
            comp.m_Medium = new Color(57f / 255f, 90f / 255f, 191f / 255f); // HSV 225-70-74
            comp.m_High = new Color(11f / 255f, 27f / 255f, 76f / 255f); // HSV 225-85-30
            Plugin.Log($"Modded {prefab.name}.{comp.m_Type}: {comp.m_Steps} steps, colors {comp.m_Low} -> {comp.m_Medium} -> {comp.m_High}");
        }
        if (prefab.GetType() == typeof(BuildingStatusInfomodePrefab) && prefab.name == "Building Land Value")
        {
            BuildingStatusInfomodePrefab comp = prefab as BuildingStatusInfomodePrefab;
            comp.m_Steps = 26;
            comp.m_Range = new Colossal.Mathematics.Bounds1(0f, 5000f); // See Note - it divides actual PerCell value by 2
            comp.m_Low = new Color(184f / 255f, 192f / 255f, 216f / 255f); // HSV 225-15-86
            comp.m_Medium = new Color(57f / 255f, 90f / 255f, 191f / 255f); // HSV 225-70-74
            comp.m_High = new Color(11f / 255f, 27f / 255f, 76f / 255f); // HSV 225-85-30
            Plugin.Log($"Modded {prefab.name}.{comp.m_Type}: {comp.m_Steps} steps, colors {comp.m_Low} -> {comp.m_Medium} -> {comp.m_High}, range {comp.m_Range}");
            // Note: Game.Rendering.ObjectColorSystem.UpdateObjectColorsJob.GetBuildingStatusColors calculates colors
            // Formula: (MaxRent - Upkeep/Exp) / (2 * LotSize)
        }
        return true;
    }

    [HarmonyPatch(typeof(Game.Common.SystemOrder), "Initialize")]
    [HarmonyPostfix]
    public static void Initialize_Postfix(UpdateSystem updateSystem)
    {
        updateSystem.UpdateAt<LandValueHeatMap.OverlayInfomodeSystem>(SystemUpdatePhase.PreCulling);
    }

    /*
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(Game.Rendering.OverlayInfomodeSystem), "GetTerrainTextureData")]
    public static NativeArray<byte> GetTerrainTextureData(int2 size) =>
        // its a stub so it has no initial content
        throw new NotImplementedException("It's a stub");
    */

    [HarmonyPatch(typeof(Game.Rendering.OverlayInfomodeSystem), "OnUpdate")]
    [HarmonyPrefix]
    static bool OverlayInfomodeSystem_OnUpdate(
        //Game.Rendering.OverlayInfomodeSystem __instance,
        //JobHandle ___Dependency,
        //JobHandle ___m_Dependency,
        //LandValueToGridSystem ___m_LandValueToGridSystem
        )
    {


        /*
        m_TerrainRenderSystem.overrideOverlaymap = null;
        m_TerrainRenderSystem.overlayExtramap = null;
        m_TerrainRenderSystem.overlayArrowMask = default(float4);
        m_WaterRenderSystem.overrideOverlaymap = null;
        m_WaterRenderSystem.overlayExtramap = null;
        m_WaterRenderSystem.overlayPollutionMask = default(float4);
        m_WaterRenderSystem.overlayArrowMask = default(float4);
        if (!m_InfomodeQuery.IsEmptyIgnoreFilter)
        {
            NativeArray<ArchetypeChunk> nativeArray = m_InfomodeQuery.ToArchetypeChunkArray(Allocator.TempJob);
            __TypeHandle.__Game_Prefabs_InfoviewHeatmapData_RO_ComponentTypeHandle.Update(ref base.CheckedStateRef);
            ComponentTypeHandle<InfoviewHeatmapData> typeHandle = __TypeHandle.__Game_Prefabs_InfoviewHeatmapData_RO_ComponentTypeHandle;
            __TypeHandle.__Game_Prefabs_InfomodeActive_RO_ComponentTypeHandle.Update(ref base.CheckedStateRef);
            ComponentTypeHandle<InfomodeActive> typeHandle2 = __TypeHandle.__Game_Prefabs_InfomodeActive_RO_ComponentTypeHandle;
            for (int i = 0; i < nativeArray.Length; i++)
            {
                ArchetypeChunk archetypeChunk = nativeArray[i];
                NativeArray<InfoviewHeatmapData> nativeArray2 = archetypeChunk.GetNativeArray(ref typeHandle);
                NativeArray<InfomodeActive> nativeArray3 = archetypeChunk.GetNativeArray(ref typeHandle2);
                for (int j = 0; j < nativeArray2.Length; j++)
                {
                    InfoviewHeatmapData infoviewHeatmapData = nativeArray2[j];
                    InfomodeActive activeData = nativeArray3[j];
                    switch (infoviewHeatmapData.m_Type)
                    {
                        case HeatmapData.LandValue:
                        {
                            LandValueJob landValueJob = default(LandValueJob);
                            landValueJob.m_ActiveData = activeData;
                            landValueJob.m_MapData = ___m_LandValueToGridSystem.GetData(readOnly: true, out var dependencies7);
                            LandValueJob jobData7 = landValueJob;
                            jobData7.m_TextureData = GetTerrainTextureData(jobData7.m_MapData.m_TextureSize);
                            JobHandle jobHandle7 = IJobExtensions.Schedule(jobData7, JobHandle.CombineDependencies(dependencies7, base.Dependency));
                            ___m_LandValueToGridSystem.AddReader(jobHandle7);
                            ___m_Dependency = jobHandle7;
                            base.Dependency = jobHandle7;
                            break;
                        }
                        default:
                            break;
                    }
                }
            }
            nativeArray.Dispose();
        }
        if (m_ToolSystem.activeInfoview != null)
        {
            if (m_TerrainRenderSystem.overrideOverlaymap == null)
            {
                GetTerrainTextureData(1);
            }
            if (m_WaterRenderSystem.overrideOverlaymap == null)
            {
                GetWaterTextureData(1);
            }
        }

        */
        return false; // don't execute the original system
    }

    [HarmonyPatch(typeof(Game.Rendering.TerrainRenderSystem), "OnCreate")]
    [HarmonyPostfix]
    static void TerrainRenderSystem_OnCreate(TerrainRenderSystem __instance, ref OverlayInfomodeSystem ___m_OverlayInfomodeSystem)
    {
        ___m_OverlayInfomodeSystem = __instance.World.GetOrCreateSystemManaged<LandValueHeatMap.OverlayInfomodeSystem>();
    }

}

[BurstCompile]
public struct LandValueJob : IJob
{
    [ReadOnly]
    public InfomodeActive m_ActiveData;

    [ReadOnly]
    public CellMapData<LandValueCell> m_MapData;

    public NativeArray<byte> m_TextureData;

    public void Execute()
    {
        int num = m_ActiveData.m_Index - 1;
        for (int i = 0; i < m_MapData.m_TextureSize.y; i++)
        {
            for (int j = 0; j < m_MapData.m_TextureSize.x; j++)
            {
                int num2 = j + i * m_MapData.m_TextureSize.x;
                LandValueCell landValueCell = m_MapData.m_Buffer[num2];
                // Modded scaling - vanilla game uses 0.51f so basically it saturates at LV=500
                m_TextureData[num2 * 4 + num] = (byte)math.clamp(Mathf.RoundToInt(landValueCell.m_LandValue * 0.1f), 0, 255); // 0.1275f=2000/255
            }
        }
    }
}
