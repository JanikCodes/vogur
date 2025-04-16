/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev, Vladimir Deryabin
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using UnityEngine;

namespace RenownedGames.AITree.Nodes
{
    [NodeContent("Set XY", "Tasks/Vector 2/Set XY", IconPath = "Images/Icons/Node/Vector2Icon.png")]
    public class V2SetXYTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private Vector2Key storeResult;

        [SerializeField]
        private FloatKey setX;

        [SerializeField]
        private FloatKey setY;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if(storeResult == null)
            {
                return State.Failure;
            }

            Vector2 result = storeResult.GetValue();
            if(setX != null)
            {
                result.x = setX.GetValue();
            }

            if (setY != null)
            {
                result.y = setY.GetValue();
            }

            storeResult.SetValue(result);
            return State.Success;
        }
    }
}