/* ================================================================
   ----------------------------------------------------------------
   Project   :   Apex
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;

namespace RenownedGames.ApexEditor
{
    public abstract class MemberManipulator : IManipulatorInitializaton
    {
        /// <summary>
        /// Called once when initializing member manipulator.
        /// </summary>
        /// <param name="member">Serialized member with ManipulatorAttribute.</param>
        /// <param name="ManipulatorAttribute">ManipulatorAttribute of serialized member.</param>
        public virtual void Initialize(SerializedMember member, ManipulatorAttribute ManipulatorAttribute) { }

        /// <summary>
        /// Called before rendering member GUI.
        /// </summary>
        public virtual void OnBeforeGUI() { }

        /// <summary>
        /// Called after rendering member GUI.
        /// </summary>
        public virtual void OnAfterGUI() { }
    }
}