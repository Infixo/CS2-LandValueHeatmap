using System.Runtime.CompilerServices;
using Colossal.Serialization.Entities;
using Game.City;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Scripting;
using Game;
using Game.Rendering;

namespace LandValueHeatmap;

[CompilerGenerated]
public class OverlayInfomodeSystem : GameSystemBase
{
    [BurstCompile]
    private struct ClearJob : IJob
    {
        public NativeArray<byte> m_TextureData;

        public void Execute()
        {
            for (int i = 0; i < m_TextureData.Length; i++)
            {
                m_TextureData[i] = 0;
            }
        }
    }

    [BurstCompile]
    private struct GroundWaterJob : IJob
    {
        [ReadOnly]
        public InfomodeActive m_ActiveData;

        [ReadOnly]
        public CellMapData<GroundWater> m_MapData;

        public NativeArray<byte> m_TextureData;

        public void Execute()
        {
            int num = m_ActiveData.m_Index - 1;
            for (int i = 0; i < m_MapData.m_TextureSize.y; i++)
            {
                for (int j = 0; j < m_MapData.m_TextureSize.x; j++)
                {
                    int num2 = j + i * m_MapData.m_TextureSize.x;
                    GroundWater groundWater = m_MapData.m_Buffer[num2];
                    m_TextureData[num2 * 4 + num] = (byte)math.clamp(groundWater.m_Amount / 32, 0, 255);
                }
            }
        }
    }

    [BurstCompile]
    private struct GroundPollutionJob : IJob
    {
        [ReadOnly]
        public InfomodeActive m_ActiveData;

        [ReadOnly]
        public CellMapData<GroundPollution> m_MapData;

        public NativeArray<byte> m_TextureData;

        public float m_Multiplier;

        public void Execute()
        {
            int num = m_ActiveData.m_Index - 1;
            for (int i = 0; i < m_MapData.m_TextureSize.y; i++)
            {
                for (int j = 0; j < m_MapData.m_TextureSize.x; j++)
                {
                    int num2 = j + i * m_MapData.m_TextureSize.x;
                    GroundPollution groundPollution = m_MapData.m_Buffer[num2];
                    m_TextureData[num2 * 4 + num] = (byte)math.clamp(Mathf.RoundToInt((float)groundPollution.m_Pollution * m_Multiplier), 0, 255);
                }
            }
        }
    }

    [BurstCompile]
    private struct NoisePollutionJob : IJob
    {
        [ReadOnly]
        public InfomodeActive m_ActiveData;

        [ReadOnly]
        public CellMapData<NoisePollution> m_MapData;

        public NativeArray<byte> m_TextureData;

        public float m_Multiplier;

        public void Execute()
        {
            int num = m_ActiveData.m_Index - 1;
            for (int i = 0; i < m_MapData.m_TextureSize.y; i++)
            {
                for (int j = 0; j < m_MapData.m_TextureSize.x; j++)
                {
                    int num2 = j + i * m_MapData.m_TextureSize.x;
                    NoisePollution noisePollution = m_MapData.m_Buffer[num2];
                    m_TextureData[num2 * 4 + num] = (byte)math.clamp(Mathf.RoundToInt((float)noisePollution.m_Pollution * m_Multiplier), 0, 255);
                }
            }
        }
    }

    [BurstCompile]
    private struct AirPollutionJob : IJob
    {
        [ReadOnly]
        public InfomodeActive m_ActiveData;

        [ReadOnly]
        public CellMapData<AirPollution> m_MapData;

        public NativeArray<byte> m_TextureData;

        public float m_Multiplier;

        public void Execute()
        {
            int num = m_ActiveData.m_Index - 1;
            for (int i = 0; i < m_MapData.m_TextureSize.y; i++)
            {
                for (int j = 0; j < m_MapData.m_TextureSize.x; j++)
                {
                    int num2 = j + i * m_MapData.m_TextureSize.x;
                    AirPollution airPollution = m_MapData.m_Buffer[num2];
                    m_TextureData[num2 * 4 + num] = (byte)math.clamp(Mathf.RoundToInt((float)airPollution.m_Pollution * m_Multiplier), 0, 255);
                }
            }
        }
    }

    [BurstCompile]
    private struct WindJob : IJob
    {
        [ReadOnly]
        public CellMapData<Wind> m_MapData;

        public NativeArray<half4> m_TextureData;

        public void Execute()
        {
            for (int i = 0; i < m_MapData.m_TextureSize.y; i++)
            {
                for (int j = 0; j < m_MapData.m_TextureSize.x; j++)
                {
                    int index = j + i * m_MapData.m_TextureSize.x;
                    Wind wind = m_MapData.m_Buffer[index];
                    m_TextureData[index] = new half4((half)wind.m_Wind.x, (half)wind.m_Wind.y, (half)0f, (half)0f);
                }
            }
        }
    }

