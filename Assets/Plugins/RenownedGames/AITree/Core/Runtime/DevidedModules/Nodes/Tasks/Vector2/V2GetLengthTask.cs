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
    [NodeContent("Get Vector2 Length", "Tasks/Vector 2/Get Length", IconPath = "Images/Icons/Node/Vector2Icon.png")]
    public class V2GetLengthTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private Vector2Key vector2;

        [SerializeField]
        [NonLocal]
        private FloatKey storeLength;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if(vector2 == null || storeLength == null)
            {
                return State.Failure;
            }

            storeLength.SetValue(vector2.GetValue().magnitude);
            return State.Success;
        }
    }
}