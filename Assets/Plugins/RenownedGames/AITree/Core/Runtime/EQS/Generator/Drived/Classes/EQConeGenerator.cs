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
using UnityEditor;
using UnityEngine;

namespace RenownedGames.AITree.EQS
{
    [Serializable]
    [SearchContent("Cone", Image = "Images/Icons/EQS/Generators/ConeGenIcon.png")]
    public class EQConeGenerator : EQGenerator
    {
        [SerializeField]
        [Min(0)]
        private float coneDegrees = 90;

        [SerializeField]
        [Min(1)]
        private float angleStep = 10;

        [SerializeField]
        [MinVector2(0, "range.x")]
        private Vector2 range = new Vector2(2, 10);

        [SerializeField]
        [Min(.2f)]
        private float spaceBetween = 1.5f;

        /// <summary>
        /// Returns points created by the generator.
        /// </summary>
        protected override IEnumerable<Vector3> CalculatePoints()
        {
            float count = Mathf.FloorToInt(coneDegrees / angleStep);
            float step = count == 0 ? 1 : coneDegrees / count;

            float halfRad = Mathf.Clamp(coneDegrees * Mathf.Deg2Rad * .5f, 0, Mathf.PI);
            float radStep = step * Mathf.Deg2Rad;

            float minRange = Mathf.Max(Mathf.Min(range.x, range.y), 0);
            float maxRange = Mathf.Max(Mathf.Max(range.x, range.y), 0);

            for (int i = 0; i <= count; i++)
            {
                float angle = -halfRad + i * radStep;

                float x = Mathf.Cos(angle);
                float y = Mathf.Sin(angle);

                float distance = minRange;
                while (distance <= maxRange)
                {
                    yield return new Vector3(y, 0, x) * distance;

                    distance += spaceBetween;
                }
            }
        }

#if UNITY_EDITOR
        #region [Editor]
        /// <summary>
        /// Implement this method to visualize bounds of environment query.
        /// <br><i>Note this is editor-only method.</i></br>
        /// </summary>
        public override void VisualizeBounds()
        {
            Handles.matrix = GetQuerier().localToWorldMatrix;

            float count = Mathf.FloorToInt(coneDegrees / angleStep);
            float step = count == 0 ? 1 : coneDegrees / count;

            float halfRad = Mathf.Clamp(coneDegrees * Mathf.Deg2Rad * .5f, 0, Mathf.PI);
            float radStep = step * Mathf.Deg2Rad;

            float minRange = Mathf.Max(Mathf.Min(range.x, range.y), 0);
            float maxRange = Mathf.Max(Mathf.Max(range.x, range.y), 0);


            Vector3 lastMinPoint = Vector3.zero;
            Vector3 lastMaxPoint = Vector3.zero;
            for (int i = 0; i <= count; i++)
            {
                float angle = -halfRad + i * radStep;

                float x = Mathf.Cos(angle);
                float y = Mathf.Sin(angle);

                Vector3 minPoint = new Vector3(y, 0, x) * minRange;
                Vector3 maxPoint = new Vector3(y, 0, x) * maxRange;

                if (i == 0 || i == count)
                {
                    Handles.DrawLine(minPoint, maxPoint);
                }

                if (i > 0)
                {
                    Handles.DrawLine(lastMinPoint, minPoint);
                    Handles.DrawLine(lastMaxPoint, maxPoint);
                }

                lastMinPoint = minPoint;
                lastMaxPoint = maxPoint;
            }
        }
        #endregion
#endif
    }
}