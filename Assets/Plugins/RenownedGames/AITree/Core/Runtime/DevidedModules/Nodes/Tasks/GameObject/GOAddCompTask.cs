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
    [NodeContent("Add Component", "Tasks/Game Object/Add Component", IconPath = "Images/Icons/Node/GameObjectIcon.png")]
    public class GOAddCompTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [NonLocal]
        private TransformKey transform;

        [Title("Node")]
        [SerializeField]
        private StringKey componentType;

        //[SerializeField]
        //[NonLocal]
        //private ObjectKey storeComponent;

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

            _transform.gameObject.AddComponent(type);
            return State.Success;
        }
    }
}