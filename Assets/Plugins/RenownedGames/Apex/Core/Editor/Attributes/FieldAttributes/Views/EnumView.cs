/* ================================================================
   ----------------------------------------------------------------
   Project   :   Apex
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using Vexe.Runtime.Extensions;

namespace RenownedGames.ApexEditor
{
    [ViewTarget(typeof(EnumAttribute))]
    public class EnumView : BaseEnumView
    {
        /// <summary>
        /// Called for drawing element view GUI.
        /// </summary>
        /// <param name="position">Position of the serialized element.</param>
        /// <param name="field">Serialized element with ViewAttribute.</param>
        /// <param name="label">Label of serialized element.</param>
        public override void OnGUI(Rect position, SerializedField field, GUIContent label)
        {
            position = EditorGUI.PrefixLabel(position, label);

            EditorGUI.BeginDisabledGroup(IsEmpty());
            if (GUI.Button(position, GetValueName(position.width), EditorStyles.popup))
            {
                GenericMenu genericMenu = new GenericMenu();
                List<MenuItem> menuItems = GetMenuItems();
                for (int i = 0; i < menuItems.Count; i++)
                {
                    MenuItem menuItem = menuItems[i];
                    genericMenu.AddItem(menuItem.content, menuItem.isOn, SetEnumFunction, menuItem.bit);
                }
                genericMenu.DropDown(position);
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}