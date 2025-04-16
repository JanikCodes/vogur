/* ================================================================
   ----------------------------------------------------------------
   Project   :   ExLib
   Company   :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright Renowned Games All rights reserved.
   ================================================================ */

using UnityEditor;
using UnityEngine;

namespace RenownedGames.ExLibEditor
{
    public static class ExEditorStyles
    {
        private static GUIStyle _ArrayLabel;
        public static GUIStyle Label
        {
            get
            {
                if (_ArrayLabel == null)
                {
                    _ArrayLabel = new GUIStyle();

                    _ArrayLabel.fontSize = 12;
                    _ArrayLabel.fontStyle = FontStyle.Normal;
                    _ArrayLabel.alignment = TextAnchor.MiddleLeft;

                    Color32 textColor = EditorGUIUtility.isProSkin ? new Color32(200, 200, 200, 255) : new Color32(3, 3, 3, 255);

                    _ArrayLabel.normal.textColor = textColor;
                    _ArrayLabel.onNormal.textColor = textColor;
                    _ArrayLabel.active.textColor = textColor;
                    _ArrayLabel.onActive.textColor = textColor;
                    _ArrayLabel.focused.textColor = textColor;
                    _ArrayLabel.onFocused.textColor = textColor;
                    _ArrayLabel.hover.textColor = textColor;
                    _ArrayLabel.onHover.textColor = textColor;
                }
                return _ArrayLabel;
            }
        }

        private static GUIStyle _ArrayLabelBold;
        public static GUIStyle LabelBold
        {
            get
            {
                if (_ArrayLabelBold == null)
                {
                    _ArrayLabelBold = new GUIStyle(Label);
                    _ArrayLabelBold.fontStyle = FontStyle.Bold;
                }
                return _ArrayLabelBold;
            }
        }

        private static GUIStyle _ArrayHeader;
        public static GUIStyle ArrayHeader
        {
            get
            {
                if (_ArrayHeader == null)
                {
                    _ArrayHeader = new GUIStyle();

                    _ArrayHeader.fontSize = 12;
                    _ArrayHeader.fontStyle = FontStyle.Bold;
                    _ArrayHeader.alignment = TextAnchor.MiddleLeft;
                    _ArrayHeader.border = new RectOffset(2, 2, 2, 2);
                    _ArrayHeader.padding = new RectOffset(10, 0, 0, 1);

                    string theme = EditorGUIUtility.isProSkin ? "DarkTheme" : "LightTheme";
                    Color32 textColor = EditorGUIUtility.isProSkin ? new Color32(200, 200, 200, 255) : new Color32(3, 3, 3, 255);

                    _ArrayHeader.normal.textColor = textColor;
                    _ArrayHeader.normal.background = EditorResources.Load<Texture2D>($"Textures/ReorderableArray/{theme}/NormalTexture.png");
                    _ArrayHeader.normal.scaledBackgrounds = new Texture2D[1] { _ArrayHeader.normal.background };
                }
                return _ArrayHeader;
            }
        }

        private static GUIStyle _ArrayEntryBkg;
        public static GUIStyle ArrayEntryBkg
        {
            get
            {
                if (_ArrayEntryBkg == null)
                {

                    _ArrayEntryBkg = new GUIStyle();

                    _ArrayEntryBkg.fontSize = 12;
                    _ArrayEntryBkg.fontStyle = FontStyle.Normal;
                    _ArrayEntryBkg.alignment = TextAnchor.MiddleLeft;
                    _ArrayEntryBkg.border = new RectOffset(2, 2, 2, 2);
                    _ArrayEntryBkg.padding = new RectOffset(10, 0, 0, 1);

                    string theme = EditorGUIUtility.isProSkin ? "DarkTheme" : "LightTheme";
                    Color32 textColor = EditorGUIUtility.isProSkin ? new Color32(200, 200, 200, 255) : new Color32(3, 3, 3, 255);

                    _ArrayEntryBkg.normal.textColor = textColor;
                    _ArrayEntryBkg.normal.background = EditorResources.Load<Texture2D>($"Textures/ReorderableArray/{theme}/EntryBkgTexture.png");
                    _ArrayEntryBkg.normal.scaledBackgrounds = new Texture2D[1] { _ArrayEntryBkg.normal.background };

                    _ArrayEntryBkg.active.textColor = textColor;
                    _ArrayEntryBkg.active.background = _ArrayEntryBkg.normal.background;
                    _ArrayEntryBkg.active.scaledBackgrounds = new Texture2D[1] { _ArrayEntryBkg.normal.background };

                    _ArrayEntryBkg.focused.textColor = textColor;
                    _ArrayEntryBkg.focused.background = _ArrayEntryBkg.normal.background;
                    _ArrayEntryBkg.focused.scaledBackgrounds = new Texture2D[1] { _ArrayEntryBkg.normal.background };

                    _ArrayEntryBkg.hover.textColor = textColor;
                    _ArrayEntryBkg.hover.background = _ArrayEntryBkg.normal.background;
                    _ArrayEntryBkg.hover.scaledBackgrounds = new Texture2D[1] { _ArrayEntryBkg.normal.background };
                }
                return _ArrayEntryBkg;
            }
        }

