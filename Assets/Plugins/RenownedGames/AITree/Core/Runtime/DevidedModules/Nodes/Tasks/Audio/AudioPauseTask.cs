/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using UnityEngine;

namespace RenownedGames.AITree.Nodes
{
    [NodeContent("Audio Pause", "Tasks/Audio/Audio Pause", IconPath = "Images/Icons/Node/SoundIcon.png")]
    public class AudioPauseTask : BaseAudioTask
    {
        protected override void Action(AudioSource audioSource)
        {
            audioSource.Pause();
        }
    }
}