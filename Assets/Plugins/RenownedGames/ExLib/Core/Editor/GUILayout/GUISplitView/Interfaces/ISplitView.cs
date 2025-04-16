/* ================================================================
   ----------------------------------------------------------------
   Project   :   ExLib
   Company   :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright Renowned Games All rights reserved.
   ================================================================ */

namespace RenownedGames.ExLibEditor
{
    public interface ISplitView
    {
        void BeginSplitView();

        void Split();

        void EndSplitView();
    }
}