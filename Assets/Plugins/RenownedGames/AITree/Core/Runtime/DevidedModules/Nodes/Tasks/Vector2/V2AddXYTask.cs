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
    [NodeContent("Add XY", "Tasks/Vector 2/Add XY", IconPath = "Images/Icons/Node/Vector2Icon.png")]
    public class V2AddXYTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private Vector2Key vector2;

        [SerializeField]
        private FloatKey addX;

        [SerializeField]
        private FloatKey addY;

        [SerializeField]
        [NonLocal]
        private Vector2Key storedResult;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (vector2 == null || storedResult == null)
            {
                return State.Failure;
            }

            Vector2 result = vector2.GetValue();
            if(addX != null)
            {
                result.x += addX.GetValue();
            }

            if (addY != null)
            {
                result.y += addY.GetValue();
            }

            storedResult.SetValue(result);
            return State.Success;
        }
    }
}