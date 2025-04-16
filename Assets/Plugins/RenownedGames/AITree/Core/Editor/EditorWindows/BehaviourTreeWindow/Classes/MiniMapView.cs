/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2023 Renowned Games All rights reserved.
   ================================================================ */

using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace RenownedGames.AITreeEditor
{
    internal class MiniMapView : MiniMap
    {
        private static readonly StyleColor BackgroundColor = new StyleColor(new Color32(29, 29, 30, 255));
        private static readonly StyleColor BorderColor = new StyleColor(new Color32(51, 51, 51, 255));

        /// <summary>
        /// Mini map view constructor.
        /// </summary>
        public MiniMapView()
        {
            zoomFactorTextChanged += s =>
            {
                this.Q<Label>().text = s;
            };

            this.Q<Label>().text = "";
            this.Q<Label>().style.alignSelf = Align.Center;
            this.Q<Label>().style.unityTextAlign = TextAnchor.LowerCenter;
            this.Q<Label>().style.width = new StyleLength(new Length(100, LengthUnit.Percent));
            this.Q<Label>().style.height = new StyleLength(new Length(100, LengthUnit.Percent));

            style.backgroundColor = BackgroundColor;

            style.borderTopColor = BorderColor;
            style.borderLeftColor = BorderColor;
            style.borderBottomColor = BorderColor;
            style.borderLeftColor = BorderColor;

            style.position = Position.Absolute;
            style.right = 0;
            style.top = 0;
            style.width = 150;
            style.height = 150;
        }
    }
}