    [BurstCompile]
    private struct TelecomCoverageJob : IJob
    {
        [ReadOnly]
        public InfomodeActive m_ActiveData;

        [ReadOnly]
        public CellMapData<TelecomCoverage> m_MapData;

        public NativeArray<byte> m_TextureData;

        public void Execute()
        {
            int num = m_ActiveData.m_Index - 1;
            for (int i = 0; i < m_MapData.m_TextureSize.y; i++)
            {
                for (int j = 0; j < m_MapData.m_TextureSize.x; j++)
                {
                    int num2 = j + i * m_MapData.m_TextureSize.x;
                    TelecomCoverage telecomCoverage = m_MapData.m_Buffer[num2];
                    m_TextureData[num2 * 4 + num] = (byte)math.clamp(telecomCoverage.networkQuality, 0, 255);
                }
            }
        }
    }

    [BurstCompile]
    private struct FertilityJob : IJob
    {
        [ReadOnly]
        public InfomodeActive m_ActiveData;

        [ReadOnly]
        public CellMapData<NaturalResourceCell> m_MapData;

        public NativeArray<byte> m_TextureData;

        public void Execute()
        {
            int num = m_ActiveData.m_Index - 1;
            for (int i = 0; i < m_MapData.m_TextureSize.y; i++)
            {
                for (int j = 0; j < m_MapData.m_TextureSize.x; j++)
                {
                    int num2 = j + i * m_MapData.m_TextureSize.x;
                    NaturalResourceCell naturalResourceCell = m_MapData.m_Buffer[num2];
                    float num3 = (int)naturalResourceCell.m_Fertility.m_Base;
                    num3 -= (float)(int)naturalResourceCell.m_Fertility.m_Used;
                    num3 = math.saturate(num3 * 0.0001f);
                    m_TextureData[num2 * 4 + num] = (byte)math.clamp(Mathf.RoundToInt(num3 * 255f), 0, 255);
                }
            }
        }
    }

    [BurstCompile]
    private struct OreJob : IJob
    {
        [ReadOnly]
        public InfomodeActive m_ActiveData;

        [ReadOnly]
        public CellMapData<NaturalResourceCell> m_MapData;

        [ReadOnly]
        public Entity m_City;

        [ReadOnly]
        public BufferLookup<CityModifier> m_CityModifiers;

        public NativeArray<byte> m_TextureData;

        public void Execute()
        {
            int num = m_ActiveData.m_Index - 1;
            DynamicBuffer<CityModifier> modifiers = default(DynamicBuffer<CityModifier>);
            if (m_CityModifiers.HasBuffer(m_City))
            {
                modifiers = m_CityModifiers[m_City];
            }
            for (int i = 0; i < m_MapData.m_TextureSize.y; i++)
            {
                for (int j = 0; j < m_MapData.m_TextureSize.x; j++)
                {
                    int num2 = j + i * m_MapData.m_TextureSize.x;
                    NaturalResourceCell naturalResourceCell = m_MapData.m_Buffer[num2];
                    float value = (int)naturalResourceCell.m_Ore.m_Base;
                    if (modifiers.IsCreated)
                    {
                        CityUtils.ApplyModifier(ref value, modifiers, CityModifierType.OreResourceAmount);
                    }
                    value -= (float)(int)naturalResourceCell.m_Ore.m_Used;
                    value = math.saturate(value * 0.0001f);
                    m_TextureData[num2 * 4 + num] = (byte)math.clamp(Mathf.RoundToInt(value * 255f), 0, 255);
                }
            }
        }
    }

    [BurstCompile]
    private struct OilJob : IJob
    {
        [ReadOnly]
        public InfomodeActive m_ActiveData;

        [ReadOnly]
        public CellMapData<NaturalResourceCell> m_MapData;

        [ReadOnly]
        public Entity m_City;

        [ReadOnly]
        public BufferLookup<CityModifier> m_CityModifiers;

        public NativeArray<byte> m_TextureData;

        public void Execute()
        {
            int num = m_ActiveData.m_Index - 1;
            DynamicBuffer<CityModifier> modifiers = default(DynamicBuffer<CityModifier>);
            if (m_CityModifiers.HasBuffer(m_City))
            {
                modifiers = m_CityModifiers[m_City];
            }
            for (int i = 0; i < m_MapData.m_TextureSize.y; i++)
            {
                for (int j = 0; j < m_MapData.m_TextureSize.x; j++)
                {
                    int num2 = j + i * m_MapData.m_TextureSize.x;
                    NaturalResourceCell naturalResourceCell = m_MapData.m_Buffer[num2];
                    float value = (int)naturalResourceCell.m_Oil.m_Base;
                    if (modifiers.IsCreated)
                    {
                        CityUtils.ApplyModifier(ref value, modifiers, CityModifierType.OilResourceAmount);
                    }
                    value -= (float)(int)naturalResourceCell.m_Oil.m_Used;
                    value = math.saturate(value * 0.0001f);
                    m_TextureData[num2 * 4 + num] = (byte)math.clamp(Mathf.RoundToInt(value * 255f), 0, 255);
                }
            }
        }
    }