        private static GUIStyle _ArrayButton;
        public static GUIStyle ArrayButton
        {
            get
            {
                if (_ArrayButton == null)
                {
                    _ArrayButton = new GUIStyle(ArrayHeader);

                    string theme = EditorGUIUtility.isProSkin ? "DarkTheme" : "LightTheme";
                    Color32 textColor = EditorGUIUtility.isProSkin ? new Color32(200, 200, 200, 255) : new Color32(3, 3, 3, 255);

                    _ArrayButton.active.textColor = textColor;
                    _ArrayButton.onActive.textColor = textColor;
                    _ArrayButton.active.background = EditorResources.Load<Texture2D>($"Textures/ReorderableArray/{theme}/PressTexture.png");
                    _ArrayButton.onActive.background = _ArrayButton.active.background;
                    _ArrayButton.active.scaledBackgrounds = new Texture2D[1] { _ArrayButton.active.background };
                    _ArrayButton.onActive.scaledBackgrounds = new Texture2D[1] { _ArrayButton.active.background };

                    _ArrayButton.focused.textColor = textColor;
                    _ArrayButton.onFocused.textColor = textColor;
                    _ArrayButton.focused.background = EditorResources.Load<Texture2D>($"Textures/ReorderableArray/{theme}/HoverTexture.png");
                    _ArrayButton.onFocused.background = _ArrayHeader.focused.background;
                    _ArrayButton.focused.scaledBackgrounds = new Texture2D[1] { _ArrayButton.focused.background };
                    _ArrayButton.onFocused.scaledBackgrounds = new Texture2D[1] { _ArrayButton.focused.background };

                    _ArrayButton.hover.textColor = textColor;
                    _ArrayButton.onHover.textColor = textColor;
                    _ArrayButton.hover.background = EditorResources.Load<Texture2D>($"Textures/ReorderableArray/{theme}/HoverTexture.png");
                    _ArrayButton.onHover.background = _ArrayHeader.hover.background;
                    _ArrayButton.hover.scaledBackgrounds = new Texture2D[1] { _ArrayButton.hover.background };
                    _ArrayButton.onHover.scaledBackgrounds = new Texture2D[1] { _ArrayButton.hover.background };
                }
                return _ArrayButton;
            }
        }

        private static GUIStyle _ArrayCenteredButton;
        public static GUIStyle ArrayCenteredButton
        {
            get
            {
                if (_ArrayCenteredButton == null)
                {
                    _ArrayCenteredButton = new GUIStyle(ArrayButton);
                    _ArrayCenteredButton.fontStyle = FontStyle.Normal;
                    _ArrayCenteredButton.alignment = TextAnchor.MiddleCenter;
                    _ArrayCenteredButton.padding = new RectOffset(0, 0, 0, 0);
                }
                return _ArrayCenteredButton;
            }
        }

        private static GUIStyle _ArrayEntryEven;
        public static GUIStyle ArrayEntryEven
        {
            get
            {
                if (_ArrayEntryEven == null)
                {
                    _ArrayEntryEven = new GUIStyle();

                    _ArrayEntryEven.fontSize = 12;
                    _ArrayEntryEven.fontStyle = FontStyle.Normal;
                    _ArrayEntryEven.alignment = TextAnchor.MiddleLeft;
                    _ArrayEntryEven.border = new RectOffset(2, 2, 2, 2);
                    _ArrayEntryEven.padding = new RectOffset(10, 0, 0, 1);

                    string theme = EditorGUIUtility.isProSkin ? "DarkTheme" : "LightTheme";
                    Color32 textColor = EditorGUIUtility.isProSkin ? new Color32(200, 200, 200, 255) : new Color32(3, 3, 3, 255);

                    _ArrayEntryEven.normal.textColor = textColor;
                    _ArrayEntryEven.onNormal.textColor = textColor;
                    _ArrayEntryEven.normal.background = EditorResources.Load<Texture2D>($"Textures/ReorderableArray/{theme}/EntryEvenTexture.png");
                    _ArrayEntryEven.onNormal.background = EditorResources.Load<Texture2D>($"Textures/ReorderableArray/{theme}/EntryActiveTexture.png");
                    _ArrayEntryEven.normal.scaledBackgrounds = new Texture2D[1] { _ArrayEntryEven.normal.background };
                    _ArrayEntryEven.onNormal.scaledBackgrounds = new Texture2D[1] { _ArrayEntryEven.onNormal.background };

                    _ArrayEntryEven.active.textColor = textColor;
                    _ArrayEntryEven.onActive.textColor = textColor;
                    _ArrayEntryEven.active.background = _ArrayEntryEven.onNormal.background;
                    _ArrayEntryEven.onActive.background = _ArrayEntryEven.onNormal.background;
                    _ArrayEntryEven.active.scaledBackgrounds = new Texture2D[1] { _ArrayEntryEven.onNormal.background };
                    _ArrayEntryEven.onActive.scaledBackgrounds = new Texture2D[1] { _ArrayEntryEven.onNormal.background };

                    _ArrayEntryEven.focused.textColor = textColor;
                    _ArrayEntryEven.onFocused.textColor = textColor;
                    _ArrayEntryEven.focused.background = EditorResources.Load<Texture2D>($"Textures/ReorderableArray/{theme}/EntryFocusedTexture.png");
                    _ArrayEntryEven.onFocused.background = _ArrayEntryEven.focused.background;
                    _ArrayEntryEven.focused.scaledBackgrounds = new Texture2D[1] { _ArrayEntryEven.focused.background };
                    _ArrayEntryEven.onFocused.scaledBackgrounds = new Texture2D[1] { _ArrayEntryEven.focused.background };

                    _ArrayEntryEven.hover.textColor = textColor;
                    _ArrayEntryEven.onHover.textColor = textColor;
                    _ArrayEntryEven.hover.background = _ArrayEntryEven.normal.background;
                    _ArrayEntryEven.onHover.background = _ArrayEntryEven.normal.background;
                    _ArrayEntryEven.hover.scaledBackgrounds = new Texture2D[1] { _ArrayEntryEven.normal.background };
                    _ArrayEntryEven.onHover.scaledBackgrounds = new Texture2D[1] { _ArrayEntryEven.normal.background };
                }
                return _ArrayEntryEven;
            }
        }

