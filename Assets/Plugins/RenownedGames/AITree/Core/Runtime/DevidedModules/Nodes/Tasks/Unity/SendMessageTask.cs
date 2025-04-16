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
    [NodeContent("Send Message", "Tasks/Unity/Send Message", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
    public class SendMessageTask : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        [NonLocal]
        private TransformKey receiver;

        [SerializeField]
        private string methodName;

        [SerializeField]
        private Key valueKey;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (receiver == null)
            {
                return State.Failure;
            }

            Transform receiverTransform = receiver.GetValue();
            if (receiverTransform == null) return State.Failure;

            if (valueKey == null)
            {
                receiverTransform.SendMessage(methodName);
                return State.Success;
            }

            object value = valueKey.GetValueObject();
            if (value == null) return State.Failure;

            receiverTransform.SendMessage(methodName, value);
            return State.Success;
        }
    }
}