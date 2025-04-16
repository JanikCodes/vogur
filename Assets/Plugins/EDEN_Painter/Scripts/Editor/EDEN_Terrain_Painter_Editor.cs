using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

namespace GapperGames
{
    [CustomEditor(typeof(EDEN_Terrain_Painter))]
    public class EDEN_Terrain_Painter_Editor : Editor
    {
        List<Terrain> terrains;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EDEN_Terrain_Painter eden = (EDEN_Terrain_Painter)target;
            Terrain terrain = target.GetComponent<Terrain>();

            if (GUILayout.Button("Paint Terrain"))
            {
                if(eden.affectsNeighbourTerrains)
                {
                    terrains = new List<Terrain>();
                    AddNeighbors(terrain);
                    foreach (Terrain t in terrains)
                    {
                        eden.Run(t);
                    }
                }
                else
                {
                    eden.Run(terrain);
                }
            }
        }

        public void AddNeighbors(Terrain terrain)
        {
            if(!terrains.Contains(terrain) && terrain != null)
            {
                terrains.Add(terrain);
                AddNeighbors(terrain.bottomNeighbor);
                AddNeighbors(terrain.topNeighbor);
                AddNeighbors(terrain.leftNeighbor);
                AddNeighbors(terrain.rightNeighbor);
            }
        }
    }
}
