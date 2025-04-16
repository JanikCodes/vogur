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
    [NodeContent("Debug Log", "Tasks/Debug/Debug Log", IconPath = "Images/Icons/Node/DebugIcon.png")]
    public class DebugLogTask : TaskNode
    {
        private enum LogLevel
        {
            Info,
            Warning,
            Error
        }

        [Title("Node")]
        [SerializeField]
        private LogLevel logLevel;

        [SerializeField]
        private Key message;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (message == null)
            {
                return State.Failure;
            }

            switch (logLevel)
            {
                case LogLevel.Info:
                    Debug.Log(message.GetValueObject());
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning(message.GetValueObject());
                    break;
                case LogLevel.Error:
                    Debug.LogError(message.GetValueObject());
                    break;
            }

            return State.Success;
        }
    }
}