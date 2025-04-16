/* ================================================================
   ---------------------------------------------------
   Project   :    Apex
   Publisher :    Renowned Games
   Developer :    Tamerlan Shakirov
   ---------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.ExLibEditor;
using UnityEditor;
using UnityEngine;

namespace RenownedGames.ApexEditor
{
    public static class ApexStyles
    {
        private static string ThemeFolder;
        static ApexStyles()
        {
            ThemeFolder = EditorGUIUtility.isProSkin ? "DarkTheme" : "LightTheme";
        }

        private static GUIStyle _Label;
        public static GUIStyle Label
        {
            get
            {
                if (_Label == null)
                {
                    _Label = new GUIStyle();

                    _Label.fontSize = 12;
                    _Label.fontStyle = FontStyle.Normal;
                    _Label.alignment = TextAnchor.MiddleLeft;

                    Color32 textColor = EditorGUIUtility.isProSkin ? new Color32(200, 200, 200, 255) : new Color32(3, 3, 3, 255);

                    _Label.normal.textColor = textColor;
                    _Label.onNormal.textColor = textColor;
                    _Label.active.textColor = textColor;
                    _Label.onActive.textColor = textColor;
                    _Label.focused.textColor = textColor;
                    _Label.onFocused.textColor = textColor;
                    _Label.hover.textColor = textColor;
                    _Label.onHover.textColor = textColor;
                }
                return _Label;
            }
        }

        private static GUIStyle _LabelBold;
        public static GUIStyle LabelBold
        {
            get
            {
                if (_LabelBold == null)
                {
                    _LabelBold = new GUIStyle(Label);
                    _LabelBold.fontStyle = FontStyle.Bold;
                }
                return _LabelBold;
            }
        }

        private static GUIStyle _SuffixMessage;
        public static GUIStyle SuffixMessage
        {
            get
            {
                if (_SuffixMessage == null)
                {
                    _SuffixMessage = new GUIStyle(Label);
                    _SuffixMessage.fontSize = 10;
                    _SuffixMessage.alignment = TextAnchor.MiddleRight;
                    _SuffixMessage.fontStyle = FontStyle.Italic;
                }
                return _SuffixMessage;
            }
        }

        private static GUIStyle _BoxHeader;
        public static GUIStyle BoxHeader
        {
            get
            {
                if (_BoxHeader == null)
                {
                    _BoxHeader = new GUIStyle();

                    _BoxHeader.fontSize = 12;
                    _BoxHeader.fontStyle = FontStyle.Bold;
                    _BoxHeader.alignment = TextAnchor.MiddleLeft;
                    _BoxHeader.border = new RectOffset(2, 2, 2, 2);
                    _BoxHeader.padding = new RectOffset(10, 0, 2, 2);

                    string theme = EditorGUIUtility.isProSkin ? "DarkTheme" : "LightTheme";
                    Color32 textColor = EditorGUIUtility.isProSkin ? new Color32(200, 200, 200, 255) : new Color32(3, 3, 3, 255);

                    _BoxHeader.normal.textColor = textColor;
                    _BoxHeader.normal.background = EditorResources.Load<Texture2D>($"Textures/Container/{theme}/NormalTexture.png");
                    _BoxHeader.normal.scaledBackgrounds = new Texture2D[1] { _BoxHeader.normal.background };
                }
                return _BoxHeader;
            }
        }

        private static GUIStyle _BoxEntryBkg;
        public static GUIStyle BoxEntryBkg
        {
            get
            {
                if (_BoxEntryBkg == null)
                {

                    _BoxEntryBkg = new GUIStyle();

                    _BoxEntryBkg.fontSize = 12;
                    _BoxEntryBkg.fontStyle = FontStyle.Normal;
                    _BoxEntryBkg.alignment = TextAnchor.MiddleLeft;
                    _BoxEntryBkg.border = new RectOffset(2, 2, 2, 2);
                    _BoxEntryBkg.padding = new RectOffset(10, 0, 2, 2);

                    string theme = EditorGUIUtility.isProSkin ? "DarkTheme" : "LightTheme";
                    Color32 textColor = EditorGUIUtility.isProSkin ? new Color32(200, 200, 200, 255) : new Color32(3, 3, 3, 255);

                    _BoxEntryBkg.normal.textColor = textColor;
                    _BoxEntryBkg.normal.background = EditorResources.Load<Texture2D>($"Textures/Container/{theme}/EntryBkgTexture.png");
                    _BoxEntryBkg.normal.scaledBackgrounds = new Texture2D[1] { _BoxEntryBkg.normal.background };

                    _BoxEntryBkg.active.textColor = textColor;
                    _BoxEntryBkg.active.background = _BoxEntryBkg.normal.background;
                    _BoxEntryBkg.active.scaledBackgrounds = new Texture2D[1] { _BoxEntryBkg.normal.background };

                    _BoxEntryBkg.focused.textColor = textColor;
                    _BoxEntryBkg.focused.background = _BoxEntryBkg.normal.background;
                    _BoxEntryBkg.focused.scaledBackgrounds = new Texture2D[1] { _BoxEntryBkg.normal.background };

                    _BoxEntryBkg.hover.textColor = textColor;
                    _BoxEntryBkg.hover.background = _BoxEntryBkg.normal.background;
                    _BoxEntryBkg.hover.scaledBackgrounds = new Texture2D[1] { _BoxEntryBkg.normal.background };
                }
                return _BoxEntryBkg;
            }
        }

        private static GUIStyle _BoxButton;
        public static GUIStyle BoxButton
        {
            get
            {
                if (_BoxButton == null)
                {
                    _BoxButton = new GUIStyle(BoxHeader);

                    string theme = EditorGUIUtility.isProSkin ? "DarkTheme" : "LightTheme";
                    Color32 textColor = EditorGUIUtility.isProSkin ? new Color32(200, 200, 200, 255) : new Color32(3, 3, 3, 255);

                    _BoxButton.active.textColor = textColor;
                    _BoxButton.onActive.textColor = textColor;
                    _BoxButton.active.background = EditorResources.Load<Texture2D>($"Textures/Container/{theme}/PressTexture.png");
                    _BoxButton.onActive.background = _BoxButton.active.background;
                    _BoxButton.active.scaledBackgrounds = new Texture2D[1] { _BoxButton.active.background };
                    _BoxButton.onActive.scaledBackgrounds = new Texture2D[1] { _BoxButton.active.background };

                    _BoxButton.focused.textColor = textColor;
                    _BoxButton.onFocused.textColor = textColor;
                    _BoxButton.focused.background = EditorResources.Load<Texture2D>($"Textures/Container/{theme}/HoverTexture.png");
                    _BoxButton.onFocused.background = _BoxHeader.focused.background;
                    _BoxButton.focused.scaledBackgrounds = new Texture2D[1] { _BoxButton.focused.background };
                    _BoxButton.onFocused.scaledBackgrounds = new Texture2D[1] { _BoxButton.focused.background };

                    _BoxButton.hover.textColor = textColor;
                    _BoxButton.onHover.textColor = textColor;
                    _BoxButton.hover.background = EditorResources.Load<Texture2D>($"Textures/Container/{theme}/HoverTexture.png");
                    _BoxButton.onHover.background = _BoxHeader.hover.background;
                    _BoxButton.hover.scaledBackgrounds = new Texture2D[1] { _BoxButton.hover.background };
                    _BoxButton.onHover.scaledBackgrounds = new Texture2D[1] { _BoxButton.hover.background };
                }
                return _BoxButton;
            }
        }

        private static GUIStyle _BoxCenteredButton;
        public static GUIStyle BoxCenteredButton
        {
            get
            {
                if (_BoxCenteredButton == null)
                {
                    _BoxCenteredButton = new GUIStyle(BoxButton);
                    _BoxCenteredButton.fontStyle = FontStyle.Normal;
                    _BoxCenteredButton.alignment = TextAnchor.MiddleCenter;
                    _BoxCenteredButton.padding = new RectOffset(0, 0, 0, 0);
                }
                return _BoxCenteredButton;
            }
        }

        private static GUIStyle _BoxEntryEven;
        public static GUIStyle BoxEntryEven
        {
            get
            {
                if (_BoxEntryEven == null)
                {
                    _BoxEntryEven = new GUIStyle();

                    _BoxEntryEven.fontSize = 12;
                    _BoxEntryEven.fontStyle = FontStyle.Normal;
                    _BoxEntryEven.alignment = TextAnchor.MiddleLeft;
                    _BoxEntryEven.border = new RectOffset(2, 2, 2, 2);
                    _BoxEntryEven.padding = new RectOffset(10, 0, 0, 1);

                    string theme = EditorGUIUtility.isProSkin ? "DarkTheme" : "LightTheme";
                    Color32 textColor = EditorGUIUtility.isProSkin ? new Color32(200, 200, 200, 255) : new Color32(3, 3, 3, 255);

                    _BoxEntryEven.normal.textColor = textColor;
                    _BoxEntryEven.onNormal.textColor = textColor;
                    _BoxEntryEven.normal.background = EditorResources.Load<Texture2D>($"Textures/Container/{theme}/EntryEvenTexture.png");
                    _BoxEntryEven.onNormal.background = EditorResources.Load<Texture2D>($"Textures/Container/{theme}/EntryActiveTexture.png");
                    _BoxEntryEven.normal.scaledBackgrounds = new Texture2D[1] { _BoxEntryEven.normal.background };
                    _BoxEntryEven.onNormal.scaledBackgrounds = new Texture2D[1] { _BoxEntryEven.onNormal.background };

                    _BoxEntryEven.active.textColor = textColor;
                    _BoxEntryEven.onActive.textColor = textColor;
                    _BoxEntryEven.active.background = _BoxEntryEven.onNormal.background;
                    _BoxEntryEven.onActive.background = _BoxEntryEven.onNormal.background;
                    _BoxEntryEven.active.scaledBackgrounds = new Texture2D[1] { _BoxEntryEven.onNormal.background };
                    _BoxEntryEven.onActive.scaledBackgrounds = new Texture2D[1] { _BoxEntryEven.onNormal.background };

                    _BoxEntryEven.focused.textColor = textColor;
                    _BoxEntryEven.onFocused.textColor = textColor;
                    _BoxEntryEven.focused.background = EditorResources.Load<Texture2D>($"Textures/Container/{theme}/EntryFocusedTexture.png");
                    _BoxEntryEven.onFocused.background = _BoxEntryEven.focused.background;
                    _BoxEntryEven.focused.scaledBackgrounds = new Texture2D[1] { _BoxEntryEven.focused.background };
                    _BoxEntryEven.onFocused.scaledBackgrounds = new Texture2D[1] { _BoxEntryEven.focused.background };

                    _BoxEntryEven.hover.textColor = textColor;
                    _BoxEntryEven.onHover.textColor = textColor;
                    _BoxEntryEven.hover.background = _BoxEntryEven.normal.background;
                    _BoxEntryEven.onHover.background = _BoxEntryEven.normal.background;
                    _BoxEntryEven.hover.scaledBackgrounds = new Texture2D[1] { _BoxEntryEven.normal.background };
                    _BoxEntryEven.onHover.scaledBackgrounds = new Texture2D[1] { _BoxEntryEven.normal.background };
                }
                return _BoxEntryEven;
            }
        }

        private static GUIStyle _BoxEntryOdd;
        public static GUIStyle BoxEntryOdd
        {
            get
            {
                if (_BoxEntryOdd == null)
                {
                    _BoxEntryOdd = new GUIStyle();

                    _BoxEntryOdd.fontSize = 12;
                    _BoxEntryOdd.fontStyle = FontStyle.Normal;
                    _BoxEntryOdd.alignment = TextAnchor.MiddleLeft;
                    _BoxEntryOdd.border = new RectOffset(2, 2, 2, 2);
                    _BoxEntryOdd.padding = new RectOffset(10, 0, 0, 1);

                    string theme = EditorGUIUtility.isProSkin ? "DarkTheme" : "LightTheme";
                    Color32 textColor = EditorGUIUtility.isProSkin ? new Color32(200, 200, 200, 255) : new Color32(3, 3, 3, 255);

                    _BoxEntryOdd.normal.textColor = textColor;
                    _BoxEntryOdd.onNormal.textColor = textColor;
                    _BoxEntryOdd.normal.background = EditorResources.Load<Texture2D>($"Textures/Container/{theme}/EntryOddTexture.png");
                    _BoxEntryOdd.onNormal.background = EditorResources.Load<Texture2D>($"Textures/Container/{theme}/EntryActiveTexture.png");
                    _BoxEntryOdd.normal.scaledBackgrounds = new Texture2D[1] { _BoxEntryOdd.normal.background };
                    _BoxEntryOdd.onNormal.scaledBackgrounds = new Texture2D[1] { _BoxEntryOdd.onNormal.background };

                    _BoxEntryOdd.active.textColor = textColor;
                    _BoxEntryOdd.onActive.textColor = textColor;
                    _BoxEntryOdd.active.background = _BoxEntryOdd.onNormal.background;
                    _BoxEntryOdd.onActive.background = _BoxEntryOdd.onNormal.background;
                    _BoxEntryOdd.active.scaledBackgrounds = new Texture2D[1] { _BoxEntryOdd.onNormal.background };
                    _BoxEntryOdd.onActive.scaledBackgrounds = new Texture2D[1] { _BoxEntryOdd.onNormal.background };

                    _BoxEntryOdd.focused.textColor = textColor;
                    _BoxEntryOdd.onFocused.textColor = textColor;
                    _BoxEntryOdd.focused.background = EditorResources.Load<Texture2D>($"Textures/Container/{theme}/EntryFocusedTexture.png");
                    _BoxEntryOdd.onFocused.background = _BoxEntryOdd.focused.background;
                    _BoxEntryOdd.focused.scaledBackgrounds = new Texture2D[1] { _BoxEntryOdd.focused.background };
                    _BoxEntryOdd.onFocused.scaledBackgrounds = new Texture2D[1] { _BoxEntryOdd.focused.background };

                    _BoxEntryOdd.hover.textColor = textColor;
                    _BoxEntryOdd.onHover.textColor = textColor;
                    _BoxEntryOdd.hover.background = _BoxEntryOdd.normal.background;
                    _BoxEntryOdd.onHover.background = _BoxEntryOdd.normal.background;
                    _BoxEntryOdd.hover.scaledBackgrounds = new Texture2D[1] { _BoxEntryOdd.normal.background };
                    _BoxEntryOdd.onHover.scaledBackgrounds = new Texture2D[1] { _BoxEntryOdd.normal.background };
                }
                return _BoxEntryOdd;
            }
        }

        private static GUIStyle _BoldFoldout;
        public static GUIStyle BoldFoldout
        {
            get
            {
                if(_BoldFoldout == null)
                {
                    _BoldFoldout = new GUIStyle(EditorStyles.foldout);
                    _BoldFoldout.padding = new RectOffset(14, 0, 3, 3);
                    _BoldFoldout.fontStyle = FontStyle.Bold;
                }
                return _BoldFoldout;
            }
        }

        #region [OLD]
        private static GUIStyle _Header = null;
        [System.Obsolete]
        public static GUIStyle Header
        {
            get
            {
                if (_Header == null)
                {
                    Texture2D texture = EditorResources.Load<Texture2D>($"Images/Textures/{ThemeFolder}/HeaderTexture.png");

                    _Header = new GUIStyle();
                    _Header.normal.background = texture;
                    _Header.normal.scaledBackgrounds = new Texture2D[1] { texture };
                    _Header.border = new RectOffset(2, 2, 2, 2);
                }
                return _Header;
            }
        }

        private static Texture _ContentBackgroundTexture = null;
        [System.Obsolete]
        public static Texture ContentBackgroundTexture
        {
            get
            {
                if (_ContentBackgroundTexture == null)
                {
                    _ContentBackgroundTexture = EditorResources.Load<Texture2D>($"Images/Textures/{ThemeFolder}/ContentBackgroundTexture.png");

                }
                return _ContentBackgroundTexture;
            }
        }

        private static GUIStyle _ContentBackground = null;
        [System.Obsolete]
        public static GUIStyle ContentBackground
        {
            get
            {
                if (_ContentBackground == null)
                {
                    Texture2D texture = EditorResources.Load<Texture2D>($"Images/Textures/{ThemeFolder}/ContentBackgroundTexture.png");


                    _ContentBackground = new GUIStyle();
                    _ContentBackground.normal.background = texture;
                    _ContentBackground.normal.scaledBackgrounds = new Texture2D[1] { texture };
                    _ContentBackground.border = new RectOffset(2, 2, 2, 2);
                }
                return _ContentBackground;
            }
        }

        private static GUIStyle _Button = null;
        [System.Obsolete]
        public static GUIStyle Button
        {
            get
            {
                if (_Button == null)
                {
                    Texture2D normalTexture = EditorResources.Load<Texture2D>($"Images/Textures/{ThemeFolder}/HeaderTexture.png");
                    Texture2D hoverTexture = EditorResources.Load<Texture2D>($"Images/Textures/{ThemeFolder}/HoverTexture.png");
                    Texture2D pressTexture = EditorResources.Load<Texture2D>($"Images/Textures/{ThemeFolder}/PressTexture.png");

                    _Button = new GUIStyle();
                    _Button.alignment = TextAnchor.MiddleCenter;
                    _Button.border = new RectOffset(2, 2, 2, 2);

                    _Button.normal.textColor = Color.white;
                    _Button.normal.background = normalTexture;
                    _Button.normal.scaledBackgrounds = new Texture2D[1] { normalTexture };
                    _Button.onNormal.background = normalTexture;
                    _Button.onNormal.scaledBackgrounds = new Texture2D[1] { normalTexture };

                    _Button.hover.textColor = Color.white;
                    _Button.hover.background = hoverTexture;
                    _Button.hover.scaledBackgrounds = new Texture2D[1] { hoverTexture };
                    _Button.onHover.background = hoverTexture;
                    _Button.onHover.scaledBackgrounds = new Texture2D[1] { hoverTexture };

                    _Button.focused.textColor = Color.white;
                    _Button.focused.background = hoverTexture;
                    _Button.focused.scaledBackgrounds = new Texture2D[1] { hoverTexture };
                    _Button.onFocused.background = hoverTexture;
                    _Button.onFocused.scaledBackgrounds = new Texture2D[1] { hoverTexture };

                    _Button.active.textColor = Color.white;
                    _Button.active.background = pressTexture;
                    _Button.active.scaledBackgrounds = new Texture2D[1] { pressTexture };
                    _Button.onActive.background = pressTexture;
                    _Button.onActive.scaledBackgrounds = new Texture2D[1] { pressTexture };
                }
                return _Button;
            }
        }

        private static GUIStyle _ButtonItem = null;
        [System.Obsolete]
        public static GUIStyle ButtonItem
        {
            get
            {
                if (_ButtonItem == null)
                {
                    _ButtonItem = new GUIStyle(Button);
                    _ButtonItem.fontSize = 12;
                    _ButtonItem.alignment = TextAnchor.MiddleLeft;
                    _ButtonItem.contentOffset = new Vector2(10, 0);

                    _ButtonItem.normal.textColor = new Color(0.8f ,0.8f, 0.8f, 1.0f);
                    _ButtonItem.onNormal.textColor = new Color(0.8f, 0.8f, 0.8f, 1.0f);

                    _ButtonItem.hover.textColor = Color.white;
                    _ButtonItem.onHover.textColor = Color.white;

                    _ButtonItem.focused.textColor = Color.white;
                    _ButtonItem.onFocused.textColor = Color.white;

                    _ButtonItem.active.textColor = Color.white;
                    _ButtonItem.onActive.textColor = Color.white;
                }
                return _ButtonItem;
            }
        }

        private static GUIStyle _TabButton = null;
        [System.Obsolete]
        public static GUIStyle TabButton
        {
            get
            {
                if (_TabButton == null)
                {
                    _TabButton = new GUIStyle(Button);
                    _TabButton.wordWrap = true;
                    _TabButton.fontSize = HeaderLabel.fontSize;
                    _TabButton.fontStyle = HeaderLabel.fontStyle;
                    _TabButton.normal.textColor = HeaderLabel.normal.textColor;
                    _TabButton.active.textColor = HeaderLabel.active.textColor;
                    _TabButton.focused.textColor = HeaderLabel.focused.textColor;
                    _TabButton.hover.textColor = HeaderLabel.hover.textColor;
                    _TabButton.onNormal.textColor = HeaderLabel.normal.textColor;
                    _TabButton.onActive.textColor = HeaderLabel.active.textColor;
                    _TabButton.onFocused.textColor = HeaderLabel.focused.textColor;
                    _TabButton.onHover.textColor = HeaderLabel.hover.textColor;
                }
                return _TabButton;
            }
        }

        private static GUIStyle _SettingsButton = null;
        [System.Obsolete]
        public static GUIStyle ActionButton
        {
            get
            {
                if (_SettingsButton == null)
                {
                    _SettingsButton = new GUIStyle(Button);
                    _SettingsButton.alignment = TextAnchor.MiddleCenter;
                }
                return _SettingsButton;
            }
        }

        private static GUIStyle _HeaderLabel = null;
        [System.Obsolete]
        public static GUIStyle HeaderLabel
        {
            get
            {
                if(_HeaderLabel == null)
                {
                    _HeaderLabel = new GUIStyle(GUI.skin.label);
                    _HeaderLabel.fontStyle = FontStyle.Bold;
                    _HeaderLabel.fontSize = 12;
                    _HeaderLabel.alignment = TextAnchor.MiddleCenter;
                }
                return _HeaderLabel;
            }
        }

        private static GUIStyle _ItalicLabel = null;
        [System.Obsolete]
        public static GUIStyle ItalicLabel
        {
            get
            {
                if (_ItalicLabel == null)
                {
                    _ItalicLabel = new GUIStyle(GUI.skin.label);
                    _ItalicLabel.fontStyle = FontStyle.Italic;
                }
                return _ItalicLabel;
            }
        }
        #endregion
    }
}