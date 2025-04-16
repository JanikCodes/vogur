/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using UnityEngine;
using UnityEngine.UIElements;

namespace RenownedGames.AITreeEditor.UIElements
{
    public class NodeProgressView : BindableElement
    {
        public const string USS_CLASS_NAME = "node-progress-bar";
        public const string USS_PROGRESS_CLASS_NAME = USS_CLASS_NAME + "__progress";

        public new class UxmlFactory : UxmlFactory<NodeProgressView, UxmlTraits> { }

        private float value;
        private VisualElement progressElement;
        private Gradient gradient;
        private AITreeSettings settings;

        public NodeProgressView()
        {
            VisualElement root = this;
            root.AddToClassList(USS_CLASS_NAME);

            progressElement = new VisualElement();
            progressElement.AddToClassList(USS_PROGRESS_CLASS_NAME);
            root.Add(progressElement);

            gradient = CreateGradient();

            RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);

            settings = AITreeSettings.instance;
        }

        private Gradient CreateGradient()
        {
            Gradient gradient = new Gradient();

            GradientColorKey[] colorKeys = new GradientColorKey[3]
            {
                new GradientColorKey(Color.white, 0f),
                new GradientColorKey(Color.yellow, 0.5f),
                new GradientColorKey(Color.red, 1f)
            };

            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[3]
            {
                new GradientAlphaKey(1f, 0),
                new GradientAlphaKey(1f, 0.5f),
                new GradientAlphaKey(1f, 1f)
            };

            gradient.SetKeys(colorKeys, alphaKeys);
            gradient.mode = GradientMode.Blend;
            return gradient;
        }

        /// <summary>
        /// Called on geometry changed event.
        /// <br>This event is sent after layout calculations, when the position or the dimension
        /// of an element changes.</br>
        /// </summary>
        private void OnGeometryChanged(GeometryChangedEvent e)
        {
            UpdateProgress();
        }

        /// <summary>
        /// Update line of progress. 
        /// </summary>
        private void UpdateProgress()
        {
            StyleColor styleColor = progressElement.style.backgroundColor;
            styleColor.value = Color.white;

            if (settings.GetNodeProgressMode() == AITreeSettings.NodeProgressMode.TopToBottom)
            {
                value = 100 - value;

                if (settings.UseNodeProgressGradient())
                {
                    styleColor.value = gradient.Evaluate(Mathf.InverseLerp(100, 0, value));
                    progressElement.style.backgroundColor = styleColor;
                }
            }

            StyleLength styleLength = progressElement.style.height;
            styleLength.value = new Length(value, LengthUnit.Percent);
            progressElement.style.height = styleLength;
            progressElement.style.backgroundColor = styleColor;
        }

        #region [Getter / Setter]
        public void SetValue(float value)
        {
            this.value = Mathf.Clamp01(value) * 100f;
            UpdateProgress();
        }

        public float GetValue()
        {
            return value;
        }
        #endregion
    }
}