/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace RenownedGames.AITreeEditor
{
    public class AllSelector : Manipulator
    {
        #region [IManipulator Implementation]
        /// <summary>
        /// Called to register event callbacks on the target element.
        /// </summary>
        protected override void RegisterCallbacksOnTarget()
        {
            AITreeSettings settings = AITreeSettings.instance;
            switch (settings.GetGraphHotKeyAPI())
            {
                default:
                case AITreeSettings.HotKeyAPI.KeyDownListener:
                    EditorApplication.update -= OnKeyDownListener;
                    EditorApplication.update += OnKeyDownListener;
                    break;
                case AITreeSettings.HotKeyAPI.KeyDownEvent:
                    target.RegisterCallback<KeyDownEvent>(OnKeyDownEvent);
                    break;
            }
        }

        /// <summary>
        /// Called to unregister event callbacks from the target element.
        /// </summary>
        protected override void UnregisterCallbacksFromTarget()
        {
            AITreeSettings settings = AITreeSettings.instance;
            switch (settings.GetGraphHotKeyAPI())
            {
                default:
                case AITreeSettings.HotKeyAPI.KeyDownListener:
                    EditorApplication.update -= OnKeyDownListener;
                    break;
                case AITreeSettings.HotKeyAPI.KeyDownEvent:
                    target.UnregisterCallback<KeyDownEvent>(OnKeyDownEvent);
                    break;
            }
        }
        #endregion

        /// <summary>
        /// Select all graph elements.
        /// </summary>
        private void SelectAll()
        {
            BehaviourTreeGraph graph = target as BehaviourTreeGraph;
            if (graph == null)
            {
                return;
            }

            graph.ClearSelection();
            foreach (GraphElement element in graph.graphElements)
            {
                if (element is ISelectable selectable && element is not Edge)
                {
                    graph.AddToSelection(selectable);
                }
            }
        }

        /// <summary>
        /// Internal key down callback listener.
        /// </summary>
        private void OnKeyDownListener()
        {
            Event evt = BTEventTracker.Current;
            if (evt != null
                && evt.type == EventType.Used
                && evt.keyCode == KeyCode.A
                && evt.control)
            {
                SelectAll();
            }
        }

        /// <summary>
        /// Built-in Unity key down callback.
        /// </summary>
        /// <param name="evt">Event reference.</param>
        private void OnKeyDownEvent(KeyDownEvent evt)
        {
            if (target == null)
            {
                return;
            }

            if (evt.modifiers == EventModifiers.Control && evt.keyCode == KeyCode.A)
            {
                SelectAll();
            }
        }

    }
}