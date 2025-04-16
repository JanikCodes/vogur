/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022 - 2023 Renowned Games All rights reserved.
   ================================================================ */

using System.Collections.Generic;
using UnityEngine;

namespace RenownedGames.AITree
{
    public class Group : ScriptableObject
    {
        [HideInInspector]
        public Vector2 position;

        [HideInInspector]
        public string title;

        [SerializeReference]
        [HideInInspector]
        public List<Node> nodes = new List<Node>();

        internal void OnClone(CloneData cloneData)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i] = cloneData.cloneNodeMap[nodes[i]];
            }
        }
    }
}