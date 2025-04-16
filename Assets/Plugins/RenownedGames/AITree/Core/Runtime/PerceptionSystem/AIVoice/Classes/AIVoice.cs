/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using UnityEngine;

namespace RenownedGames.AITree.PerceptionSystem
{
    [HideMonoScript]
    [AddComponentMenu("Renowned Games/AI Tree/Perception System/AI Voice")]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(AIPerception))]
    public class AIVoice : MonoBehaviour
    {
        [SerializeField]
        private float minRefreshTime = 3f;

        [SerializeField]
        [Foldout("Clips", Style = "Group")]
        [Array]
        private AudioClip[] detectionClips;

        [SerializeField]
        [Foldout("Clips", Style = "Group")]
        [Array]
        private AudioClip[] lossClips;

        [SerializeField]
        [Foldout("Clips", Style = "Group")]
        [Array]
        private AudioClip[] seemedClips;

        // Stored required components.
        private AudioSource audioSource;
        private AIPerception aiPerception;
        private AIDetectionSignal aiDetectionSignal;

        // Stored required properties.
        private float lastPlayTime;
        private int detectionIndex;
        private int lossIndex;
        private int seemedIndex;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            aiPerception = GetComponent<AIPerception>();
            aiDetectionSignal = GetComponent<AIDetectionSignal>();

            if (aiDetectionSignal != null)
            {
                aiDetectionSignal.OnDetectCompletely += OnDetectCompletely;
                aiDetectionSignal.OnLostCompletely += OnLostCompletely;
                aiDetectionSignal.OnLostSightOf += OnLostSightOf;
            }
            else if (aiPerception != null)
            {
                aiPerception.OnSourceDetect += OnSourceDetection;
                aiPerception.OnSourceLoss += OnSourceLoss;
            }
        }

        private void OnDestroy()
        {
            if (aiDetectionSignal != null)
            {
                aiDetectionSignal.OnDetectCompletely -= OnDetectCompletely;
                aiDetectionSignal.OnLostCompletely -= OnLostCompletely;
                aiDetectionSignal.OnLostSightOf -= OnLostSightOf;
            }
            else if (aiPerception != null)
            {
                aiPerception.OnSourceDetect -= OnSourceDetection;
                aiPerception.OnSourceLoss -= OnSourceLoss;
            }
        }

        private void OnDetectCompletely()
        {
            if (Time.time - lastPlayTime > minRefreshTime)
            {
                lastPlayTime = Time.time;
                detectionIndex = UniqueRandom(detectionIndex, detectionClips.Length);
                audioSource.PlayOneShot(detectionClips[detectionIndex]);
            }
        }

        private void OnLostCompletely()
        {
            if (Time.time - lastPlayTime > minRefreshTime)
            {
                lastPlayTime = Time.time;
                lossIndex = UniqueRandom(lossIndex, lossClips.Length);
                audioSource.PlayOneShot(lossClips[lossIndex]);
            }
        }

        private void OnLostSightOf()
        {
            if (Time.time - lastPlayTime > minRefreshTime)
            {
                lastPlayTime = Time.time;
                seemedIndex = UniqueRandom(seemedIndex, seemedClips.Length);
                audioSource.PlayOneShot(seemedClips[seemedIndex]);
            }
        }

        private void OnSourceDetection(AIPerceptionSource source)
        {
            OnDetectCompletely();
        }

        private void OnSourceLoss(AIPerceptionSource source)
        {
            OnLostCompletely();
        }

        private int UniqueRandom(int lastIndex, int length)
        {
            if (length < 2)
            {
                return 0;
            }
            else
            {
                int index;
                do
                {
                    index = Random.Range(0, length);
                }
                while (index == lastIndex);
                return index;
            }
        }
    }
}