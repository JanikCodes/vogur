/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using System;
using UnityEngine;

namespace RenownedGames.AITree
{
    [Serializable]
    public abstract class FloatOperation : ArithmeticOperation<float> { }

    [Serializable]
    [SearchContent("Add")]
    public class AddFloatOperation : FloatOperation
    {
        public override float Result(float v1, float v2)
        {
            return v1 + v2;
        }
    }

    [Serializable]
    [SearchContent("Sub")]
    public class SubtractFloatOperation : FloatOperation
    {
        public override float Result(float v1, float v2)
        {
            return v1 - v2;
        }
    }

    [Serializable]
    [SearchContent("Mul")]
    public class MultiplyFloatOperation : FloatOperation
    {
        public override float Result(float v1, float v2)
        {
            return v1 * v2;
        }
    }

    [Serializable]
    [SearchContent("Div")]
    public class DivideFloatOperation : FloatOperation
    {
        public override float Result(float v1, float v2)
        {
            return v1 / v2;
        }
    }

    [Serializable]
    [SearchContent("Min")]
    public class MinFloatOperation : FloatOperation
    {
        public override float Result(float v1, float v2)
        {
            return Mathf.Min(v1, v2);
        }
    }

    [Serializable]
    [SearchContent("Max")]
    public class MaxFloatOperation : FloatOperation
    {
        public override float Result(float v1, float v2)
        {
            return Mathf.Max(v1, v2);
        }
    }
}