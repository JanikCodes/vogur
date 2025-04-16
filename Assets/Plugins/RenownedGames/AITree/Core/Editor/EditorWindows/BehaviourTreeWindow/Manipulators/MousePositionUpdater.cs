/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace RenownedGames.AITreeEditor
{
    public class MousePositionUpdater : MouseManipulator
    {
        private BehaviourTreeGraph graph;
        private Action<Vector2> action;

        public MousePositionUpdater(BehaviourTreeGraph graph, Action<Vector2> action)
        {
            this.graph = graph;
            this.action = action;
        }

        private void OnMouseMove(MouseMoveEvent evt)
        {
            if (target == null || !CanStopManipulation(evt))
            {
                return;
            }

            action?.Invoke(graph.ChangeCoordinatesToView(evt.localMousePosition));
        }

        #region [IManipulator Implementation]
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
        }
        #endregion
    }
}