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
    [NodeContent("Play Random Sound", "Tasks/Audio/Play Random Sound", IconPath = "Images/Icons/Node/SoundIcon.png")]
    public class PlayRandomSoundTask : BaseAudioTask
    {
        [System.Serializable]
        private struct ClipElement
        {
            public AudioClip audioClip;

            [Slider(0f, 1f)]
            public float weight;
        }

        [SerializeField]
        private Vector3Key position;

        [Title("Node")]
        [SerializeField]
        private FloatKey volume;

        [SerializeField]
        [Array]
        private ClipElement[] audioClips;

        protected override void Action(AudioSource audioSource)
        {
            AudioClip clip = GetClipRandomly();
            if (clip == null) return;

            if (position != null)
            {
                if (volume != null)
                {
                    AudioSource.PlayClipAtPoint(clip, position.GetValue(), volume.GetValue());
                }
                else
                {
                    AudioSource.PlayClipAtPoint(clip, position.GetValue());
                }
            }
            else
            {
                if (volume != null)
                {
                    audioSource.volume = volume.GetValue();
                }
                audioSource.PlayOneShot(clip);
            }
        }

        private AudioClip GetClipRandomly()
        {
            float totalWeight = 0;
            for (int i = 0; i < audioClips.Length; i++)
            {
                ClipElement element = audioClips[i];
                totalWeight += element.weight;
            }

            float value = Random.Range(0f, totalWeight);
            for (int i = 0; i < audioClips.Length; i++)
            {
                ClipElement element = audioClips[i];
                value -= element.weight;
                if (value <= 0)
                {
                    return element.audioClip;
                }
            }

            return null;
        }
    }
}