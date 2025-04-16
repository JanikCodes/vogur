/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace RenownedGames.AITree.Nodes
{
    [NodeContent("Select Random Vector2", "Tasks/Vector 2/Select Random Vector2", IconPath = "Images/Icons/Node/Vector2Icon.png")]
    public class V2SelectRandomTask : TaskNode
    {
        [System.Serializable]
        private struct VectorElement
        {
            public Vector2Key vector;

            [Slider(0f, 1f)]
            public float weight;
        }

        [Title("Blackboard")]
        [SerializeField]
        [NonLocal]
        private Vector2Key storeResult;

        [Title("Node")]
        [SerializeField]
        [Array(OnAdd = "OnAdd", OnRemove = "OnRemove")]
        private VectorElement[] vectors;

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

            Vector2 vector = GetVectorRandomly();
            storeResult.SetValue(vector);

            return State.Success;
        }

        private Vector2 GetVectorRandomly()
        {
            float totalWeight = 0;
            for (int i = 0; i < vectors.Length; i++)
            {
                VectorElement element = vectors[i];
                totalWeight += element.weight;
            }

            float value = Random.Range(0f, totalWeight);
            for (int i = 0; i < vectors.Length; i++)
            {
                VectorElement element = vectors[i];
                value -= element.weight;
                if (value <= 0)
                {
                    if (element.vector != null)
                    {
                        return element.vector.GetValue();
                    }

                    return Vector2.zero;
                }
            }

            return Vector2.zero;
        }

        #region [Array Attrubute]
#if UNITY_EDITOR
        private void OnAdd(SerializedProperty property, int index)
        {
            SerializedProperty vectorElementProperty = property.GetArrayElementAtIndex(index);
            SerializedProperty serializedField = vectorElementProperty.FindPropertyRelative("vector");

            VectorElement element = vectors[index];
            if (element.vector != null && element.vector.IsLocal())
            {
                int group = Undo.GetCurrentGroup();
                Undo.IncrementCurrentGroup();

                Key originalKey = element.vector;
                System.Type keyType = originalKey.GetType();


                Object target = serializedField.serializedObject.targetObject;

                Key instanceKey = ScriptableObject.CreateInstance(keyType) as Key;
                EditorUtility.CopySerialized(originalKey, instanceKey);

                if (!EditorApplication.isPlaying)
                {
                    if (target is ScriptableObject scriptableObject)
                    {
                        AssetDatabase.AddObjectToAsset(instanceKey, scriptableObject);

                        EditorUtility.SetDirty(target);
                        AssetDatabase.SaveAssetIfDirty(target);
                    }
                }

                Undo.RegisterCreatedObjectUndo(instanceKey, "[Key] Create local key");

                Undo.RecordObject(target, "[Key] Add key to asset");

                serializedField.serializedObject.Update();
                serializedField.objectReferenceValue = instanceKey;
                serializedField.serializedObject.ApplyModifiedProperties();

                Undo.SetCurrentGroupName("[Key] Create local key");
                Undo.CollapseUndoOperations(group);
            }
        }

        private void OnRemove(SerializedProperty property, int index)
        {
            SerializedProperty vectorElementProperty = property.GetArrayElementAtIndex(index);
            SerializedProperty serializedField = vectorElementProperty.FindPropertyRelative("vector");

            VectorElement element = vectors[index];
            if (element.vector != null && element.vector.IsLocal())
            {
                int group = Undo.GetCurrentGroup();

                Object target = serializedField.serializedObject.targetObject;
                Object keyToRemove = serializedField.objectReferenceValue;

                Undo.RecordObject(target, "[Key] Delete key from node");

                serializedField.serializedObject.Update();
                serializedField.objectReferenceValue = null;
                serializedField.serializedObject.ApplyModifiedProperties();
                if (target is Node node)
                {
                    new SerializedObject(node.GetBehaviourTree()).ApplyModifiedProperties();
                }

                if (target is ScriptableObject)
                {
                    Undo.DestroyObjectImmediate(keyToRemove);
                    if (!EditorApplication.isPlaying)
                    {
                        EditorUtility.SetDirty(target);
                        AssetDatabase.SaveAssetIfDirty(target);
                    }
                }

                Undo.CollapseUndoOperations(group);
            }
        }
#endif
        #endregion
    }
}