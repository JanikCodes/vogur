/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System.Collections.Generic;

namespace RenownedGames.AITree
{
    internal struct CloneData
    {
        public Dictionary<Node, Node> cloneNodeMap;
        public Dictionary<Group, Group> cloneGroupMap;
        public Dictionary<Note, Note> cloneNoteMap;

        public CloneData(Dictionary<Node, Node> cloneNodeMap, Dictionary<Group, Group> cloneGroupMap, Dictionary<Note, Note> cloneNoteMap)
        {
            this.cloneNodeMap = cloneNodeMap;
            this.cloneGroupMap = cloneGroupMap;
            this.cloneNoteMap = cloneNoteMap;
        }
    }
}