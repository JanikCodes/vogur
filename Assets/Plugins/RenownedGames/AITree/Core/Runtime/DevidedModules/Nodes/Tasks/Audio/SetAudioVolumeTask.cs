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
    [NodeContent("Set Audio Volume", "Tasks/Audio/Set Audio Volume", IconPath = "Images/Icons/Node/SoundIcon.png")]
    public class SetAudioVolumeTask : BaseAudioTask
    {
        [Title("Node")]
        [SerializeField]
        private float volume;

        protected override void Action(AudioSource audioSource)
        {
            audioSource.volume = volume;
        }
    }
}