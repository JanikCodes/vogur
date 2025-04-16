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
using System.Collections.Generic;
using UnityEngine;

namespace RenownedGames.AITree.Nodes
{
    [NodeContent("Find", "Tasks/Game Object/Find", IconPath = "Images/Icons/Node/GameObjectIcon.png")]
    public class GOFindTask : TaskNode
    {
        private enum ParameterType
        {
            Name = 1,
            Tag = 2,
        }

        [Title("Blackboard")]
        [SerializeField]
        [HideSelfKey]
        [NonLocal]
        private TransformKey storeResult;

        [Title("Node")]
        [SerializeField]
        [Enum(UseAsFlags = true, HideValues = "EnumHideValues")]
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
            if (storeResult == null)
            {
                return State.Failure;
            }

            if (parameterType == ParameterType.Name)
            {
                GameObject go = GameObject.Find(withName);
                if (go != null)
                {
                    storeResult.SetValue(go.transform);
                    return State.Success;
                }
            }
            else if (parameterType == ParameterType.Tag)
            {
                GameObject go = GameObject.FindWithTag(withTag);
                if (go != null)
                {
                    storeResult.SetValue(go.transform);
                    return State.Success;
                }
            }
            else if ((parameterType & ~ParameterType.Name) == ParameterType.Tag)
            {
                GameObject[] gos = GameObject.FindGameObjectsWithTag(withTag);
                for (int i = 0; i < gos.Length; i++)
                {
                    GameObject go = gos[i];
                    if (go.name == withName)
                    {
                        storeResult.SetValue(go.transform);
                        return State.Success;
                    }
                }
            }

            return State.Failure;
        }

        #region [Attribyte Helper]
        private bool ParameterTypeHas(ParameterType parameterType)
        {
            return (this.parameterType & parameterType) != 0;
        }

        private IEnumerable<ParameterType> EnumHideValues()
        {
            yield return (ParameterType)0;
            yield return (ParameterType)3;
        }
        #endregion
    }
}