    [BurstCompile]
    private struct LandValueJob : IJob
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
                    m_TextureData[num2 * 4 + num] = (byte)math.clamp(Mathf.RoundToInt(landValueCell.m_LandValue * 0.51f), 0, 255);
                }
            }
        }
    }

    [BurstCompile]
    private struct PopulationJob : IJob
    {
        [ReadOnly]
        public InfomodeActive m_ActiveData;

        [ReadOnly]
        public CellMapData<PopulationCell> m_MapData;

        public NativeArray<byte> m_TextureData;

        public void Execute()
        {
            int num = m_ActiveData.m_Index - 1;
            for (int i = 0; i < m_MapData.m_TextureSize.y; i++)
            {
                for (int j = 0; j < m_MapData.m_TextureSize.x; j++)
                {
                    int num2 = j + i * m_MapData.m_TextureSize.x;
                    PopulationCell populationCell = m_MapData.m_Buffer[num2];
                    m_TextureData[num2 * 4 + num] = (byte)math.clamp(Mathf.RoundToInt(populationCell.Get() * 0.24902344f), 0, 255);
                }
            }
        }
    }

    [BurstCompile]
    private struct AttractionJob : IJob
    {
        [ReadOnly]
        public InfomodeActive m_ActiveData;

        [ReadOnly]
        public CellMapData<AvailabilityInfoCell> m_MapData;

        public NativeArray<byte> m_TextureData;

        public void Execute()
        {
            int num = m_ActiveData.m_Index - 1;
            for (int i = 0; i < m_MapData.m_TextureSize.y; i++)
            {
                for (int j = 0; j < m_MapData.m_TextureSize.x; j++)
                {
                    int num2 = j + i * m_MapData.m_TextureSize.x;
                    AvailabilityInfoCell availabilityInfoCell = m_MapData.m_Buffer[num2];
                    m_TextureData[num2 * 4 + num] = (byte)math.clamp(Mathf.RoundToInt(availabilityInfoCell.m_AvailabilityInfo.x * 15.9375f), 0, 255);
                }
            }
        }
    }

    [BurstCompile]
    private struct CustomerJob : IJob
    {
        [ReadOnly]
        public InfomodeActive m_ActiveData;

        [ReadOnly]
        public CellMapData<AvailabilityInfoCell> m_MapData;

        public NativeArray<byte> m_TextureData;

        public void Execute()
        {
            int num = m_ActiveData.m_Index - 1;
            for (int i = 0; i < m_MapData.m_TextureSize.y; i++)
            {
                for (int j = 0; j < m_MapData.m_TextureSize.x; j++)
                {
                    int num2 = j + i * m_MapData.m_TextureSize.x;
                    AvailabilityInfoCell availabilityInfoCell = m_MapData.m_Buffer[num2];
                    m_TextureData[num2 * 4 + num] = (byte)math.clamp(Mathf.RoundToInt(availabilityInfoCell.m_AvailabilityInfo.y * 15.9375f), 0, 255);
                }
            }
        }
    }

    [BurstCompile]
    private struct WorkplaceJob : IJob
    {
        [ReadOnly]
        public InfomodeActive m_ActiveData;

        [ReadOnly]
        public CellMapData<AvailabilityInfoCell> m_MapData;

        public NativeArray<byte> m_TextureData;

        public void Execute()
        {
            int num = m_ActiveData.m_Index - 1;
            for (int i = 0; i < m_MapData.m_TextureSize.y; i++)
            {
                for (int j = 0; j < m_MapData.m_TextureSize.x; j++)
                {
                    int num2 = j + i * m_MapData.m_TextureSize.x;
                    AvailabilityInfoCell availabilityInfoCell = m_MapData.m_Buffer[num2];
                    m_TextureData[num2 * 4 + num] = (byte)math.clamp(Mathf.RoundToInt(availabilityInfoCell.m_AvailabilityInfo.z * 15.9375f), 0, 255);
                }
            }
        }
    }

    [BurstCompile]
    private struct ServiceJob : IJob
    {
        [ReadOnly]
        public InfomodeActive m_ActiveData;

        [ReadOnly]
        public CellMapData<AvailabilityInfoCell> m_MapData;

        public NativeArray<byte> m_TextureData;

        public void Execute()
        {
            int num = m_ActiveData.m_Index - 1;
            for (int i = 0; i < m_MapData.m_TextureSize.y; i++)
            {
                for (int j = 0; j < m_MapData.m_TextureSize.x; j++)
                {
                    int num2 = j + i * m_MapData.m_TextureSize.x;
                    AvailabilityInfoCell availabilityInfoCell = m_MapData.m_Buffer[num2];
                    m_TextureData[num2 * 4 + num] = (byte)math.clamp(Mathf.RoundToInt(availabilityInfoCell.m_AvailabilityInfo.w * 15.9375f), 0, 255);
                }
            }
        }
    }

    [BurstCompile]
    private struct GroundWaterPollutionJob : IJob
    {
        [ReadOnly]
        public InfomodeActive m_ActiveData;

        [ReadOnly]
        public CellMapData<GroundWater> m_MapData;

        public NativeArray<byte> m_TextureData;

        public void Execute()
        {
            int num = m_ActiveData.m_Index - 1;
            for (int i = 0; i < m_MapData.m_TextureSize.y; i++)
            {
                for (int j = 0; j < m_MapData.m_TextureSize.x; j++)
                {
                    int num2 = j + i * m_MapData.m_TextureSize.x;
                    GroundWater groundWater = m_MapData.m_Buffer[num2];
                    m_TextureData[num2 * 4 + num] = (byte)math.clamp(math.min(groundWater.m_Amount / 32, groundWater.m_Polluted * 256 / math.max(1, groundWater.m_Amount)), 0, 255);
                }
            }
        }
    }

    private struct TypeHandle
    {
        [ReadOnly]
        public ComponentTypeHandle<InfoviewHeatmapData> __Game_Prefabs_InfoviewHeatmapData_RO_ComponentTypeHandle;

        [ReadOnly]
        public ComponentTypeHandle<InfomodeActive> __Game_Prefabs_InfomodeActive_RO_ComponentTypeHandle;

        [ReadOnly]
        public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void __AssignHandles(ref SystemState state)
        {
            __Game_Prefabs_InfoviewHeatmapData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewHeatmapData>(isReadOnly: true);
            __Game_Prefabs_InfomodeActive_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfomodeActive>(isReadOnly: true);
            __Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(isReadOnly: true);
        }
    }

    private TerrainRenderSystem m_TerrainRenderSystem;

    private WaterRenderSystem m_WaterRenderSystem;

    private GroundWaterSystem m_GroundWaterSystem;

    private GroundPollutionSystem m_GroundPollutionSystem;

    private NoisePollutionSystem m_NoisePollutionSystem;

    private AirPollutionSystem m_AirPollutionSystem;

    private WindSystem m_WindSystem;

    private CitySystem m_CitySystem;

    private TelecomPreviewSystem m_TelecomCoverageSystem;

    private NaturalResourceSystem m_NaturalResourceSystem;

    private LandValueToGridSystem m_LandValueToGridSystem;

    private PopulationToGridSystem m_PopulationToGridSystem;

    private AvailabilityInfoToGridSystem m_AvailabilityInfoToGridSystem;

    private ToolSystem m_ToolSystem;

    private EntityQuery m_InfomodeQuery;

    private EntityQuery m_HappinessParameterQuery;

    private Texture2D m_TerrainTexture;

    private Texture2D m_WaterTexture;

    private Texture2D m_WindTexture;

    private JobHandle m_Dependency;

    private TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
        base.OnCreate();
        m_TerrainRenderSystem = base.World.GetOrCreateSystemManaged<TerrainRenderSystem>();
        m_WaterRenderSystem = base.World.GetOrCreateSystemManaged<WaterRenderSystem>();
        m_GroundWaterSystem = base.World.GetOrCreateSystemManaged<GroundWaterSystem>();
        m_GroundPollutionSystem = base.World.GetOrCreateSystemManaged<GroundPollutionSystem>();
        m_NoisePollutionSystem = base.World.GetOrCreateSystemManaged<NoisePollutionSystem>();
        m_AirPollutionSystem = base.World.GetOrCreateSystemManaged<AirPollutionSystem>();
        m_WindSystem = base.World.GetOrCreateSystemManaged<WindSystem>();
        m_CitySystem = base.World.GetOrCreateSystemManaged<CitySystem>();
        m_TelecomCoverageSystem = base.World.GetOrCreateSystemManaged<TelecomPreviewSystem>();
        m_NaturalResourceSystem = base.World.GetOrCreateSystemManaged<NaturalResourceSystem>();
        m_ToolSystem = base.World.GetOrCreateSystemManaged<ToolSystem>();
        m_LandValueToGridSystem = base.World.GetOrCreateSystemManaged<LandValueToGridSystem>();
        m_PopulationToGridSystem = base.World.GetOrCreateSystemManaged<PopulationToGridSystem>();
        m_AvailabilityInfoToGridSystem = base.World.GetOrCreateSystemManaged<AvailabilityInfoToGridSystem>();
        m_TerrainTexture = new Texture2D(1, 1, TextureFormat.RGBA32, mipChain: false, linear: true)
        {
            name = "TerrainInfoTexture",
            hideFlags = HideFlags.HideAndDontSave
        };
        m_WaterTexture = new Texture2D(1, 1, TextureFormat.RGBA32, mipChain: false, linear: true)
        {
            name = "WaterInfoTexture",
            hideFlags = HideFlags.HideAndDontSave
        };
        m_WindTexture = new Texture2D(m_WindSystem.TextureSize.x, m_WindSystem.TextureSize.y, GraphicsFormat.R16G16B16A16_SFloat, 1, TextureCreationFlags.None)
        {
            name = "WindInfoTexture",
            hideFlags = HideFlags.HideAndDontSave
        };
        m_InfomodeQuery = GetEntityQuery(ComponentType.ReadOnly<InfomodeActive>(), ComponentType.ReadOnly<InfoviewHeatmapData>());
        m_HappinessParameterQuery = GetEntityQuery(ComponentType.ReadOnly<CitizenHappinessParameterData>());
    }

    [Preserve]
    protected override void OnDestroy()
    {
        CoreUtils.Destroy(m_TerrainTexture);
        CoreUtils.Destroy(m_WaterTexture);
        CoreUtils.Destroy(m_WindTexture);
        base.OnDestroy();
    }

    public void ApplyOverlay()
    {
        if (m_TerrainRenderSystem.overrideOverlaymap == m_TerrainTexture)
        {
            m_Dependency.Complete();
            m_TerrainTexture.Apply();
        }
        if (m_TerrainRenderSystem.overlayExtramap == m_WindTexture)
        {
            m_Dependency.Complete();
            m_WindTexture.Apply();
        }
        if (m_WaterRenderSystem.overrideOverlaymap == m_WaterTexture)
        {
            m_Dependency.Complete();
            m_WaterTexture.Apply();
        }
    }

    private NativeArray<byte> GetTerrainTextureData<T>(CellMapData<T> cellMapData) where T : struct, ISerializable
    {
        return GetTerrainTextureData(cellMapData.m_TextureSize);
    }

    private NativeArray<byte> GetTerrainTextureData(int2 size)
    {
        if (m_TerrainTexture.width != size.x || m_TerrainTexture.height != size.y)
        {
            m_TerrainTexture.Reinitialize(size.x, size.y);
            m_TerrainRenderSystem.overrideOverlaymap = null;
        }
        if (m_TerrainRenderSystem.overrideOverlaymap != m_TerrainTexture)
        {
            m_TerrainRenderSystem.overrideOverlaymap = m_TerrainTexture;
            ClearJob clearJob = default(ClearJob);
            clearJob.m_TextureData = m_TerrainTexture.GetRawTextureData<byte>();
            ClearJob jobData = clearJob;
            m_Dependency = IJobExtensions.Schedule(jobData, base.Dependency);
            base.Dependency = m_Dependency;
        }
        return m_TerrainTexture.GetRawTextureData<byte>();
    }

    private NativeArray<byte> GetWaterTextureData<T>(CellMapData<T> cellMapData) where T : struct, ISerializable
    {
        return GetWaterTextureData(cellMapData.m_TextureSize);
    }

    private NativeArray<byte> GetWaterTextureData(int2 size)
    {
        if (m_WaterTexture.width != size.x || m_WaterTexture.height != size.y)
        {
            m_WaterTexture.Reinitialize(size.x, size.y);
            m_WaterRenderSystem.overrideOverlaymap = null;
        }
        if (m_WaterRenderSystem.overrideOverlaymap != m_WaterTexture)
        {
            m_WaterRenderSystem.overrideOverlaymap = m_WaterTexture;
            ClearJob clearJob = default(ClearJob);
            clearJob.m_TextureData = m_WaterTexture.GetRawTextureData<byte>();
            ClearJob jobData = clearJob;
            m_Dependency = IJobExtensions.Schedule(jobData, base.Dependency);
            base.Dependency = m_Dependency;
        }
        return m_WaterTexture.GetRawTextureData<byte>();
    }

    [Preserve]
    protected override void OnUpdate()
    {
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
                    case HeatmapData.GroundWater:
                    {
                        GroundWaterJob groundWaterJob = default(GroundWaterJob);
                        groundWaterJob.m_ActiveData = activeData;
                        groundWaterJob.m_MapData = m_GroundWaterSystem.GetData(readOnly: true, out var dependencies16);
                        GroundWaterJob jobData16 = groundWaterJob;
                        jobData16.m_TextureData = GetTerrainTextureData(jobData16.m_MapData);
                        JobHandle jobHandle16 = IJobExtensions.Schedule(jobData16, JobHandle.CombineDependencies(base.Dependency, dependencies16));
                        m_GroundWaterSystem.AddReader(jobHandle16);
                        m_Dependency = jobHandle16;
                        base.Dependency = jobHandle16;
                        break;
                    }
                    case HeatmapData.GroundPollution:
                    {
                        CitizenHappinessParameterData singleton3 = m_HappinessParameterQuery.GetSingleton<CitizenHappinessParameterData>();
                        GroundPollutionJob groundPollutionJob = default(GroundPollutionJob);
                        groundPollutionJob.m_ActiveData = activeData;
                        groundPollutionJob.m_MapData = m_GroundPollutionSystem.GetData(readOnly: true, out var dependencies15);
                        groundPollutionJob.m_Multiplier = 256f / ((float)singleton3.m_MaxAirAndGroundPollutionBonus * (float)singleton3.m_PollutionBonusDivisor);
                        GroundPollutionJob jobData15 = groundPollutionJob;
                        jobData15.m_TextureData = GetTerrainTextureData(jobData15.m_MapData);
                        JobHandle jobHandle15 = IJobExtensions.Schedule(jobData15, JobHandle.CombineDependencies(base.Dependency, dependencies15));
                        m_GroundPollutionSystem.AddReader(jobHandle15);
                        m_Dependency = jobHandle15;
                        base.Dependency = jobHandle15;
                        break;
                    }
                    case HeatmapData.AirPollution:
                    {
                        CitizenHappinessParameterData singleton2 = m_HappinessParameterQuery.GetSingleton<CitizenHappinessParameterData>();
                        AirPollutionJob airPollutionJob = default(AirPollutionJob);
                        airPollutionJob.m_ActiveData = activeData;
                        airPollutionJob.m_MapData = m_AirPollutionSystem.GetData(readOnly: true, out var dependencies14);
                        airPollutionJob.m_Multiplier = 256f / ((float)singleton2.m_MaxAirAndGroundPollutionBonus * (float)singleton2.m_PollutionBonusDivisor);
                        AirPollutionJob jobData14 = airPollutionJob;
                        jobData14.m_TextureData = GetTerrainTextureData(jobData14.m_MapData);
                        JobHandle jobHandle14 = IJobExtensions.Schedule(jobData14, JobHandle.CombineDependencies(base.Dependency, dependencies14));
                        m_AirPollutionSystem.AddReader(jobHandle14);
                        m_Dependency = jobHandle14;
                        base.Dependency = jobHandle14;
                        break;
                    }
                    case HeatmapData.Noise:
                    {
                        CitizenHappinessParameterData singleton = m_HappinessParameterQuery.GetSingleton<CitizenHappinessParameterData>();
                        NoisePollutionJob noisePollutionJob = default(NoisePollutionJob);
                        noisePollutionJob.m_ActiveData = activeData;
                        noisePollutionJob.m_MapData = m_NoisePollutionSystem.GetData(readOnly: true, out var dependencies13);
                        noisePollutionJob.m_Multiplier = 256f / ((float)singleton.m_MaxNoisePollutionBonus * (float)singleton.m_PollutionBonusDivisor);
                        NoisePollutionJob jobData13 = noisePollutionJob;
                        jobData13.m_TextureData = GetTerrainTextureData(jobData13.m_MapData);
                        JobHandle jobHandle13 = IJobExtensions.Schedule(jobData13, JobHandle.CombineDependencies(base.Dependency, dependencies13));
                        m_NoisePollutionSystem.AddReader(jobHandle13);
                        m_Dependency = jobHandle13;
                        base.Dependency = jobHandle13;
                        break;
                    }
                    case HeatmapData.Wind:
                    {
                        m_TerrainRenderSystem.overlayExtramap = m_WindTexture;
                        float4 overlayArrowMask2 = default(float4);
                        overlayArrowMask2[activeData.m_Index - 1] = 1f;
                        m_TerrainRenderSystem.overlayArrowMask = overlayArrowMask2;
                        WindJob jobData12 = default(WindJob);
                        jobData12.m_MapData = m_WindSystem.GetData(readOnly: true, out var dependencies12);
                        jobData12.m_TextureData = m_WindTexture.GetRawTextureData<half4>();
                        JobHandle jobHandle12 = IJobExtensions.Schedule(jobData12, JobHandle.CombineDependencies(base.Dependency, dependencies12));
                        m_WindSystem.AddReader(jobHandle12);
                        m_Dependency = jobHandle12;
                        base.Dependency = jobHandle12;
                        break;
                    }
                    case HeatmapData.WaterFlow:
                    {
                        m_WaterRenderSystem.overlayExtramap = m_WaterRenderSystem.flowTexture;
                        float4 overlayArrowMask = default(float4);
                        overlayArrowMask[activeData.m_Index - 5] = 1f;
                        m_WaterRenderSystem.overlayArrowMask = overlayArrowMask;
                        break;
                    }
                    case HeatmapData.TelecomCoverage:
                    {
                        TelecomCoverageJob telecomCoverageJob = default(TelecomCoverageJob);
                        telecomCoverageJob.m_ActiveData = activeData;
                        telecomCoverageJob.m_MapData = m_TelecomCoverageSystem.GetData(readOnly: true, out var dependencies11);
                        TelecomCoverageJob jobData11 = telecomCoverageJob;
                        jobData11.m_TextureData = GetTerrainTextureData(jobData11.m_MapData);
                        JobHandle jobHandle11 = IJobExtensions.Schedule(jobData11, JobHandle.CombineDependencies(base.Dependency, dependencies11));
                        m_TelecomCoverageSystem.AddReader(jobHandle11);
                        m_Dependency = jobHandle11;
                        base.Dependency = jobHandle11;
                        break;
                    }
                    case HeatmapData.Fertility:
                    {
                        FertilityJob fertilityJob = default(FertilityJob);
                        fertilityJob.m_ActiveData = activeData;
                        fertilityJob.m_MapData = m_NaturalResourceSystem.GetData(readOnly: true, out var dependencies10);
                        FertilityJob jobData10 = fertilityJob;
                        jobData10.m_TextureData = GetTerrainTextureData(jobData10.m_MapData);
                        JobHandle jobHandle10 = IJobExtensions.Schedule(jobData10, JobHandle.CombineDependencies(base.Dependency, dependencies10));
                        m_NaturalResourceSystem.AddReader(jobHandle10);
                        m_Dependency = jobHandle10;
                        base.Dependency = jobHandle10;
                        break;
                    }
                    case HeatmapData.Ore:
                    {
                        __TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref base.CheckedStateRef);
                        OreJob oreJob = default(OreJob);
                        oreJob.m_ActiveData = activeData;
                        oreJob.m_MapData = m_NaturalResourceSystem.GetData(readOnly: true, out var dependencies9);
                        oreJob.m_City = m_CitySystem.City;
                        oreJob.m_CityModifiers = __TypeHandle.__Game_City_CityModifier_RO_BufferLookup;
                        OreJob jobData9 = oreJob;
                        jobData9.m_TextureData = GetTerrainTextureData(jobData9.m_MapData);
                        JobHandle jobHandle9 = IJobExtensions.Schedule(jobData9, JobHandle.CombineDependencies(base.Dependency, dependencies9));
                        m_NaturalResourceSystem.AddReader(jobHandle9);
                        m_Dependency = jobHandle9;
                        base.Dependency = jobHandle9;
                        break;
                    }
                    case HeatmapData.Oil:
                    {
                        __TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref base.CheckedStateRef);
                        OilJob oilJob = default(OilJob);
                        oilJob.m_ActiveData = activeData;
                        oilJob.m_MapData = m_NaturalResourceSystem.GetData(readOnly: true, out var dependencies8);
                        oilJob.m_CityModifiers = __TypeHandle.__Game_City_CityModifier_RO_BufferLookup;
                        OilJob jobData8 = oilJob;
                        jobData8.m_TextureData = GetTerrainTextureData(jobData8.m_MapData);
                        JobHandle jobHandle8 = IJobExtensions.Schedule(jobData8, JobHandle.CombineDependencies(base.Dependency, dependencies8));
                        m_NaturalResourceSystem.AddReader(jobHandle8);
                        m_Dependency = jobHandle8;
                        base.Dependency = jobHandle8;
                        break;
                    }
                    case HeatmapData.LandValue:
                    {
                        LandValueHeatmap.LandValueJob landValueJob = default(LandValueHeatmap.LandValueJob); // modded
                        landValueJob.m_ActiveData = activeData;
                        landValueJob.m_MapData = m_LandValueToGridSystem.GetData(readOnly: true, out var dependencies7);
                        LandValueHeatmap.LandValueJob jobData7 = landValueJob; // modded
                        jobData7.m_TextureData = GetTerrainTextureData(jobData7.m_MapData);
                        JobHandle jobHandle7 = IJobExtensions.Schedule(jobData7, JobHandle.CombineDependencies(dependencies7, base.Dependency));
                        m_LandValueToGridSystem.AddReader(jobHandle7);
                        m_Dependency = jobHandle7;
                        base.Dependency = jobHandle7;
                        break;
                    }
                    case HeatmapData.Population:
                    {
                        PopulationJob populationJob = default(PopulationJob);
                        populationJob.m_ActiveData = activeData;
                        populationJob.m_MapData = m_PopulationToGridSystem.GetData(readOnly: true, out var dependencies6);
                        PopulationJob jobData6 = populationJob;
                        jobData6.m_TextureData = GetTerrainTextureData(jobData6.m_MapData);
                        JobHandle jobHandle6 = IJobExtensions.Schedule(jobData6, JobHandle.CombineDependencies(dependencies6, base.Dependency));
                        m_PopulationToGridSystem.AddReader(jobHandle6);
                        m_Dependency = jobHandle6;
                        base.Dependency = jobHandle6;
                        break;
                    }
                    case HeatmapData.Attraction:
                    {
                        AttractionJob attractionJob = default(AttractionJob);
                        attractionJob.m_ActiveData = activeData;
                        attractionJob.m_MapData = m_AvailabilityInfoToGridSystem.GetData(readOnly: true, out var dependencies5);
                        AttractionJob jobData5 = attractionJob;
                        jobData5.m_TextureData = GetTerrainTextureData(jobData5.m_MapData);
                        JobHandle jobHandle5 = IJobExtensions.Schedule(jobData5, JobHandle.CombineDependencies(base.Dependency, dependencies5));
                        m_AvailabilityInfoToGridSystem.AddReader(jobHandle5);
                        m_Dependency = jobHandle5;
                        base.Dependency = jobHandle5;
                        break;
                    }
                    case HeatmapData.Customers:
                    {
                        CustomerJob customerJob = default(CustomerJob);
                        customerJob.m_ActiveData = activeData;
                        customerJob.m_MapData = m_AvailabilityInfoToGridSystem.GetData(readOnly: true, out var dependencies4);
                        CustomerJob jobData4 = customerJob;
                        jobData4.m_TextureData = GetTerrainTextureData(jobData4.m_MapData);
                        JobHandle jobHandle4 = IJobExtensions.Schedule(jobData4, JobHandle.CombineDependencies(base.Dependency, dependencies4));
                        m_AvailabilityInfoToGridSystem.AddReader(jobHandle4);
                        m_Dependency = jobHandle4;
                        base.Dependency = jobHandle4;
                        break;
                    }
                    case HeatmapData.Workplaces:
                    {
                        WorkplaceJob workplaceJob = default(WorkplaceJob);
                        workplaceJob.m_ActiveData = activeData;
                        workplaceJob.m_MapData = m_AvailabilityInfoToGridSystem.GetData(readOnly: true, out var dependencies3);
                        WorkplaceJob jobData3 = workplaceJob;
                        jobData3.m_TextureData = GetTerrainTextureData(jobData3.m_MapData);
                        JobHandle jobHandle3 = IJobExtensions.Schedule(jobData3, JobHandle.CombineDependencies(base.Dependency, dependencies3));
                        m_AvailabilityInfoToGridSystem.AddReader(jobHandle3);
                        m_Dependency = jobHandle3;
                        base.Dependency = jobHandle3;
                        break;
                    }
                    case HeatmapData.Services:
                    {
                        ServiceJob serviceJob = default(ServiceJob);
                        serviceJob.m_ActiveData = activeData;
                        serviceJob.m_MapData = m_AvailabilityInfoToGridSystem.GetData(readOnly: true, out var dependencies2);
                        ServiceJob jobData2 = serviceJob;
                        jobData2.m_TextureData = GetTerrainTextureData(jobData2.m_MapData);
                        JobHandle jobHandle2 = IJobExtensions.Schedule(jobData2, JobHandle.CombineDependencies(base.Dependency, dependencies2));
                        m_AvailabilityInfoToGridSystem.AddReader(jobHandle2);
                        m_Dependency = jobHandle2;
                        base.Dependency = jobHandle2;
                        break;
                    }
                    case HeatmapData.GroundWaterPollution:
                    {
                        GroundWaterPollutionJob groundWaterPollutionJob = default(GroundWaterPollutionJob);
                        groundWaterPollutionJob.m_ActiveData = activeData;
                        groundWaterPollutionJob.m_MapData = m_GroundWaterSystem.GetData(readOnly: true, out var dependencies);
                        GroundWaterPollutionJob jobData = groundWaterPollutionJob;
                        jobData.m_TextureData = GetTerrainTextureData(jobData.m_MapData);
                        JobHandle jobHandle = IJobExtensions.Schedule(jobData, JobHandle.CombineDependencies(base.Dependency, dependencies));
                        m_GroundWaterSystem.AddReader(jobHandle);
                        m_Dependency = jobHandle;
                        base.Dependency = jobHandle;
                        break;
                    }
                    case HeatmapData.WaterPollution:
                    {
                        float4 overlayPollutionMask = default(float4);
                        overlayPollutionMask[activeData.m_Index - 5] = 1f;
                        m_WaterRenderSystem.overlayPollutionMask = overlayPollutionMask;
                        break;
                    }
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
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
    }

    protected override void OnCreateForCompiler()
    {
        base.OnCreateForCompiler();
        __AssignQueries(ref base.CheckedStateRef);
        __TypeHandle.__AssignHandles(ref base.CheckedStateRef);
    }

    [Preserve]
    public OverlayInfomodeSystem()
    {
    }
}
