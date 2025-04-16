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

namespace RenownedGames.ExLibEditor.Windows
{
    public abstract class AboutWindow : EditorWindow
    {
        static class Styles
        {
            public static GUIStyle Title
            {
                get
                {
                    GUIStyle style = new GUIStyle(GUI.skin.label);
                    style.fontSize = 20;
                    style.fontStyle = FontStyle.Bold;
                    return style;
                }
            }

            public static GUIStyle Copyright
            {
                get
                {
                    GUIStyle style = new GUIStyle(GUI.skin.label);
                    style.fontSize = 10;
                    style.fontStyle = FontStyle.Normal;
                    style.normal.textColor = Color.gray;
                    style.onNormal.textColor = Color.gray;
                    style.active.textColor = Color.gray;
                    style.onActive.textColor = Color.gray;
                    style.hover.textColor = Color.gray;
                    style.onHover.textColor = Color.gray;
                    style.focused.textColor = Color.gray;
                    style.onFocused.textColor = Color.gray;
                    return style;
                }
            }

            public static GUIStyle Version
            {
                get
                {
                    GUIStyle style = new GUIStyle(GUI.skin.label);
                    style.fontSize = 10;
                    style.fontStyle = FontStyle.Bold;
                    style.normal.textColor = Color.gray;
                    style.onNormal.textColor = Color.gray;
                    style.active.textColor = Color.gray;
                    style.onActive.textColor = Color.gray;
                    style.hover.textColor = Color.gray;
                    style.onHover.textColor = Color.gray;
                    style.focused.textColor = Color.gray;
                    style.onFocused.textColor = Color.gray;
                    return style;
                }
            }

            public static GUIStyle Label
            {
                get
                {
                    GUIStyle style = new GUIStyle(GUI.skin.label);
                    style.alignment = TextAnchor.UpperLeft;
                    style.fontSize = 11;
                    style.richText = true;
                    style.fontStyle = FontStyle.Bold;
                    return style;
                }
            }
        }

        protected struct Developer
        {
            public readonly string position;
            public readonly string name;

            public Developer(string position, string name)
            {
                this.position = position;
                this.name = name;
            }
        }

        private float textureWidth;
        private float textureHeight;
        private string projectName;
        private string version;
        private string copyright;
        private string publisherLink;
        private Developer[] developers;
        private Texture2D logotype;

        /// <summary>
        /// Called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            InitializeDevelopers(out developers);
            InitializeLogotype(out logotype, out textureWidth, out textureHeight);
            InitializeProjectName(out projectName);
            InitializeVersion(out version);
            InitializeCopyright(out copyright);
            InitializePublisherLink(out publisherLink);
        }

