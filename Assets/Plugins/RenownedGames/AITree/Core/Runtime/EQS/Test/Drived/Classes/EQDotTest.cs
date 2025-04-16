/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using System;
using UnityEngine;

namespace RenownedGames.AITree.EQS
{
    [Serializable]
    [SearchContent("Dot", Image = "Images/Icons/EQS/Tests/DotTestIcon.png")]
    public class EQDotTest : EQTest
    {
        [Serializable]
        private class Line
        {
            private enum Mode
            {
                Rotation,
                TwoPoints
            }

            [SerializeField]
            private Mode mode = Mode.Rotation;

            [SerializeField]
            [ShowIf("mode", Mode.Rotation)]
            private TargetType rotation = TargetType.Querier;

            [SerializeField]
            [Label("Transform Key")]
            [ShowIf("KeyNameShowIf")]
            private string transformKeyName;

            [SerializeField]
            [ShowIf("mode", Mode.TwoPoints)]
            private TargetType lineFrom = TargetType.Querier;

            [SerializeField]
            [Label("Key")]
            [ShowIf("FromKeyNameShowIf")]
            private string fromKeyName;

            [SerializeField]
            [ShowIf("mode", Mode.TwoPoints)]
            private TargetType lineTo = TargetType.Item;

            [SerializeField]
            [Label("Key")]
            [ShowIf("ToKeyNameShowIf")]
            private string toKeyName;

            public Vector3 GetDirection()
            {
                if (mode == Mode.Rotation)
                {
                    if (rotation == TargetType.Querier)
                    {
                        return instance.GetQuerier().forward;
                    }
                    else if (rotation == TargetType.Key)
                    {
                        return instance.GetForwardByKey(transformKeyName);
                    }
                    else
                    {
                        return Vector3.zero;
                    }
                }
                else
                {
                    return (instance.GetPositionByTarget(lineTo, toKeyName) - instance.GetPositionByTarget(lineFrom, fromKeyName)).normalized;
                }
            }

            #region [ShowIf]
            private bool KeyNameShowIf()
            {
                return mode == Mode.Rotation && rotation == TargetType.Key;
            }

            private bool FromKeyNameShowIf()
            {
                return mode == Mode.TwoPoints && lineFrom == TargetType.Key;
            }

            private bool ToKeyNameShowIf()
            {
                return mode == Mode.TwoPoints && lineTo == TargetType.Key;
            }
            #endregion
        }

        private static EQDotTest instance;

        [Title("Dot")]
        [SerializeField]
        private Line lineA;

        [SerializeField]
        private Line lineB;

        protected override float CalculateWeight(EQItem item)
        {
            instance = this;
            return Vector3.Dot(lineA.GetDirection(), lineB.GetDirection());

            //Vector3 direction = (item.GetPosition() - GetQuerier().position).normalized;
            //return Vector3.Dot(GetQuerier().forward, direction);
        }
    }
}