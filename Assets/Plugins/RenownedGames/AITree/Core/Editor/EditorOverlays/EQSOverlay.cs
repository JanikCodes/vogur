/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.AITree.EQS;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;

namespace RenownedGames.AITreeEditor
{
    [Overlay(typeof(SceneView), "EQS Visualization")]
    public class EQSOverlay : Overlay
    {
        private const string EQ_GUID = "EQSOverlay_environmentQuery";
        private const string EQ_LOCK_GUID = "EQSOverlay_environmentQueryLock";
        private const string TRANSFORM_GUID = "EQSOverlay_transform";
        private const string TRANSFORM_LOCK_GUID = "EQSOverlay_transformLock";
        private const string SCORE_GUID = "EQSOverlay_showScore";
        private const string VISUALIZING_GUID = "EQSOverlay_visualizing";

        private sealed class Styles
        {
            private GUIStyle lockButtonStyle;
            private GUIStyle infoStyle;
            private GUIStyle miniButton;
            private Texture playIcon;
            private Texture pauseIcon;
            private Texture scoreIcon;

            public Styles()
            {
                playIcon = EditorGUIUtility.IconContent("d_PlayButton").image;
                pauseIcon = EditorGUIUtility.IconContent("d_PauseButton").image;
                scoreIcon = EditorGUIUtility.IconContent("d_Favorite").image;
            }

            /// <summary>
            /// Use it only in GUI calls.
            /// </summary>
            public GUIStyle GetInfoStyle()
            {
                if (infoStyle == null)
                {
                    infoStyle = new GUIStyle(EditorStyles.helpBox);
                    infoStyle.alignment = TextAnchor.MiddleCenter;
                }
                return infoStyle;
            }

            /// <summary>
            /// Use it only in GUI calls.
            /// </summary>
            public GUIStyle GetLockStyle()
            {
                if (lockButtonStyle == null)
                {
                    lockButtonStyle = new GUIStyle("IN LockButton");
                }
                return lockButtonStyle;
            }

            /// <summary>
            /// Use it only in GUI calls.
            /// </summary>
            public GUIStyle GetMiniButtonStyle()
            {
                if (miniButton == null)
                {
                    miniButton = new GUIStyle(EditorStyles.miniButton);
                    miniButton.fixedWidth = 0;
                    miniButton.fixedHeight = 0;
                    miniButton.stretchWidth = true;
                    miniButton.stretchHeight = true;
                }
                return miniButton;
            }

            public Texture GetPlayIcon()
            {
                return playIcon;
            }

            public Texture GetPauseIcon()
            {
                return pauseIcon;
            }

            public Texture GetScoreIcon()
            {
                return scoreIcon;
            }
        }

        // Stored required components.
        private EnvironmentQuery environmentQuery;
        private Transform transform;

        // Stored required properties.
        private bool lockQuery;
        private bool lockQuerier;
        private int playIndex;
        private int scoreIndex;
        private Styles styles;

        /// <summary>
        /// OnCreated is invoked when an Overlay is instantiated in an Overlay Canvas.
        /// </summary>
        public override void OnCreated()
        {
            base.OnCreated();

            styles = new Styles();

            LoadPrefs();

            Selection.selectionChanged -= OnSelectionChange;
            Selection.selectionChanged += OnSelectionChange;

            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;
        }

        /// <summary>
        /// Implement this method to return your visual element content.
        /// </summary>
        /// <returns>Visual element containing the content of your overlay.</returns>
        public override VisualElement CreatePanelContent()
        {
            return new IMGUIContainer(OnGUI);
        }

