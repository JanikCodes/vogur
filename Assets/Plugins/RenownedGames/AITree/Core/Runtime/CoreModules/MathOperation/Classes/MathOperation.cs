/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;

namespace RenownedGames.AITree
{
    [Serializable]
    public abstract class MathOperation<TResult, TValue>
    {
        public abstract TResult Result(TValue lhs, TValue rhs);
    }

    [Serializable]
    public abstract class ArithmeticOperation<TValue> : MathOperation<TValue, TValue> { }

    [Serializable]
    public abstract class ComparisonOperation<TValue> : MathOperation<bool, TValue> { }
}