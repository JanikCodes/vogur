/* ================================================================
   ----------------------------------------------------------------
   Project   :   ExLib
   Company   :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright Renowned Games All rights reserved.
   ================================================================ */

using UnityEngine;

namespace RenownedGames.ExLib.Patterns
{
    /// <summary>
    /// It's a generating pattern that guarantees the existence of only one object of a certain class, 
    /// and also allows you to reach this object from anywhere in the program.
    /// 
    /// Be aware this will not prevent a non singleton constructor
    ///   such as `T myT = new T();`
    /// To prevent that, add `protected T () {}` to your singleton class.
    /// 
    /// As a note, this is made as MonoBehaviour because we need Coroutines.
    /// </summary>
    /// <typeparam name="T">Certain class that implemented of MonoBehaviour.</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        private static object lockInstance = new object();
        private static bool applicationIsQuitting = false;

        /// <summary>
        /// Runtime instance reference of T.
        /// </summary>
        public static T GetRuntimeInstance()
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' already destroyed on application quit." +
                    " Won't create again - returning null.");
                return null;
            }

            lock (lockInstance)
            {
                if (instance == null)
                {
                    T[] instances = FindObjectsOfType<T>();

                    if (instances != null && instances.Length > 0)
                    {
                        instance = instances[0];

                        if (instances.Length > 1)
                        {
                            Debug.LogError("[Singleton] Something went really wrong " +
                                " - there should never be more than 1 singleton!" +
                                " Reopening the scene might fix it.");
                            return instance;
                        }
                    }

                    if (instance == null)
                    {
                        GameObject singleton = new GameObject();
                        instance = singleton.AddComponent<T>();
                        singleton.name = string.Format("[Singleton] - {0}", typeof(T).Name);
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// 
        /// When Unity quits, it destroys objects in a random order.
        /// In principle, a Singleton is only destroyed when application quits.
        /// If any script calls Instance after it have been destroyed, 
        ///   it will create a buggy ghost object that will stay on the Editor scene
        ///   even after stopping playing the Application. Really bad!
        /// So, this was made to be sure we're not creating that buggy ghost object.
        /// </summary>
        protected virtual void OnApplicationQuit()
        {
            applicationIsQuitting = true;
        }
    }
}