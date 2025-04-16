/* ================================================================
   ----------------------------------------------------------------
   Project   :   ExLib
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright Renowned Games All rights reserved.
   ================================================================ */

using System;
using UnityEngine;

namespace RenownedGames.ExLib
{
    public abstract class Scope : IDisposable
    {
        private bool disposed;

        /// <summary>
        /// Destructor.
        /// </summary>
        ~Scope()
        {
            if (!disposed)
            {
                Debug.LogError("Scope was not disposed! You should use the 'using' keyword or manually call Dispose.");
            }
        }

        /// <summary>
        /// Implement this method to execute some code when scope disposed.
        /// </summary>
        protected abstract void CloseScope();

        #region [IDisposable Implementation]
        public void Dispose()
        {
            if (!disposed)
            {
                CloseScope();
                disposed = true;
            }
        }
        #endregion
    }
}
