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
    [NodeContent("Play Sound", "Tasks/Audio/Play Sound", IconPath = "Images/Icons/Node/SoundIcon.png")]
    public class PlaySoundTask : BaseAudioTask
    {
        [SerializeField]
        private Vector3Key position;

        [Title("Node")]
        [SerializeField]
        //[MinMaxSlider(0f, 1f)]
        private FloatKey volume;

        [SerializeField]
        private AudioClip audioClip;

        protected override void Action(AudioSource audioSource)
        {
            if (position != null)
            {
                AudioSource.PlayClipAtPoint(audioClip, position.GetValue());
            }
            else
            {
                AudioSource.PlayClipAtPoint(audioClip, GetGameObjectPosition());
            }
        }
    }
}