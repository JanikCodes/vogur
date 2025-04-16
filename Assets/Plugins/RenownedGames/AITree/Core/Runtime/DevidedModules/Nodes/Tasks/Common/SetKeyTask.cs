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
    [NodeContent("Set Key", "Tasks/Common/Set Key", IconPath = "Images/Icons/Node/SetKeyIcon.png")]
    public class SetKeyTask : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        [NonLocal]
        [OnValueChanged("OnKeyChanged")]
        private Key blackboardKey;

        [SerializeReference]
        [InlineEditor]
        [HideChildren("key")]
        [HideIf("IsKeyEmpty")]
        private KeyReceiver keyReceiver;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (keyReceiver == null || blackboardKey == null)
            {
                return State.Failure;
            }

            keyReceiver.Apply(blackboardKey);
            return State.Success;
        }

        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            string description = base.GetDescription();

            if (keyReceiver != null)
            {
                string responseDescription = keyReceiver.GetDescription(blackboardKey);
                if (!string.IsNullOrEmpty(responseDescription))
                {
                    description += $"Set Key: {responseDescription}";
                }
            }

            return description;
        }

        #region [Editor]
#if UNITY_EDITOR
        private bool IsKeyEmpty()
        {
            return blackboardKey == null;
        }

        private void OnKeyChanged(SerializedProperty property)
        {
            SerializedObject serializedObject = property.serializedObject;

            Key key = property.objectReferenceValue as Key;
            if (key != null)
            {
                TypeCache.TypeCollection receiverTypes = TypeCache.GetTypesDerivedFrom<KeyReceiver>();
                for (int i = 0; i < receiverTypes.Count; i++)
                {
                    Type receiverType = receiverTypes[i];
                    if (!receiverType.IsAbstract && !receiverType.IsGenericType)
                    {
                        Type receiverKeyType = FindReceiverKeyType(receiverType);
                        if (receiverKeyType != null &&
                            (receiverKeyType == key.GetType() || key.GetType().IsSubclassOf(receiverKeyType)))
                        {
                            SerializedProperty rewceiver = serializedObject.FindProperty("keyReceiver");
                            rewceiver.managedReferenceValue = Activator.CreateInstance(receiverType);
                            rewceiver.serializedObject.ApplyModifiedProperties();
                            break;
                        }
                    }
                }
            }
            else
            {
                SerializedProperty receiver = serializedObject.FindProperty("keyReceiver");
                receiver.managedReferenceValue = null;
                receiver.serializedObject.ApplyModifiedProperties();
            }
        }

        private Type FindReceiverKeyType(Type type)
        {
            if (type.IsGenericType)
            {
                return type.GetGenericArguments()[0];
            }
            else if (type.BaseType != null)
            {
                return FindReceiverKeyType(type.BaseType);
            }
            return null;
        }
#endif
        #endregion
    }
}