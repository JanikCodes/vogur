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
    [NodeContent("Audio Stop", "Tasks/Audio/Audio Stop", IconPath = "Images/Icons/Node/SoundIcon.png")]
    public class AudioStopTask : BaseAudioTask
    {
        protected override void Action(AudioSource audioSource)
        {
            audioSource.Stop();
        }
    }
}