        /// <summary>
        /// Called for rendering and handling GUI events.
        /// </summary>
        private void OnGUI()
        {
            const float LABEL_WIDTH = 50;
            const float FIELD_WIDTH = 150;
            const float TOGGLE_WIDTH = 18;
            const float TOTAL_WIDTH = LABEL_WIDTH + FIELD_WIDTH + TOGGLE_WIDTH;

            //const string INFO_TEXT = "Select Environment Query and object to visualize EQS relative to the selected object.";
            //Rect position = GUILayoutUtility.GetRect(TOTAL_WIDTH, 55);
            //position.height -= EditorGUIUtility.standardVerticalSpacing;
            //GUI.Label(position, INFO_TEXT, styles.GetInfoStyle());

            Rect position = GUILayoutUtility.GetRect(TOTAL_WIDTH, EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing * 2);
            position.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.BeginChangeCheck();
            EditorGUI.PrefixLabel(position, new GUIContent("Query"));
            position.x += LABEL_WIDTH;
            position.width = FIELD_WIDTH;
            environmentQuery = EditorGUI.ObjectField(position, environmentQuery, typeof(EnvironmentQuery), true) as EnvironmentQuery;
            position.x += FIELD_WIDTH + 2;
            position.width = TOTAL_WIDTH;
            lockQuery = EditorGUI.Toggle(position, lockQuery, styles.GetLockStyle());
            position.x = 0;
            position.width = TOTAL_WIDTH;
            position.y = position.yMax + EditorGUIUtility.standardVerticalSpacing;

            EditorGUI.PrefixLabel(position, new GUIContent("Querier"));
            position.x += LABEL_WIDTH;
            position.width = FIELD_WIDTH;
            transform = EditorGUI.ObjectField(position, transform, typeof(Transform), true) as Transform;
            position.x += FIELD_WIDTH + 2;
            position.width = TOTAL_WIDTH;
            lockQuerier = EditorGUI.Toggle(position, lockQuerier, styles.GetLockStyle());
            if (EditorGUI.EndChangeCheck())
            {
                SavePrefs();
            }

            position = GUILayoutUtility.GetRect(TOTAL_WIDTH, 20);
            position.width -= TOTAL_WIDTH / 4;

            EditorGUI.BeginDisabledGroup(environmentQuery == null || transform == null);
            EditorGUI.BeginChangeCheck();
            int prevIndex = playIndex;
            playIndex = GUI.Toolbar(position, playIndex, new[] { playIndex == 0 ? styles.GetPauseIcon() : styles.GetPlayIcon() }, styles.GetMiniButtonStyle());
            if (EditorGUI.EndChangeCheck())
            {
                if (playIndex == prevIndex)
                {
                    playIndex = -1;
                }
                SavePrefs();
            }

            position.x = position.xMax + 2;
            position.width = (TOTAL_WIDTH / 4) - 2;

            EditorGUI.BeginChangeCheck();
            prevIndex = scoreIndex;
            scoreIndex = GUI.Toolbar(position, scoreIndex, new[] { styles.GetScoreIcon() }, styles.GetMiniButtonStyle());
            if (EditorGUI.EndChangeCheck())
            {
                if (scoreIndex == prevIndex)
                {
                    scoreIndex = -1;
                }
                SavePrefs();
            }
            EditorGUI.EndDisabledGroup();
        }

        /// <summary>
        /// Called when an Overlay is about to be destroyed.
        /// </summary>
        public override void OnWillBeDestroyed()
        {
            base.OnWillBeDestroyed();
            Selection.selectionChanged -= OnSelectionChange;
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        /// <summary>
        /// Event to receive a callback whenever the Scene view calls the OnGUI method.
        /// </summary>
        /// <param name="scene">The Scene view invoking this callback.</param>
        private void OnSceneGUI(SceneView scene)
        {
            if (playIndex == 0 && environmentQuery != null && transform != null)
            {
                environmentQuery.SetQuerier(transform);
                environmentQuery.Visualize(true, scoreIndex == 0);
            }
        }

        /// <summary>
        /// Called whenever the selection has changed.
        /// </summary>
        private void OnSelectionChange()
        {
            if (!lockQuery && Selection.activeObject is EnvironmentQuery selection)
            {
                environmentQuery = selection;
                SavePrefs();
            }

            if (!lockQuerier && Selection.activeTransform != null)
            {
                transform = Selection.activeTransform;
                SavePrefs();
            }
        }     

        /// <summary>
        /// Save current prefs in editor session state.
        /// </summary>
        private void SavePrefs()
        {
            if (environmentQuery != null)
            {
                SessionState.SetInt(EQ_GUID, environmentQuery.GetInstanceID());
            }

            if (transform != null)
            {
                SessionState.SetInt(TRANSFORM_GUID, transform.GetInstanceID());
            }

            SessionState.SetBool(EQ_LOCK_GUID, lockQuery);
            SessionState.SetBool(TRANSFORM_LOCK_GUID, lockQuerier);
            SessionState.SetInt(SCORE_GUID, scoreIndex);
            SessionState.SetInt(VISUALIZING_GUID, playIndex);
        }

        /// <summary>
        /// Load editor session state prefs.
        /// </summary>
        private void LoadPrefs()
        {
            int environmentQueryID = SessionState.GetInt(EQ_GUID, 0);
            EnvironmentQuery environmentQuery = EditorUtility.InstanceIDToObject(environmentQueryID) as EnvironmentQuery;
            if (environmentQuery != null)
            {
                this.environmentQuery = environmentQuery;
            }

            int transformID = SessionState.GetInt(TRANSFORM_GUID, 0);
            Transform transform = EditorUtility.InstanceIDToObject(transformID) as Transform;
            if (transform != null)
            {
                this.transform = transform;
            }

            lockQuery = SessionState.GetBool(EQ_LOCK_GUID, false);
            lockQuerier = SessionState.GetBool(TRANSFORM_LOCK_GUID, false);
            scoreIndex = SessionState.GetInt(SCORE_GUID, -1);
            playIndex = SessionState.GetInt(VISUALIZING_GUID, -1);
        }
    }
}