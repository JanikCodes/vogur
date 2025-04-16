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

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RenownedGames.AITree.EQS
{
    [Serializable]
    [SearchContent("Circle", Image = "Images/Icons/EQS/Generators/CircleGenIcon.png")]
    public class EQCircleGenerator : EQGenerator
    {
        [SerializeField]
        [MinValue(1)]
        private float radius = 10f;

        [SerializeField]
        [MinValue(.2f)]
        private float spaceBetween = 1.5f;

        [SerializeField]
        private bool fillInside = true;

        /// <summary>
        /// Returns points created by the generator.
        /// </summary>
        protected override IEnumerable<Vector3> CalculatePoints()
        {
            float pi2 = Mathf.PI * 2f;
            float radiusStep = radius / Mathf.Floor(radius / spaceBetween);

            float currentRadius = radius;
            while (currentRadius > 0)
            {
                float l = pi2 * currentRadius;
                float a = pi2 / (int)(l / spaceBetween);
                int n = (int)Mathf.Round(pi2 / a);

                for (float i = 0; i < n; i++)
                {
                    float angle = i * a;
                    float x = Mathf.Sin(angle) * currentRadius;
                    float z = Mathf.Cos(angle) * currentRadius;

                    Vector3 point = new Vector3(x, 0, z);

                    yield return point;
                }

                if (!fillInside)
                {
                    break;
                }

                currentRadius -= radiusStep;
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

            float pi2 = Mathf.PI * 2f;

            float l = pi2 * radius;
            float a = pi2 / (int)(l / spaceBetween);
            int n = (int)Mathf.Round(pi2 / a);

            Vector3 lastPoint = Vector3.zero;

            for (float i = 0; i <= n; i++)
            {
                float angle = i * a;
                float x = Mathf.Sin(angle) * radius;
                float z = Mathf.Cos(angle) * radius;

                Vector3 point = new Vector3(x, 0, z);

                if (i > 0)
                {
                    Handles.DrawLine(lastPoint, point);
                }

                lastPoint = point;
            }
        }
        #endregion
#endif
    }
}