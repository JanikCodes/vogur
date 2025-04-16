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
    [NodeContent("Get XY", "Tasks/Vector 2/Get XY", IconPath = "Images/Icons/Node/Vector2Icon.png")]
    public class V2GetXYTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [NonLocal]
        private Vector2Key vector2;

        [SerializeField]
        [NonLocal]
        private FloatKey storeX;

        [SerializeField]
        [NonLocal]
        private FloatKey storeY;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (vector2 == null || storeX == null || storeY == null)
            {
                return State.Failure;
            }

            Vector2 result = vector2.GetValue();
            storeX.SetValue(result.x);
            storeY.SetValue(result.y);

            return State.Success;
        }
    }
}
