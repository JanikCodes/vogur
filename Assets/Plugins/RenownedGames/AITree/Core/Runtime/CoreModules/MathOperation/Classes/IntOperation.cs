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
    public abstract class IntOperation : ArithmeticOperation<int> { }

    [Serializable]
    [SearchContent("Add")]
    public class AddIntOperation : IntOperation
    {
        public override int Result(int v1, int v2)
        {
            return v1 + v2;
        }
    }

    [Serializable]
    [SearchContent("Sub")]
    public class SubtractIntOperation : IntOperation
    {
        public override int Result(int v1, int v2)
        {
            return v1 - v2;
        }
    }

    [Serializable]
    [SearchContent("Mul")]
    public class MultiplyIntOperation : IntOperation
    {
        public override int Result(int v1, int v2)
        {
            return v1 * v2;
        }
    }

    [Serializable]
    [SearchContent("Div")]
    public class DivideIntOperation : IntOperation
    {
        public override int Result(int v1, int v2)
        {
            return v1 / v2;
        }
    }

    [Serializable]
    [SearchContent("Min")]
    public class MinIntOperation : IntOperation
    {
        public override int Result(int v1, int v2)
        {
            return Mathf.Min(v1, v2);
        }
    }

    [Serializable]
    [SearchContent("Max")]
    public class MaxIntOperation : IntOperation
    {
        public override int Result(int v1, int v2)
        {
            return Mathf.Max(v1, v2);
        }
    }
}