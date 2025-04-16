using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GapperGames
{
    [CreateAssetMenu(fileName = "new Texture Data", menuName = "EDEN/ New Texture Data")]
    public class EDENTextureData : ScriptableObject
    {
        public TerrainLayer baseLayer;
        public TerrainTexture[] terrainLayers;

        private void OnValidate()
        {
            EDEN_Terrain_Painter[] terrains = FindObjectsOfType<EDEN_Terrain_Painter>();

            foreach(EDEN_Terrain_Painter t in terrains)
            {
                if (t.autoUpdate)
                {
                    t.Run(t.GetComponent<Terrain>());
                }
            }
        }
    }
}

