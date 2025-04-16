/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.AITree;
using RenownedGames.ApexEditor;
using UnityEditor;
using UnityEngine;

namespace RenownedGames.AITreeEditor
{
    [DrawerTarget(typeof(KeyQuery), Subclasses = true)]
    public sealed class KeyQueryDrawer : FieldDrawer
    {
        /// <summary>
        /// Called once when initializing serialized field drawer.
        /// </summary>
        /// <param name="serializedField">Serialized field with DrawerAttribute.</param>
        /// <param name="label">Label of serialized field.</param>
        public override void Initialize(SerializedField serializedField, GUIContent label)
        {
            serializedField.ApplyChildren();
        }

        /// <summary>
        /// Called for rendering and handling drawer GUI.
        /// </summary>
        /// <param name="position">Rectangle on the screen to use for the serialized field drawer GUI.</param>
        /// <param name="serializedField">Reference of serialized field with drawer attribute.</param>
        /// <param name="label">Display label of serialized field.</param>
        public override void OnGUI(Rect position, SerializedField serializedField, GUIContent label)
        {
            if(serializedField.GetChildrenCount() > 0)
            {
                serializedField.DrawChildren(position);
            }
            else
            {
                int controlID = GUIUtility.GetControlID(FocusType.Passive, position);
                position = EditorGUI.PrefixLabel(position, controlID, label);
                EditorGUI.HelpBox(position, "Query for this key type not found.", MessageType.None);
            }
        }

        /// <summary>
        /// Get height which needed to serialized field drawer.
        /// </summary>
        /// <param name="serializedField">Serialized field with DrawerAttribute.</param>
        /// <param name="label">Label of serialized field.</param>
        public override float GetHeight(SerializedField serializedField, GUIContent label)
        {
            if(serializedField.GetChildrenCount() > 0)
            {
                return serializedField.GetChildrenHeight();
            }
            else
            {
                return EditorGUIUtility.singleLineHeight;
            }
        }
    }
}