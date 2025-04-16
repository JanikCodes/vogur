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

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RenownedGames.AITree.Nodes
{
    [HideMonoScript]
    [NodeContent("Conditional Loop", "Conditional Loop", IconPath = "Images/Icons/Node/LoopIcon.png")]
    public class ConditionalLoopDecorator : DecoratorNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [NonLocal]
        [OnValueChanged("OnKeyChanged")]
        private Key key;

        [SerializeReference]
        [InlineEditor]
        [HideChildren("key")]
        [HideIf("IsKeyEmpty")]
        private KeyQuery keyQuery;

        [SerializeField]
        private State finishState = State.Failure;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if(keyQuery == null || key == null)
            {
                return State.Failure;
            }

            if (keyQuery.Result(key))
            {
                UpdateChild();
                return State.Running;
            }

            ResetChild(finishState);
            return finishState;
        }

        /// <summary>
        /// Called every tick regardless of the node execution.
        /// </summary>
        protected override void OnFlowUpdate() { }

        #region [IEntityDescription Implementation]
        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            string description = base.GetDescription();

            if (keyQuery != null)
            {
                string responseDescription = keyQuery.GetDescription(key);
                if (!string.IsNullOrEmpty(responseDescription))
                {
                    if (!string.IsNullOrEmpty(description)) description += "\n";
                    description += $"Conditional Loop: {responseDescription}";
                }
            }

            return description;
        }
        #endregion

        #region [Editor]
#if UNITY_EDITOR
        private bool IsKeyEmpty()
        {
            return key == null;
        }

        private void OnKeyChanged(SerializedProperty property)
        {
            SerializedObject serializedObject = property.serializedObject;

            Key key = property.objectReferenceValue as Key;
            if (key != null)
            {
                TypeCache.TypeCollection queryTypes = TypeCache.GetTypesDerivedFrom<KeyQuery>();
                for (int i = 0; i < queryTypes.Count; i++)
                {
                    Type queryType = queryTypes[i];
                    if (!queryType.IsAbstract && !queryType.IsGenericType)
                    {
                        Type queryKeyType = FindQueryKeyType(queryType);
                        if (queryKeyType != null &&
                            (queryKeyType == key.GetType() || key.GetType().IsSubclassOf(queryKeyType)))
                        {
                            SerializedProperty query = serializedObject.FindProperty("keyQuery");
                            query.managedReferenceValue = Activator.CreateInstance(queryType, new object[1] { key });
                            query.serializedObject.ApplyModifiedProperties();
                            break;
                        }
                    }
                }
            }
            else
            {
                SerializedProperty query = serializedObject.FindProperty("keyQuery");
                query.managedReferenceValue = null;
                query.serializedObject.ApplyModifiedProperties();
            }
        }

        private Type FindQueryKeyType(Type type)
        {
            if (type.IsGenericType)
            {
                return type.GetGenericArguments()[0];
            }
            else if (type.BaseType != null)
            {
                return FindQueryKeyType(type.BaseType);
            }
            return null;
        }
#endif
#endregion
    }
}