/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright Renowned Games All rights reserved.
   ================================================================ */

using System;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RenownedGames.ExLibEditor
{
    public abstract class IntegrationWindow : EditorWindow, IHasCustomMenu
    {
        /// <summary>
        /// Common GUIStyles used for Integration provider GUI.
        /// </summary>
        public sealed class Styles
        {
            private Texture2D entryTexture;
            private Texture2D buttonNormalTexture;
            private Texture2D buttonHoverTexture;
            private Texture2D buttonPressTexture;
            private GUIStyle entryStyle;
            private GUIStyle buttonStyle;
            private GUIStyle titleStyle;
            private GUIContent helpIcon;

            public Styles()
            {
                entryTexture = CreateTexture(new Color32(64, 64, 64, 255), Color.black);
                buttonNormalTexture = CreateTexture(new Color32(64, 64, 64, 255), Color.black);
                buttonHoverTexture = CreateTexture(new Color32(70, 70, 70, 255), Color.black);
                buttonPressTexture = CreateTexture(new Color32(80, 80, 80, 255), Color.black);
                helpIcon = EditorGUIUtility.IconContent("_Help@2x");
            }

            /// <summary>
            /// Required to use only in GUI call.
            /// </summary>
            /// <returns></returns>
            public GUIStyle GetEntryStyle()
            {
                if (entryStyle == null)
                {
                    entryStyle = new GUIStyle();

                    entryStyle.fontSize = 12;
                    entryStyle.fontStyle = FontStyle.Normal;
                    entryStyle.alignment = TextAnchor.MiddleLeft;
                    entryStyle.border = new RectOffset(2, 2, 2, 2);
                    entryStyle.padding = new RectOffset(10, 0, 0, 0);

                    Color32 textColor = EditorGUIUtility.isProSkin ? new Color32(200, 200, 200, 255) : new Color32(3, 3, 3, 255);

                    entryStyle.normal.textColor = textColor;
                    entryStyle.normal.background = entryTexture;
                    entryStyle.normal.scaledBackgrounds = new Texture2D[1] { entryStyle.normal.background };

                    entryStyle.active.textColor = textColor;
                    entryStyle.active.background = entryStyle.normal.background;
                    entryStyle.active.scaledBackgrounds = new Texture2D[1] { entryStyle.normal.background };

                    entryStyle.focused.textColor = textColor;
                    entryStyle.focused.background = entryStyle.normal.background;
                    entryStyle.focused.scaledBackgrounds = new Texture2D[1] { entryStyle.normal.background };

                    entryStyle.hover.textColor = textColor;
                    entryStyle.hover.background = entryStyle.normal.background;
                    entryStyle.hover.scaledBackgrounds = new Texture2D[1] { entryStyle.normal.background };
                }
                return entryStyle;
            }

            /// <summary>
            /// Required to use only in GUI call.
            /// </summary>
            /// <returns></returns>
            public GUIStyle GetButtonStyle()
            {
                if (buttonStyle == null)
                {
                    buttonStyle = new GUIStyle();

                    buttonStyle.fontSize = 12;
                    buttonStyle.fontStyle = FontStyle.Normal;
                    buttonStyle.alignment = TextAnchor.MiddleCenter;
                    buttonStyle.border = new RectOffset(2, 2, 2, 2);
                    buttonStyle.padding = new RectOffset(0, 0, 0, 0);

                    Color32 textColor = EditorGUIUtility.isProSkin ? new Color32(200, 200, 200, 255) : new Color32(3, 3, 3, 255);

                    buttonStyle.normal.textColor = textColor;
                    buttonStyle.normal.background = buttonNormalTexture;
                    buttonStyle.normal.scaledBackgrounds = new Texture2D[1] { buttonStyle.normal.background };

                    buttonStyle.active.textColor = textColor;
                    buttonStyle.active.background = buttonPressTexture;
                    buttonStyle.active.scaledBackgrounds = new Texture2D[1] { buttonStyle.active.background };

                    buttonStyle.focused.textColor = textColor;
                    buttonStyle.focused.background = buttonPressTexture;
                    buttonStyle.focused.scaledBackgrounds = new Texture2D[1] { buttonStyle.focused.background };

                    buttonStyle.hover.textColor = textColor;
                    buttonStyle.hover.background = buttonHoverTexture;
                    buttonStyle.hover.scaledBackgrounds = new Texture2D[1] { buttonStyle.hover.background };
                }
                return buttonStyle;
            }

            /// <summary>
            /// Required to use only in GUI call.
            /// </summary>
            /// <returns></returns>
            public GUIStyle GetTitleStyle()
            {
                if (titleStyle == null)
                {
                    titleStyle = new GUIStyle();

                    titleStyle.fontSize = 15;
                    titleStyle.fontStyle = FontStyle.Bold;
                    titleStyle.alignment = TextAnchor.MiddleLeft;
                    titleStyle.padding = new RectOffset(0, 0, 0, 0);

                    Color32 textColor = EditorGUIUtility.isProSkin ? new Color32(200, 200, 200, 255) : new Color32(3, 3, 3, 255);

                    titleStyle.normal.textColor = textColor;
                    titleStyle.active.textColor = textColor;
                    titleStyle.focused.textColor = textColor;
                    titleStyle.hover.textColor = textColor;
                }
                return titleStyle;
            }

            /// <summary>
            /// Help icon in GUIContent representation.
            /// </summary>
            public GUIContent GetHelpIcon()
            {
                return helpIcon;
            }

            /// <summary>
            /// Create new square texture with border.
            /// </summary>
            public Texture2D CreateTexture(Color mainColor, Color borderColor)
            {
                Texture2D texture = new Texture2D(8, 8);

                Color[] colors = new Color[texture.width * texture.height];
                for (int i = 0; i < colors.Length; i++)
                {
                    colors[i] = mainColor;
                }
                texture.SetPixels(colors);

                for (int x = 0; x < texture.width; x++)
                {
                    texture.SetPixel(x, 0, borderColor);
                    texture.SetPixel(x, texture.height - 1, borderColor);
                }

                for (int y = 0; y < texture.height; y++)
                {
                    texture.SetPixel(0, y, borderColor);
                    texture.SetPixel(texture.width - 1, y, borderColor);
                }

                texture.filterMode = FilterMode.Point;
                texture.Apply();

                return texture;
            }
        }

        /// <summary>
        /// The base type for a convenient representation for reading integration info after parsing from the manifest.
        /// </summary>
        public readonly struct Integration
        {
            /// <summary>
            /// Name of integration package.
            /// </summary>
            public readonly string name;

            /// <summary>
            /// URL of integration installer.
            /// </summary>
            public readonly string url;

            /// <summary>
            /// Help or documentation of integration (url).
            /// </summary>
            public readonly string help;

            /// <summary>
            /// Integration dependencies (url)
            /// </summary>
            public readonly string[] dependencies;

            public Integration(string name, string url, string help, string[] dependencies)
            {
                this.name = name;
                this.url = url;
                this.help = help;
                this.dependencies = dependencies;
            }

            public override string ToString()
            {
                return $"[name = \"{name}\", url = \"{url}\", help = \"{url}\", dependencies = \"{dependencies?.Length ?? 0}\"]";
            }
        }

        // Stored required properties.
        private Styles styles;
        private Vector2 scollPos;
        private string searchText;
        private SearchField searchField;
        private Integration[] integrations;
        private TextAsset manifest;

        /// <summary>
        /// Called when the window becomes enabled and active.
        /// </param>
        protected virtual void OnEnable()
        {
            styles = new Styles();
            searchField = new SearchField();
            searchText = string.Empty;
            Refresh();
        }

        /// <summary>
        /// Called for rendering and handling GUI events.
        /// </summary>
        protected virtual void OnGUI()
        {
            if(manifest != null)
            {
                DrawToolbar();
                DrawTitle();
                DrawIntegrationList();
                DrawFooter();
                Repaint();
            }
            else
            {
                Rect position = new Rect(0, 0, this.position.width, this.position.height);
                GUI.Label(position, "Integration manifest not found...", EditorStyles.centeredGreyMiniLabel);
            }
        }

        /// <summary>
        /// The name of the project to determine the correct manifest for a specific project.
        /// <br>Manifest must be located in <i>ProjectName/../EditorResources/Integrations/manifest.txt</i>.</br>
        /// </summary>
        protected abstract string GetExactDirectory();

        /// <summary>
        /// The name of the project to draw as a title.
        /// </summary>
        protected abstract string GetTitle();

        /// <summary>
        /// Install integration.
        /// </summary>
        public void Install(Integration integration)
        {
            if (IsLocal(integration))
            {
                Object package = EditorResources.LoadExact<Object>(GetExactDirectory(), $"Integrations/{integration.url}");
                if(package != null)
                {
                    if (DisplayDependenciesDialog(integration))
                    {
                        AssetDatabase.OpenAsset(package.GetInstanceID());
                    }
                }
                else
                {
                    Debug.LogError($"{integration.name} integration package not found!");
                }
            }
            else
            {
                if (DisplayDependenciesDialog(integration))
                {
                    Help.BrowseURL(integration.url);
                }
            }
        }

        /// <summary>
        /// Check if integration is local.
        /// </summary>
        public bool IsLocal(Integration integration)
        {
            Uri uri = new Uri(integration.url, UriKind.RelativeOrAbsolute);
            return !uri.IsAbsoluteUri;
        }

        public void BrowseHelp(Integration integration)
        {
            if (!string.IsNullOrEmpty(integration.help))
            {
                Help.BrowseURL(integration.help);
            }
        }

        /// <summary>
        /// Refresh manifest file in editor resources.
        /// </summary>
        public void Refresh()
        {
            manifest = EditorResources.LoadExact<TextAsset>(GetExactDirectory(), "Integrations/manifest.txt");
            if (manifest != null)
            {
                integrations = LoadIntegrations(manifest.ToString());
            }
        }

        /// <summary>
        /// Open current manifest with associated application..
        /// </summary>
        public void OpenManifest()
        {
            if(manifest != null)
            {
                AssetDatabase.OpenAsset(manifest);
            }
            else
            {
                Debug.LogError("Manifest file not found!");
            }
        }

        /// <summary>
        /// Load all integrations from manifest in convenient representation.
        /// </summary>
        /// <param name="manifest">Integrations manifest.</param>
        protected Integration[] LoadIntegrations(string manifest)
        {
            const string PATTERN = @"\[name\s*=\s*""([^""]+)""\s*,\s*url\s*=\s*""([^""]+)""(?:\s*,\s*help\s*=\s*""([^""]+)"")?(?:\s*,\s*dependencies\s*=\s*""([^""]+)"")?\]";
            MatchCollection matches = Regex.Matches(manifest, PATTERN);

            Integration[] integrations = new Integration[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                Match match = matches[i];
                string name = match.Groups[1].Value;
                string url = match.Groups[2].Value;
                string help = match.Groups[3].Value;
                string dependencies = match.Groups[4].Value;

                string[] dependencyList = dependencies.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < dependencyList.Length; j++)
                {
                    dependencyList[j] = dependencyList[j].Trim();
                }

                integrations[i] = new Integration(name, url, help, dependencyList);
            }
            return integrations;
        }

        /// <summary>
        /// Show display display dialog if integration has dependencies.
        /// </summary>
        /// <returns>True if user click continue the installation. Otherwise false.</returns>
        protected bool DisplayDependenciesDialog(Integration integration)
        {
            if (integration.dependencies.Length > 0)
            {
                int id = EditorUtility.DisplayDialogComplex("Integration Installer",
                                                      "Before installing this integration, you need to install all dependencies.\nInstallation without dependencies can cause errors.",
                                                      "Continue",
                                                      "Cancel",
                                                      "View Dependencies");

                if (id == 1)
                {
                    return false;
                }
                else if (id == 2)
                {
                    for (int j = 0; j < integration.dependencies.Length; j++)
                    {
                        Help.BrowseURL(integration.dependencies[j]);
                    }
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Draw window toolbar GUI.
        /// </summary>
        private void DrawToolbar()
        {
            Rect position = new Rect(-1, -1, this.position.width + 2, 22);
            GUI.Box(position, string.Empty, styles.GetEntryStyle());

            Rect searchPosition = new Rect(position.xMax - 248, position.y + 3, 246, 20);
            searchText = searchField.OnToolbarGUI(searchPosition, searchText);
        }

        /// <summary>
        /// Draw window title.
        /// </summary>
        private void DrawTitle()
        {
            Rect position = new Rect(20, 27, this.position.width - 20, 20);
            GUI.Label(position, GetTitle(), styles.GetTitleStyle());
        }

        /// <summary>
        /// Draw all integrations.
        /// </summary>
        private void DrawIntegrationList()
        {
            Rect position = new Rect(15, 55, this.position.width - 30, this.position.height - 77);
            Rect viewRect = new Rect(0, 0, position.width, integrations.Length * 22);

            if (position.height < viewRect.height)
            {
                position.width += 15;
            }

            scollPos = GUI.BeginScrollView(position, scollPos, viewRect);

            Rect entryPosition = new Rect(0, 0, viewRect.width, 22);
            for (int i = 0; i < integrations.Length; i++)
            {
                Integration integration = integrations[i];

                if (!string.IsNullOrEmpty(searchText) && !integration.name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                GUI.Box(entryPosition, integration.name, styles.GetEntryStyle());

                entryPosition.x = entryPosition.xMax -= 50;
                entryPosition.width = 50;
                if (GUI.Button(entryPosition, "Install", styles.GetButtonStyle()))
                {
                    Install(integration);
                    GUI.FocusControl(string.Empty);
                }

                EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(integration.help));
                entryPosition.x -= 24;
                entryPosition.width = 25;
                if (GUI.Button(entryPosition, styles.GetHelpIcon(), styles.GetButtonStyle()))
                {
                    BrowseHelp(integration);
                    GUI.FocusControl(string.Empty);
                }
                EditorGUI.EndDisabledGroup();

                entryPosition.x = 0;
                entryPosition.y = entryPosition.yMax - 1;
                entryPosition.width = viewRect.width;
            }
            GUI.EndScrollView();
        }

        /// <summary>
        /// Draw window footer.
        /// </summary>
        private void DrawFooter()
        {
            Rect position = new Rect(-1, this.position.height - 22, this.position.width + 2, 22);
            GUI.Box(position, string.Empty, styles.GetEntryStyle());
        }

        #region [IHasCustomMenu Implementation]
        /// <summary>
        /// Adds your custom menu items to an Editor Window.
        /// </summary>
        public void AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Refresh"), false, Refresh);
            menu.AddItem(new GUIContent("Open Manifest"), false, OpenManifest);
        }
        #endregion

        #region [Static]
        /// <summary>
        /// Shortcut to open integration window.
        /// </summary>
        /// <param name="title">Window title.</param>
        /// <param name="size">Window size.</param>
        protected static void Open<T>(GUIContent content) where T : IntegrationWindow
        {
            T window = GetWindow<T>();
            window.titleContent = content;
            window.minSize = new Vector2(250, 175);
            window.maxSize = new Vector2(800, 600);
            window.Show();
        }
        #endregion

        #region [Getter / Setter]
        public Integration[] GetIntegrations()
        {
            return integrations;
        }
        #endregion
    }
}