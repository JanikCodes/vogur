/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using System;
using UnityEngine;

namespace RenownedGames.AITree.Nodes
{
    [NodeContent("Destroy Component", "Tasks/Game Object/Destroy Component", IconPath = "Images/Icons/Node/GameObjectIcon.png")]
    public class GODestroyCompTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [NonLocal]
        private TransformKey transform;

        [Title("Node")]
        [SerializeField]
        private StringKey componentType;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (transform == null || componentType == null)
            {
                return State.Failure;
            }

            Transform _transform = transform.GetValue();
            if (_transform == null)
            {
                return State.Failure;
            }

            Type type = Type.GetType(componentType.GetValue());
            if (type == null)
            {
                return State.Failure;
            }
            
            if (_transform.TryGetComponent(type, out Component component))
            {
                Destroy(component);
                return State.Success;
            }

            return State.Failure;
        }
    }
}