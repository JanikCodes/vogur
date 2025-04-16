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
    [NodeContent("Destroy Object", "Tasks/Game Object/Destroy Object", IconPath = "Images/Icons/Node/GameObjectIcon.png")]
    public class GODestroyObjectTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [HideSelfKey]
        [NonLocal]
        private TransformKey transform;

        [Title("Node")]
        [SerializeField]
        private FloatKey delay;

        [SerializeField]
        private BoolKey detachChildren;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (transform == null)
            {
                return State.Failure;
            }

            Transform _transform = transform.GetValue();
            if (_transform == null)
            {
                return State.Failure;
            }

            float _delay = (delay != null) ? delay.GetValue() : 0;
            bool _detachChildren = detachChildren != null && detachChildren.GetValue();

            if (_detachChildren)
            {
                foreach (Transform child in _transform)
                {
                    child.SetParent(null);
                }
            }

            if (_delay > 0)
            {
                Destroy(_transform.gameObject, _delay);
            }
            else
            {
                Destroy(_transform.gameObject);
            }

            return State.Success;
        }

        #region [IEntityDescription Implementation]
        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            string description = $"Destroy Object: ";

            if (transform != null)
            {
                description += transform.ToString();
            }
            else
            {
                description += "None";
            }

            return description;
        }
        #endregion
    }
}
