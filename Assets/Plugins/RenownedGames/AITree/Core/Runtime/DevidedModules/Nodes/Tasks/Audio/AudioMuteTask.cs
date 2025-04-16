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
    [NodeContent("Audio Mute", "Tasks/Audio/Audio Mute", IconPath = "Images/Icons/Node/SoundIcon.png")]
    public class AudioMuteTask : BaseAudioTask
    {
        [Title("Node")]
        [SerializeField]
        private bool mute;

        protected override void Action(AudioSource audioSource)
        {
            audioSource.mute = mute;
        }
    }
}