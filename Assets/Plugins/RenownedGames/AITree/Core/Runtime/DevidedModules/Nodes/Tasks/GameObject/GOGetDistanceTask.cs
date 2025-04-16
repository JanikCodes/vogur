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
    [NodeContent("Get Distance", "Tasks/Game Object/Get Distance", IconPath = "Images/Icons/Node/GameObjectIcon.png")]
    public class GOGetDistanceTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [NonLocal]
        private TransformKey transform;

        [SerializeField]
        [HideChildren]
        [NonLocal]
        private TransformKey target;

        [SerializeField]
        [NonLocal]
        private FloatKey storeResult;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (transform == null || target == null || storeResult == null)
            {
                return State.Failure;
            }

            Transform _transform = transform.GetValue();
            Transform _target = target.GetValue();
            if (_transform == null || _target == null)
            {
                return State.Failure;
            }

            storeResult.SetValue(Vector3.Distance(_transform.position, _target.position));
            return State.Success;
        }
    }
}