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
    [NodeContent("Set Audio Clip", "Tasks/Audio/Set Audio Clip", IconPath = "Images/Icons/Node/SoundIcon.png")]
    public class SetAudioClipTask : BaseAudioTask
    {
        [Title("Node")]
        [SerializeField]
        private AudioClip audioClip;

        protected override void Action(AudioSource audioSource)
        {
            audioSource.clip = audioClip;
        }
    }
}