        /// <summary>
        /// Called for rendering and handling GUI events.
        /// </summary>
        private void OnGUI()
        {
            float textHalfWidth = 0;
            float textHalfHeight = 29;
            textHalfHeight += string.IsNullOrEmpty(version) ? 5 : 17;
            for (int i = 0; i < developers.Length; i++)
            {
                Developer developer = developers[i];

                GUIContent content = new GUIContent(developer.position);
                Vector2 posSize = Styles.Label.CalcSize(content);
                float width = posSize.x;
                content.text = developer.name;
                Vector2 nameSize = Styles.Label.CalcSize(content);
                width += nameSize.x;
                textHalfWidth = Mathf.Max(textHalfWidth, width);
                textHalfHeight += Mathf.Max(posSize.y, nameSize.y) + 2;
            }
            textHalfWidth /= 2;
            textHalfHeight /= 2;

            Rect entryPosition = new Rect((position.width / 2) - textHalfWidth, ((position.height - 20) / 2) - textHalfHeight, 0, 0);

            if(logotype != null)
            {
                Rect logoPosition = new Rect(10, (position.height - textureHeight - 20) / 2, textureWidth, textureHeight);
                GUI.DrawTexture(logoPosition, logotype);
                entryPosition.x = logoPosition.xMax + 10;
            }

            Rect projectNamePosition = new Rect(entryPosition.xMax, entryPosition.y, position.width, 25);
            if(logotype == null)
            {
                projectNamePosition.x = (position.width - Styles.Title.CalcSize(new GUIContent(projectName)).x) / 2;
            }
            GUI.Label(projectNamePosition, projectName, Styles.Title);

            Rect versionPosition = new Rect(projectNamePosition.x, projectNamePosition.yMax + 4, position.width, 0);
            if (!string.IsNullOrEmpty(version))
            {
                if (logotype == null)
                {
                    versionPosition.x = (position.width - Styles.Version.CalcSize(new GUIContent(version)).x) / 2;
                }

                versionPosition.height = 12;
                GUI.Label(versionPosition, version, Styles.Version);
            }

            Rect devsPosition = new Rect(entryPosition.x, versionPosition.yMax + 5, position.width, 14);
            for (int i = 0; i < developers.Length; i++)
            {
                Developer developer = developers[i];
                Rect devPosition = new Rect(devsPosition.x, devsPosition.y + 3, position.width, devsPosition.height);

                GUIContent posContent = new GUIContent(developer.position);
                Vector2 posSize = Styles.Label.CalcSize(posContent);
                Rect posPosition = new Rect(devPosition.x, devPosition.y, posSize.x, posSize.y);
                GUI.Label(posPosition, posContent, Styles.Label);

                GUIContent nameContent = new GUIContent(developer.name);
                Vector2 nameSize = Styles.Label.CalcSize(nameContent);
                Rect namePosition = new Rect(posPosition.xMax, posPosition.y, nameSize.x, nameSize.y);
                GUI.Label(namePosition, nameContent, Styles.Label);

                devsPosition.y = devPosition.yMax;
            }

            Rect footerPosition = new Rect(-1, position.height - 20, position.width + 2, 20);
            GUI.Box(footerPosition, GUIContent.none, "OL Title");

            if (!string.IsNullOrEmpty(publisherLink))
            {
                Rect visitButtonPosition = new Rect(position.width - 303, position.height - 19, 170, 18);
                if (GUI.Button(visitButtonPosition, "Visit the Publisher Website"))
                {
                    Help.BrowseURL(publisherLink);
                }
            }

            Rect licenseButtonPosition = new Rect(position.width - 131, position.height - 19, 130, 18);
            if (GUI.Button(licenseButtonPosition, "License Agreement"))
            {
                Help.BrowseURL("https://unity3d.com/legal/as_terms/#section-2-end-users-rights-and-obligations");
            }

            if (!string.IsNullOrEmpty(copyright))
            {
                float width = Styles.Copyright.CalcSize(new GUIContent(copyright)).x;
                Rect copyrightPosition = new Rect(position.width - (width + 2), footerPosition.yMin - 16, width, 14);
                GUI.Label(copyrightPosition, copyright, Styles.Copyright);
            }
        }

        /// <summary>
        /// Implement this method to add project name.
        /// </summary>
        protected abstract void InitializeProjectName(out string projectName);

        /// <summary>
        /// Implement this method to add all the people involved in the development.
        /// </summary>
        protected abstract void InitializeDevelopers(out Developer[] developers);

        /// <summary>
        /// Implement this method to add version label.
        /// </summary>
        protected virtual void InitializeVersion(out string version)
        {
            version = string.Empty;
        }

        /// <summary>
        /// Implement this method to add logotype.
        /// </summary>
        public virtual void InitializeLogotype(out Texture2D texture, out float width, out float height)
        {
            texture = null;
            width = 0;
            height = 0;
        }

        /// <summary>
        /// Implement this method to add copyright.
        /// </summary>
        protected virtual void InitializeCopyright(out string copyright)
        {
            copyright = string.Empty;
        }

        /// <summary>
        /// Implement this method to add publisher link button.
        /// </summary>
        protected virtual void InitializePublisherLink(out string url)
        {
            url = string.Empty;
        }

        #region [Static Methods]
        protected static void Open<T>(GUIContent title, Vector2 size) where T : AboutWindow
        {
            T window = GetWindow<T>(true);
            window.titleContent = title;
            window.minSize = size;
            window.maxSize = size;
            window.MoveToCenter();
            window.Show();
        }
        #endregion
    }
}