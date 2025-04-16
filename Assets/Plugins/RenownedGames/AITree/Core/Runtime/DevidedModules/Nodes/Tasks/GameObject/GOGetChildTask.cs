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
    [NodeContent("Get Child", "Tasks/Game Object/Get Child", IconPath = "Images/Icons/Node/GameObjectIcon.png")]
    public class GOGetChildTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [NonLocal]
        private TransformKey transform;

        [SerializeField]
        [HideSelfKey]
        [NonLocal]
        private TransformKey storeResult;

        [Title("Node")]
        [SerializeField]
        private IntKey childIndex;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (transform == null || childIndex == null || storeResult == null)
            {
                return State.Failure;
            }

            Transform _transform = transform.GetValue();
            if (_transform == null)
            {
                return State.Failure;
            }

            int index = childIndex.GetValue();
            if (index < 0 || _transform.childCount <= index)
            {
                return State.Failure;
            }

            storeResult.SetValue(_transform.GetChild(index));
            return State.Success;
        }
    }
}