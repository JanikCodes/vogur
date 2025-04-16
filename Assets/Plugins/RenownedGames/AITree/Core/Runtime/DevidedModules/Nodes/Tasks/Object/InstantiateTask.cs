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
    [Obsolete("Use Create Object task instead")]
    [NodeContent("Instantiate", "Tasks/Object/Instantiate", IconPath = "Images/Icons/Node/InstantiateIcon.png", Hide = true)]
    public class InstantiateTask : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        private GameObject objectToInstantiate;

        [SerializeField]
        private Vector3Key position;

        [SerializeField]
        private QuaternionKey rotation;

        [SerializeField]
        [Label("Parent")]
        private TransformKey objectParent;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns></returns>
        protected override State OnUpdate()
        {
            if (objectToInstantiate == null)
            {
                return State.Failure;
            }

            Vector3 _position = Vector3.zero;
            if (position != null)
            {
                _position = position.GetValue();
            }

            Quaternion _rotation = Quaternion.identity;
            if (rotation != null)
            {
                _rotation = rotation.GetValue();
            }

            Transform _parent = null;
            if (objectParent != null)
            {
                _parent = objectParent.GetValue();
            }

            GameObject.Instantiate(objectToInstantiate, _position, _rotation, _parent);
            return State.Success;
        }

        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            string description = $"Instantiate: ";

            if (objectToInstantiate != null)
            {
                description += objectToInstantiate.name;
            }
            else
            {
                description += "None";
            }

            if (position != null)
            {
                description += $"\nPosition: {position.ToString()}";
            }

            if (rotation != null)
            {
                description += $"\nRotation: {rotation.ToString()}";
            }

            if (objectParent != null)
            {
                description += $"\nParent: {objectParent.ToString()}";
            }

            return description;
        }
    }
}