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
    [NodeContent("Find Child", "Tasks/Game Object/Find Child", IconPath = "Images/Icons/Node/GameObjectIcon.png")]
    public class GOFindChildTask : TaskNode
    {
        private enum ParameterType
        {
            None = 0,
            Name = 1,
            Tag = 2,
            NameAndTag = 3
        }

        [Title("Blackboard")]
        [SerializeField]
        [HideSelfKey]
        [NonLocal]
        private TransformKey storeResult;

        [Title("Node")]
        [SerializeField]
        [NonLocal]
        private TransformKey relativeObject;

        [SerializeField]
        [Enum(UseAsFlags = true)]
        private ParameterType parameterType = ParameterType.Name;

        [SerializeField]
        [ShowIf("ParameterTypeHas", ParameterType.Name)]
        private string withName;

        [SerializeField]
        [ShowIf("ParameterTypeHas", ParameterType.Tag)]
        [TagSelecter]
        private string withTag;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (relativeObject == null || storeResult == null)
            {
                return State.Failure;
            }

            Transform _transform = relativeObject.GetValue();
            if (_transform == null)
            {
                return State.Failure;
            }

            foreach (Transform child in _transform.GetComponentsInChildren<Transform>())
            {
                if (ValidateObject(child))
                {
                    storeResult.SetValue(child);
                    return State.Success;
                }
            }

            return State.Failure;
        }

        private bool ValidateObject(Transform transform)
        {
            GameObject gameObject = transform.gameObject;

            if (ParameterTypeHas(ParameterType.Name))
            {
                if (gameObject.name != withName)
                {
                    return false;
                }
            }

            if (ParameterTypeHas(ParameterType.Tag))
            {
                if (gameObject.tag != withTag)
                {
                    return false;
                }
            }

            return true;
        }

        private bool ParameterTypeHas(ParameterType parameterType)
        {
            return (this.parameterType & parameterType) != 0;
        }
    }
}