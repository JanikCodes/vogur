/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using UnityEngine;

namespace RenownedGames.AITree.Nodes
{
    [NodeContent("Set Audio Loop", "Tasks/Audio/Set Audio Loop", IconPath = "Images/Icons/Node/SoundIcon.png")]
    public class SetAudioLoopTask : BaseAudioTask
    {
        [Title("Node")]
        [SerializeField]
        private bool loop;

        protected override void Action(AudioSource audioSource)
        {
            audioSource.loop = loop;
        }
    }
}