/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

namespace RenownedGames.AITree.Nodes
{
    [NodeContent("Parallel Selector", "Composites/Parallels/Parallel Selector", IconPath = "Images/Icons/Node/ParallelIcon.png")]
    public class ParallelSelectorNode : ParallelNode
    {
        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        protected override State OnUpdate()
        {
            bool skipLeftNodes = GetSkipLeftNodes();

            bool stillRunning = false;
            for (int i = 0; i < childrenLeftToExecute.Length; i++)
            {
                Node child = GetChild(i);
                if (!skipLeftNodes || skipLeftNodes && childrenLeftToExecute[i] == State.Running)
                {
                    if (child == null)
                    {
                        AbortRunningChildren();
                        return State.Failure;
                    }

                    State state = child.Update();
                    switch (state)
                    {
                        case State.Success:
                            AbortRunningChildren();
                            return State.Success;
                        case State.Failure:
                            break;
                        case State.Aborted:
                            AbortRunningChildren();
                            return State.Success;
                        case State.Running:
                            stillRunning = true;
                            break;
                    }

                    childrenLeftToExecute[i] = state;
                }
            }

            return stillRunning ? State.Running : State.Failure;
        }
    }
}