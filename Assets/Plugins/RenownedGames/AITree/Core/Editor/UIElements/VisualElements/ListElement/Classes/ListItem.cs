/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace RenownedGames.AITreeEditor.UIElements
{
    public struct ListItem
    {
        private string name;
        private string category;
        private string tooltip;
        private Color color;

        /// <summary>
        /// ListItem constructor.
        /// </summary>
        /// <param name="name">Name of item.</param>
        /// <param name="onClick">Called when item is clicked.</param>
        public ListItem(string name, Action<VisualElement> onClick, Action<VisualElement> onDelete)
        {
            this.name = name;
            this.OnClick = onClick;
            this.OnDelete = onDelete;
            category = string.Empty;
            tooltip = string.Empty;
            color = Color.white;
        }

        /// <summary>
        /// ListItem constructor.
        /// </summary>
        /// <param name="name">Name of item.</param>
        /// <param name="category">Category of item.</param>
        /// <param name="onClick">Called when item is clicked.</param>
        public ListItem(string name, string category, Action<VisualElement> onClick, Action<VisualElement> onDelete) : this(name, onClick, onDelete)
        {
            if (!string.IsNullOrEmpty(category))
            {
                this.category = category.Trim();
            }
        }

        /// <summary>
        /// ListItem constructor.
        /// </summary>
        /// <param name="name">Name of item.</param>
        /// <param name="category">Category of item.</param>
        /// <param name="tooltip">Tooltip of item, displayed when mouse over item.</param>
        /// <param name="onClick">Called when item is clicked.</param>
        public ListItem(string name, string category, string tooltip, Action<VisualElement> onClick, Action<VisualElement> onDelete) : this(name, category, onClick, onDelete)
        {
            if (!string.IsNullOrEmpty(tooltip))
            {
                this.tooltip = tooltip.Trim();
            }
        }

        /// <summary>
        /// ListItem constructor.
        /// </summary>
        /// <param name="name">Name of item.</param>
        /// <param name="category">Category of item.</param>
        /// <param name="tooltip">Tooltip of item, displayed when mouse over item.</param>
        /// <param name="color">Color of item, by default is white.</param>
        /// <param name="onClick">Called when item is clicked.</param>
        public ListItem(string name, string category, string tooltip, Color color, Action<VisualElement> onClick, Action<VisualElement> onDelete) : this(name, category, tooltip, onClick, onDelete)
        {
            this.color = color;
        }

        #region [Event Action]
        /// <summary>
        /// Called when item is clicked.
        /// </summary>
        public Action<VisualElement> OnClick;

        /// <summary>
        /// Called when item is deleting.
        /// </summary>
        public Action<VisualElement> OnDelete;
        #endregion

        #region [Getter / Setter]
        public string GetName()
        {
            return name;
        }

        public void SetName(string value)
        {
            name = value;
        }

        public string GetCategory()
        {
            return category;
        }

        public void SetCategory(string value)
        {
            category = value;
        }

        public string GetTooltip()
        {
            return tooltip;
        }

        public void SetTooltip(string value)
        {
            tooltip = value;
        }

        public Color GetColor()
        {
            return color;
        }

        public void SetColor(Color value)
        {
            color = value;
        }
        #endregion
    }
}
