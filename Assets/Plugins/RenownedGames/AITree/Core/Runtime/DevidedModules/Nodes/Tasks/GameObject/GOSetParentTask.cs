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
    [NodeContent("Set Parent", "Tasks/Game Object/Set Parent", IconPath = "Images/Icons/Node/GameObjectIcon.png")]
    public class GOSetParentTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [NonLocal]
        private TransformKey transform;

        [SerializeField]
        [NonLocal]
        [Label("Parent")]
        private TransformKey setParent;

        [Title("Node")]
        [SerializeField]
        private bool resetLocalPosition;

        [SerializeField]
        private bool resetLocalRotation;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (transform == null || setParent == null)
            {
                return State.Failure;
            }

            Transform _transform = transform.GetValue();
            Transform _parent = setParent.GetValue();
            if (_transform == null || _parent == null)
            {
                return State.Failure;
            }

            _transform.SetParent(_parent);

            if (resetLocalPosition)
            {
                _transform.localPosition = Vector3.zero;
            }

            if (resetLocalRotation)
            {
                _transform.localRotation = Quaternion.identity;
            }

            return State.Success;
        }
    } }