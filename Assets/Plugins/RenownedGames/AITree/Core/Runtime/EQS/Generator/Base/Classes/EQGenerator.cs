/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RenownedGames.AITree.EQS
{
    [Serializable]
    public abstract class EQGenerator
    {
        private enum PositioningType
        {
            Absolute,
            RelativeQuerier
        }

        [SerializeField]
        private PositioningType positioningType = PositioningType.RelativeQuerier;

        [SerializeField]
        [ShowIf("positioningType", PositioningType.Absolute)]
        private Vector3 absoluteCenter;

        // Stored required components.
        private Transform querier;

        /// <summary>
        /// Returns the points created by the generator taking into account the positioning.
        /// </summary>
        public IEnumerable<Vector3> GeneratePoints()
        {
            if (positioningType == PositioningType.Absolute)
            {
                foreach (Vector3 point in CalculatePoints())
                {
                    yield return absoluteCenter + point;
                }
            }
            else
            {
                foreach (Vector3 point in CalculatePoints())
                {
                    yield return querier.TransformPoint(point);
                }
            }
        }

        /// <summary>
        /// Returns points created by the generator.
        /// </summary>
        protected abstract IEnumerable<Vector3> CalculatePoints();

#if UNITY_EDITOR
        #region [Editor]
        /// <summary>
        /// Implement this method to visualize bounds of environment query.
        /// <br><i>Note this is editor-only method.</i></br>
        /// </summary>
        public virtual void VisualizeBounds() { }
        #endregion
#endif

        #region [Getter / Setter]
        public Transform GetQuerier()
        {
            return querier;
        }

        public void SetQuerier(Transform querier)
        {
            this.querier = querier;
        }
        #endregion
    }
}