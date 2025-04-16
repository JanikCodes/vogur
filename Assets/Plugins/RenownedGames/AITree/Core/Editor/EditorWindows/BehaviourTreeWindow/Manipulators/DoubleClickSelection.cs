/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace RenownedGames.AITreeEditor
{
    public class DoubleClickSelection : MouseManipulator
    {
        private const float doubleClickTime = .3f;

        // Stored required properties.
        private Action<VisualElement> action;
        private VisualElement lastSelectedElement;
        private float lastClickTime;

        public DoubleClickSelection(Action<VisualElement> action)
        {
            this.action = action;
        }

        private void OnMouseDown(MouseDownEvent evt)
        {
            if (target == null || !CanStopManipulation(evt)) return;

            VisualElement element = evt.target as VisualElement;

            float deltaTime = (float)EditorApplication.timeSinceStartup - lastClickTime;
            if (deltaTime <= doubleClickTime && element != null && lastSelectedElement == element)
            {
                action?.Invoke(lastSelectedElement);
            }

            lastSelectedElement = element;
            lastClickTime = (float)EditorApplication.timeSinceStartup;
        }

        #region [IManipulator Implementation]
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        }
        #endregion
    }
}