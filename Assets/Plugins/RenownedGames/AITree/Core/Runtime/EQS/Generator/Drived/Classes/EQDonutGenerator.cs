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
    [SearchContent("Donut", Image = "Images/Icons/EQS/Generators/DonutGenIcon.png")]
    public class EQDonutGenerator : EQGenerator
    {
        [SerializeField]
        [MinVector2(0, "range.x")]
        private Vector2 range = new Vector2(3, 10);

        [SerializeField]
        [Min(2)]
        private int ringCount = 3;

        [SerializeField]
        [Min(1)]
        private int pointsPerRing = 8;

        [SerializeField]
        private bool rotateAngle;

        /// <summary>
        /// Returns points created by the generator.
        /// </summary>
        protected override IEnumerable<Vector3> CalculatePoints()
        {
            float step = (range.y - range.x) / (ringCount - 1);
            float x = 0;
            float y = 0;

            for (int i = 0; i < pointsPerRing; i++)
            {
                float angle = (float)i / pointsPerRing * Mathf. PI * 2f;

                if (!rotateAngle)
                {
                    x = Mathf.Cos(angle);
                    y = Mathf.Sin(angle);
                }

                for (float distance = range.x; distance <= range.y; distance += step)
                {
                    if (rotateAngle)
                    {
                        angle += step;
                        x = Mathf.Cos(angle);
                        y = Mathf.Sin(angle);
                    }

                    yield return new Vector3(y, 0, x) * distance;

                    if (step == 0)
                    {
                        break;
                    }
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

            Vector3 lastMinPoint = Vector3.zero;
            Vector3 lastMaxPoint = Vector3.zero;

            int count = 32;

            for (int i = 0; i <= count; i++)
            {
                float angle = (float)i / count * Mathf.PI * 2f;

                float x = Mathf.Cos(angle);
                float y = Mathf.Sin(angle);

                Vector3 minPoint = new Vector3(x, 0, y) * range.x;
                Vector3 maxPoint = new Vector3(x, 0, y) * range.y;

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