/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using UnityEngine;

namespace RenownedGames.AITree.Demo
{
    [AddComponentMenu("Renowned Games/AI Tree/Demo/Animation Audio Event")]
    [RequireComponent(typeof(AudioSource))]
    public sealed class AnimationAudioEvent : MonoBehaviour
    {
        [SerializeField]
        private AudioClip[] onFootstepClips;

        [SerializeField]
        private AudioClip onLandClip;

        // Stored required components.
        private AudioSource audioSource;

        /// <summary>
        /// Called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Play footstep sound.
        /// </summary>
        public void OnFootstep()
        {
            AudioClip clip = onFootstepClips[Random.Range(0, onFootstepClips.Length)];
            audioSource.PlayOneShot(clip);
        }

        /// <summary>
        /// Play land sound.
        /// </summary>
        public void OnLand()
        {
            audioSource.PlayOneShot(onLandClip);
        }
    }
}