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
    [NodeContent("Create Object", "Tasks/Game Object/Create Object", IconPath = "Images/Icons/Node/GameObjectIcon.png")]
    public class GOCreateObjectTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [HideSelfKey]
        [NonLocal]
        private TransformKey storeObject;

        [Title("Node")]
        [SerializeField]
        private GameObject gameObject;

        [SerializeField]
        private Vector3Key position;

        [SerializeField]
        private QuaternionKey rotation;

        [SerializeField]
        [Label("Parent")]
        private TransformKey refParent;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (gameObject == null)
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
            if (refParent != null)
            {
                _parent = refParent.GetValue();
            }

            GameObject spawned = Instantiate(gameObject, _position, _rotation, _parent);

            if (storeObject != null)
            {
                storeObject.SetValue(spawned.transform);
            }

            return State.Success;
        }

        #region [IEntityDescription Implementation]
        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            string description = $"Create Object: ";

            if (gameObject != null)
            {
                description += gameObject.name;
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

            if (refParent != null)
            {
                description += $"\nParent: {refParent.ToString()}";
            }

            return description;
        }
        #endregion
    }
}