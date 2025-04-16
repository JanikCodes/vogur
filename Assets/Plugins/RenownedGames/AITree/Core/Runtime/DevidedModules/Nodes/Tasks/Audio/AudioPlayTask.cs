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
    [NodeContent("Audio Play", "Tasks/Audio/Audio Play", IconPath = "Images/Icons/Node/SoundIcon.png")]
    public class AudioPlayTask : BaseAudioTask
    {
        [Title("Node")]
        [SerializeField]
        //[MinMaxSlider(0f, 1f)]
        private FloatKey volume;

        [SerializeField]
        private AudioClip oneShotClip;

        protected override void Action(AudioSource audioSource)
        {
            if (volume != null)
            {
                audioSource.volume = volume.GetValue();
            }

            if (oneShotClip != null)
            {
                audioSource.PlayOneShot(oneShotClip);
            }
            else
            {
                audioSource.Play();
            }
        }
    }
}