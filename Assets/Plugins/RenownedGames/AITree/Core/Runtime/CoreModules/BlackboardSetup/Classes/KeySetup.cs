/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;
using UnityEngine;

namespace RenownedGames.AITree
{
    [Serializable]
    internal struct KeySetup
    {
        [SerializeField]
        [NonLocal]
        [HideSelfKey]
        public Key key;

        [SerializeReference]
        public KeyReceiver keyReceiver;
    }
}