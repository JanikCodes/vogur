/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

namespace RenownedGames.AITree.Nodes
{
    [NodeContent("Sequencer", "Composites/Sequencer", IconPath = "Images/Icons/Node/SequenceIcon.png")]
    public class SequencerNode : CompositeNode
    {
        /// <summary>
        /// Called when behaviour tree enter in node.
        /// </summary>
        protected override void OnEntry()
        {
            base.OnEntry();
            SetCurrent(0);
        }

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            for (int i = GetCurrent(); i < GetChildCount(); i++)
            {
                int current = i;
                SetCurrent(current);
                Node child = GetChild(current);

                if (child == null)
                {
                    return State.Failure;
                }

                switch (child.Update())
                {
                    case State.Running:
                        return State.Running;
                    case State.Success:
#if UNITY_EDITOR
                        if (child.Breakpoint())
                        {
                            SetCurrent(++current);
                            return State.Running;
                        }
                        else
                        {
                            continue;
                        }
#else
                        continue;
#endif
                    case State.Aborted:
                        return State.Aborted;
                    case State.Failure:
                        return State.Failure;
                }
            }

            return State.Success;
        }
    }
}