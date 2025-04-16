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
    [SearchContent("Grid", Image = "Images/Icons/EQS/Generators/GridGenIcon.png")]
    public class EQGridGenerator : EQGenerator
    {
        [SerializeField]
        [MinVector2(1f, "generator.halfSize.x")]
        private Vector2 halfSize = new Vector2(10, 10);

        [SerializeField]
        [MinValue(.2f)]
        private float spaceBetween = 2f;

        /// <summary>
        /// Returns points created by the generator.
        /// </summary>
        protected override IEnumerable<Vector3> CalculatePoints()
        {
            float offsetX = halfSize.x % spaceBetween;
            float offsetY = halfSize.y % spaceBetween;

            float offsetXHalfSize = halfSize.x - offsetX;
            float offsetYHalfSize = halfSize.y - offsetY;

            for (float x = -offsetXHalfSize; x <= offsetXHalfSize; x += spaceBetween)
            {
                for (float y = -offsetYHalfSize; y <= offsetYHalfSize; y += spaceBetween)
                {
                    yield return new Vector3(x, 0, y);
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
            Handles.DrawWireCube(Vector3.zero, new Vector3(halfSize.x, 0, halfSize.y) * 2);
        }
        #endregion
#endif
    }
}