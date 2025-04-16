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
    [NodeContent("Blackboard Condition", "Blackboard Condition", IconPath = "Images/Icons/Node/BlackboardConditionIcon.png")]
    public class BlackboardConditionDecorator : ObserverDecorator
    {
        [Title("Decorator")]
        [SerializeField]
        [NonLocal]
        [OnValueChanged("OnKeyChanged")]
        private Key key;

        [SerializeReference]
        [HideIf("IsKeyEmpty")]
        private KeyQuery keyQuery;

        /// <summary>
        /// Called on behaviour tree is awake.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();

            if (key != null)
            {
                key.ValueChanged += OnValueChange;
            }
        }

        /// <summary>
        /// Called every tick regardless of the node execution.
        /// </summary>
        protected override void OnFlowUpdate() { }

        #region [ObserverDecorator Implementation]
        /// <summary>
        /// An event that should be called when the observed value changes.
        /// </summary>
        public override event Action OnValueChange;

        /// <summary>
        /// Calculates the observed value of the result.
        /// </summary>
        public override bool CalculateResult()
        {
            if(keyQuery == null || key == null)
            {
                return false;
            }

            return keyQuery.Result(key);
        }
        #endregion

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
                    description += $"Blackboard: {responseDescription}";
                }
            }

            return description;
        }
        #endregion

        #region [Getter / Setter]
        public Key GetKey()
        {
            return key;
        }

        public KeyQuery GetKeyQuery()
        {
            return keyQuery;
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
                            query.managedReferenceValue = Activator.CreateInstance(queryType);
                            query.serializedObject.ApplyModifiedProperties();
                            return;
                        }
                    }
                }
            }

            SerializedProperty keyQuery = serializedObject.FindProperty("keyQuery");
            keyQuery.managedReferenceValue = null;
            keyQuery.serializedObject.ApplyModifiedProperties();
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
