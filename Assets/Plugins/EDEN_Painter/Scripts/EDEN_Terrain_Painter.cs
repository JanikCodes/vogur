using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using static Unity.Mathematics.math;
using System.Drawing;
using System;
using UnityEngine.Rendering;

namespace GapperGames
{
    public class EDEN_Terrain_Painter : MonoBehaviour
    {
        //public TerrainLayer baseLayer;
        //public TerrainTexture[] terrainLayers;

        public EDENTextureData texturesProfile;

        public bool affectsNeighbourTerrains = true;
        public bool autoUpdate = true;

        private void OnValidate()
        {
            if(autoUpdate)
            {
                Run(GetComponent<Terrain>());
            }
        }

        public void Run(Terrain terrain)
        {
            if (texturesProfile == null) { return; }
            else if (texturesProfile.baseLayer == null) { return; }
            else if (texturesProfile.terrainLayers.Length == 0) { return; }

            int heightSize = terrain.terrainData.heightmapResolution;

            float[,] roughnessMap = Smooth(terrain);
            
            List<TerrainLayer> layers = new List<TerrainLayer>();
            layers.Add(texturesProfile.baseLayer);
            foreach(TerrainTexture t in texturesProfile.terrainLayers)
            {
                layers.Add(t.layer);
            }

            terrain.terrainData.terrainLayers = layers.ToArray();

            int alphaMapSize = terrain.terrainData.alphamapResolution;

            int width = terrain.terrainData.alphamapWidth;
            int height = terrain.terrainData.alphamapHeight;

            float[,,] alphaMap = terrain.terrainData.GetAlphamaps(0, 0, alphaMapSize, alphaMapSize);

            float OriginalTerrainHeight = terrain.terrainData.size.z;

            //Loop Over Alphamap
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    float xNorm = ((float)x) / ((float)width);
                    float zNorm = ((float)z) / ((float)height);

                    float roughness = roughnessMap[(int)(xNorm * heightSize), (int)(zNorm * heightSize)];

                    Vector3 normal = normalize(terrain.terrainData.GetInterpolatedNormal(zNorm, xNorm));
                    float gradient = 1 - normal.y;
                    gradient *= 90;
                    float terrainHeight = terrain.terrainData.GetInterpolatedHeight(zNorm, xNorm);

                    alphaMap[x, z, 0] = 1;

                    for (int i = 0; i < texturesProfile.terrainLayers.Length; i++)
                    {
                        TerrainTexture t = texturesProfile.terrainLayers[i];

                        //Noise
                        float noise = Mathf.PerlinNoise(xNorm * t.noiseScale, zNorm * t.noiseScale) + Mathf.PerlinNoise(xNorm * t.noiseScale * 2, zNorm * t.noiseScale * 2);
                        noise -= 1;
                        noise *= t.noiseMagnitude;

                        //Steepness Mask

                        float lowAngle = smoothstep(t.minAngle, t.minAngle + (t.smoothness * 20), gradient + (noise / 3));
                        float highAngle = 1 - smoothstep(t.maxAngle, t.maxAngle + (t.smoothness * 20), gradient + (noise / 3));

                        float angleMask = lowAngle * highAngle;

                        angleMask = t.invertAngleMask ? 1 - angleMask : angleMask;

                        //Height Mask

                        float lowHeight = smoothstep(t.heightRange.x, t.heightRange.x + (t.smoothness * OriginalTerrainHeight * 0.025f), terrainHeight + noise);
                        float highHeight = 1 - smoothstep(t.heightRange.y, t.heightRange.y + (t.smoothness * OriginalTerrainHeight * 0.025f), terrainHeight + noise);

                        float heightMask = lowHeight * highHeight;

                        heightMask = t.invertHeightMask ? 1 - heightMask : heightMask;

                        //Roughness Mask

                        float roughnessMask = smoothstep(pow(1 - t.roughnessThreshold, 10), (pow(1 - t.roughnessThreshold, 10)) + (pow(t.smoothness, 0.8f) / 12), roughness * 50);

                        roughnessMask = t.invertRoughnessMask ? 1 - roughnessMask : roughnessMask;

                        bool isRoughness = roughness * 50 >= 1 - t.roughnessThreshold;

                        if (t.invertRoughnessMask) { isRoughness = !isRoughness; }

                        //Assign Alphamap Values

                        alphaMap[x, z, i + 1] = roughnessMask * angleMask * heightMask;
                    }
                }
            }

            //Clean Up Alphamap
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    for (int i = 0; i <= texturesProfile.terrainLayers.Length; i++)
                    {
                        for (int j = i + 1; j <= texturesProfile.terrainLayers.Length; j++)
                        {
                            alphaMap[x, z, i] *= 1 - alphaMap[x, z, j];
                        }
                    }
                }
            }

            terrain.terrainData.SetAlphamaps(0, 0, alphaMap);
        }

        public float[,] Smooth(Terrain terrain)
        {
            int sWidth = 2;

            //Initialize Variables
            int heightSize = terrain.terrainData.heightmapResolution;
            float[,] _heightMap = terrain.terrainData.GetHeights(0, 0, heightSize, heightSize);
            float[,] smoothedHeightmap = terrain.terrainData.GetHeights(0, 0, heightSize, heightSize);

            //Loop Over Heightmap
            for (int x = 0; x < heightSize; x++)
            {
                for (int z = 0; z < heightSize; z++)
                {
                    //Get Average Height
                    float total = 0;
                    for (int a = -sWidth; a <= sWidth; a++)
                    {
                        for (int b = -sWidth; b <= sWidth; b++)
                        {
                            int xPos = Mathf.Clamp(x + a, 0, heightSize - 1);
                            int zPos = Mathf.Clamp(z + b, 0, heightSize - 1);
                            total += _heightMap[xPos, zPos];
                        }
                    }
                    total /= ((sWidth * 2) + 1) * ((sWidth * 2) + 1);

                    smoothedHeightmap[x, z] = abs(total - _heightMap[x, z]);
                }
            }

            //Apply Changes
            return smoothedHeightmap;
        }

        //Random from 0 to 1
        float random(float2 uv)
        {
            float noise = frac(sin(dot(uv, float2(12.9898f, 78.233f))) * 43758.5453f);
            return noise;
        }

        float SmoothStep(float In, float Edge, float Smoothness)
        {
            return smoothstep(Edge, Edge + Smoothness, In);
        }
    }

    [System.Serializable]
    public class TerrainTexture
    {
        [Header("Texture")]
        public TerrainLayer layer;
        [Range(0, 1)] public float smoothness;

        [Header("Steepness Mask")]
        [Range(0, 90)] public float minAngle = 0;
        [Range(0, 90)] public float maxAngle = 90;
        public bool invertAngleMask;

        [Header("Height Mask")]
        public float2 heightRange = new float2(0, 500);
        public bool invertHeightMask;

        [Header("Roughness Mask")]
        [Range(0, 1)] public float roughnessThreshold;
        public bool invertRoughnessMask;

        [Header("Noise")]
        public float noiseScale = 50;
        public float noiseMagnitude = 1;

    }
}
