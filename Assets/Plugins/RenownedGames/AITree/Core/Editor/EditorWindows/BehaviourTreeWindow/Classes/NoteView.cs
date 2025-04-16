/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.AITree;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace RenownedGames.AITreeEditor
{
    internal class NoteView : StickyNote
    {
        private Note note;
        private BehaviourTreeGraph graph;

        /// <summary>
        /// Note view constructor.
        /// </summary>
        /// <param name="note">Note reference.</param>
        public NoteView(BehaviourTreeGraph graph, Note note)
        {
            this.graph = graph;
            this.note = note;

            title = note.title;
            contents = note.contents;
            theme = note.theme;
            fontSize = note.fontSize;

            SetPosition(new Rect(note.position, note.size));

            viewDataKey = note.GetInstanceID().ToString();

            RegisterCallback<StickyNoteChangeEvent>(evt => OnNoteChanged(evt.change));
        }

        /// <summary>
        /// Called when the [StickyNote] is about to be resized.
        /// </summary>
        public override void OnResized()
        {
            base.OnResized();
            EditorApplication.delayCall += () => OnNoteChanged(StickyNoteChange.Position);
        }

        /// <summary>
        /// Called when note changed once after all inspectors update.
        /// </summary>
        /// <param name="change">Sticky note change state.</param>
        protected virtual void OnNoteChanged(StickyNoteChange change)
        {
            const string CHANGE_NOTE_TITLE_RECORD_KEY = "[BehaviourTree] Change note title";
            const string CHANGE_NOTE_CONTENTS_RECORD_KEY = "[BehaviourTree] Change note contetns";
            const string CHANGE_NOTE_THEME_RECORD_KEY = "[BehaviourTree] Change note theme";
            const string CHANGE_NOTE_FONT_SIZE_RECORD_KEY = "[BehaviourTree] Change note font size";
            const string CHANGE_NOTE_POSITION_RECORD_KEY = "[BehaviourTree] Change note position";

            switch (change)
            {
                case StickyNoteChange.Title:
                    Undo.RecordObject(note, CHANGE_NOTE_TITLE_RECORD_KEY);
                    note.title = this.title;
                    break;
                case StickyNoteChange.Contents:
                    Undo.RecordObject(note, CHANGE_NOTE_CONTENTS_RECORD_KEY);
                    note.contents = this.contents;
                    break;
                case StickyNoteChange.Theme:
                    Undo.RecordObject(note, CHANGE_NOTE_THEME_RECORD_KEY);
                    note.theme = this.theme;
                    break;
                case StickyNoteChange.FontSize:
                    Undo.RecordObject(note, CHANGE_NOTE_FONT_SIZE_RECORD_KEY);
                    note.fontSize = this.fontSize;
                    break;
                case StickyNoteChange.Position:
                    Undo.RecordObject(note, CHANGE_NOTE_POSITION_RECORD_KEY);
                    note.position.x = layout.x;
                    note.position.y = layout.y;
                    note.size.x = layout.width;
                    note.size.y = layout.height;
                    break;
            }

            EditorUtility.SetDirty(note);
        }

        #region [Getter / Setter]
        public Note GetNote()
        {
            return note;
        }

        public BehaviourTreeGraph GetGraph()
        {
            return graph;
        }
        #endregion
    }
}