        private static GUIStyle _ArrayEntryOdd;
        public static GUIStyle ArrayEntryOdd
        {
            get
            {
                if (_ArrayEntryOdd == null)
                {
                    _ArrayEntryOdd = new GUIStyle();

                    _ArrayEntryOdd.fontSize = 12;
                    _ArrayEntryOdd.fontStyle = FontStyle.Normal;
                    _ArrayEntryOdd.alignment = TextAnchor.MiddleLeft;
                    _ArrayEntryOdd.border = new RectOffset(2, 2, 2, 2);
                    _ArrayEntryOdd.padding = new RectOffset(10, 0, 0, 1);

                    string theme = EditorGUIUtility.isProSkin ? "DarkTheme" : "LightTheme";
                    Color32 textColor = EditorGUIUtility.isProSkin ? new Color32(200, 200, 200, 255) : new Color32(3, 3, 3, 255);

                    _ArrayEntryOdd.normal.textColor = textColor;
                    _ArrayEntryOdd.onNormal.textColor = textColor;
                    _ArrayEntryOdd.normal.background = EditorResources.Load<Texture2D>($"Textures/ReorderableArray/{theme}/EntryOddTexture.png");
                    _ArrayEntryOdd.onNormal.background = EditorResources.Load<Texture2D>($"Textures/ReorderableArray/{theme}/EntryActiveTexture.png");
                    _ArrayEntryOdd.normal.scaledBackgrounds = new Texture2D[1] { _ArrayEntryOdd.normal.background };
                    _ArrayEntryOdd.onNormal.scaledBackgrounds = new Texture2D[1] { _ArrayEntryOdd.onNormal.background };

                    _ArrayEntryOdd.active.textColor = textColor;
                    _ArrayEntryOdd.onActive.textColor = textColor;
                    _ArrayEntryOdd.active.background = _ArrayEntryOdd.onNormal.background;
                    _ArrayEntryOdd.onActive.background = _ArrayEntryOdd.onNormal.background;
                    _ArrayEntryOdd.active.scaledBackgrounds = new Texture2D[1] { _ArrayEntryOdd.onNormal.background };
                    _ArrayEntryOdd.onActive.scaledBackgrounds = new Texture2D[1] { _ArrayEntryOdd.onNormal.background };

                    _ArrayEntryOdd.focused.textColor = textColor;
                    _ArrayEntryOdd.onFocused.textColor = textColor;
                    _ArrayEntryOdd.focused.background = EditorResources.Load<Texture2D>($"Textures/ReorderableArray/{theme}/EntryFocusedTexture.png");
                    _ArrayEntryOdd.onFocused.background = _ArrayEntryOdd.focused.background;
                    _ArrayEntryOdd.focused.scaledBackgrounds = new Texture2D[1] { _ArrayEntryOdd.focused.background };
                    _ArrayEntryOdd.onFocused.scaledBackgrounds = new Texture2D[1] { _ArrayEntryOdd.focused.background };

                    _ArrayEntryOdd.hover.textColor = textColor;
                    _ArrayEntryOdd.onHover.textColor = textColor;
                    _ArrayEntryOdd.hover.background = _ArrayEntryOdd.normal.background;
                    _ArrayEntryOdd.onHover.background = _ArrayEntryOdd.normal.background;
                    _ArrayEntryOdd.hover.scaledBackgrounds = new Texture2D[1] { _ArrayEntryOdd.normal.background };
                    _ArrayEntryOdd.onHover.scaledBackgrounds = new Texture2D[1] { _ArrayEntryOdd.normal.background };
                }
                return _ArrayEntryOdd;
            }
        }
    }
}