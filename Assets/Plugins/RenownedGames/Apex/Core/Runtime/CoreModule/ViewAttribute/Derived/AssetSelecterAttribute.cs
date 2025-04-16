/* ================================================================
   ---------------------------------------------------
   Project   :    Apex
   Publisher :    Renowned Games
   Developer :    Tamerlan Shakirov
   ---------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;
using System.IO;

namespace RenownedGames.Apex
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class AssetSelecterAttribute : ViewAttribute
    {
        public AssetSelecterAttribute()
        {
            AssetType = null;
            Path = "Assets";
            Pattern = "*";
            Option = SearchOption.AllDirectories;
            Sort = true;
        }

        #region [Parameters]
        /// <summary>
        /// Target asset search type.
        /// </summary>
        public Type AssetType { get; set; }
        
        /// <summary>
        /// Search asset path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Search pattern.
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// Search asset option.
        /// </summary>
        public SearchOption Option { get; set; }

        /// <summary>
        /// Sort asset in folders by type.
        /// </summary>
        public bool Sort { get; set; }
        #endregion
    }
}