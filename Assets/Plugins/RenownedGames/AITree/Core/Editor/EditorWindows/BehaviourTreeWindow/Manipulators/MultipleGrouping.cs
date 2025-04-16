/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace RenownedGames.AITreeEditor
{
    public class MultipleGrouping : Manipulator
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
        /// Create group of selected elements.
        /// </summary>
        private void CreateGroup()
        {
            BehaviourTreeGraph graph = target as BehaviourTreeGraph;
            if (graph == null)
            {
                return;
            }

            List<WrapView> selectedViews = graph.selection.OfType<WrapView>().ToList();
            if (selectedViews.Count == 0)
            {
                return;
            }

            HashSet<GroupView> affectedGroups = new HashSet<GroupView>();
            foreach (WrapView wrapView in selectedViews)
            {
                GroupView groupView = wrapView.GetGroup();
                if (groupView != null)
                {
                    affectedGroups.Add(groupView);
                }
            }

            if (affectedGroups.Count == 1)
            {
                GroupView groupView = affectedGroups.First();
                List<GraphElement> groupElements = groupView.containedElements.ToList();

                if (groupElements.Count == selectedViews.Count &&
                    selectedViews.All(s => groupElements.Contains(s)))
                {
                    return;
                }
            }
            else
            {
                GroupView groupView = graph.CreateGroup(Vector2.zero);
                groupView.AddElements(selectedViews);

                if (affectedGroups.Count > 0)
                {
                    List<GroupView> groupsToDelete = new List<GroupView>();
                    foreach (GroupView affectedGroup in affectedGroups)
                    {
                        if (affectedGroup.containedElements.Count() == 0)
                        {
                            groupsToDelete.Add(affectedGroup);
                        }
                    }
                    graph.DeleteElements(groupsToDelete);
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
                && evt.type == EventType.KeyDown 
                && evt.keyCode == KeyCode.G
                && evt.control)
            {
                CreateGroup();
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

            if (evt.keyCode == KeyCode.G && evt.modifiers == EventModifiers.Control)
            {
                CreateGroup();
            }
        }
    }
}