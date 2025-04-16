/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RenownedGames.AITree.Nodes
{
    [NodeContent("Random Selector", "Composites/Random Selector", IconPath = "Images/Icons/Node/ParallelIcon.png")]
    public class RandomSelectorNode : SelectorNode
    {
        private List<Node> randomizedChildren;

        /// <summary>
        /// Called when behaviour tree enter in node.
        /// </summary>
        protected override void OnEntry()
        {
            base.OnEntry();
            randomizedChildren = GetChildren().OrderBy(x => Random.value).ToList();
        }

        /// <summary>
        /// Returns the child node by index.
        /// </summary>
        /// <param name="index">Index of the child node.</param>
        /// <returns>Child node.</returns>
        public override Node GetChild(int index)
        {
            if (index >= 0 && index < randomizedChildren.Count)
            {
                return randomizedChildren[index];
            }
            return null;
        }
    }
}