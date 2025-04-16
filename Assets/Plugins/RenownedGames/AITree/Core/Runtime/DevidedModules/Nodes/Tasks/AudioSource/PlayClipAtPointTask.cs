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
    [System.Obsolete]
    [NodeContent("Play Clip At Point", "Tasks/Audio Source/Play Clip At Point", IconPath = "Images/Icons/Node/SoundIcon.png", Hide = true)]
    public class PlayClipAtPointTask : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        private AudioClip clip;

        [SerializeField]
        private Vector3Key position;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns></returns>
        protected override State OnUpdate()
        {
            if (clip == null)
            {
                return State.Failure;
            }

            Vector3 _position = Vector3.zero;
            if (position != null)
            {
                _position = position.GetValue();
            }

            AudioSource.PlayClipAtPoint(clip, _position);
            return State.Success;
        }

        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            string description = $"Play Clip At Point: ";

            if (clip != null)
            {
                description += clip.name;
            }
            else
            {
                description += "null";
            }

            if (position != null)
            {
                description += $"\nPosition: {position.ToString()}";
            }

            return description;
        }
    }
}