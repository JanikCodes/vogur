/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using UnityEngine;

namespace RenownedGames.AITree.Nodes
{
    [NodeContent("Get Tag", "Tasks/Game Object/Get Tag", IconPath = "Images/Icons/Node/GameObjectIcon.png")]
    public class GOGetTagTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [NonLocal]
        private TransformKey transform;

        [SerializeField]
        [NonLocal]
        private StringKey storeResult;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (transform == null || storeResult == null)
            {
                return State.Failure;
            }

            Transform _transform = transform.GetValue();
            if (_transform == null)
            {
                return State.Failure;
            }

            storeResult.SetValue(_transform.gameObject.tag);
            return State.Success;
        }
    }
}