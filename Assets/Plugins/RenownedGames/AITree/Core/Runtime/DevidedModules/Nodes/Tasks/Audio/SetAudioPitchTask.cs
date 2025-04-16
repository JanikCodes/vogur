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
    [NodeContent("Set Audio Pitch", "Tasks/Audio/Set Audio Pitch", IconPath = "Images/Icons/Node/SoundIcon.png")]
    public class SetAudioPitchTask : BaseAudioTask
    {
        [Title("Node")]
        [SerializeField]
        private float pitch;

        protected override void Action(AudioSource audioSource)
        {
            audioSource.pitch = pitch;
        }
    }
}