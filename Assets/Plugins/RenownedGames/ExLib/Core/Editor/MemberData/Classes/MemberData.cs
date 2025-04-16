/* ================================================================
   ----------------------------------------------------------------
   Project   :   ExLib
   Company   :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.ExLib.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;

namespace RenownedGames.ExLibEditor
{
    [Obsolete]
    public sealed class MemberData
    {
        private object declaringObject;
        private Type type;
        private MemberInfo memberInfo;

        internal MemberData(SerializedObject serializedObject, string memberPath)
        {
            declaringObject = serializedObject.targetObject;
            type = declaringObject.GetType();

            string[] pathSplit = memberPath.Split('.');
            Queue<string> paths = new Queue<string>(pathSplit);

            RecursiveSearch(ref paths);
        }

        /// <summary>
        /// Object that declares the current nested member.
        /// </summary>
        public object GetDeclaringObject()
        {
            return declaringObject;
        }

        /// <summary>
        /// Type of member.
        /// </summary>
        public Type GetMemberType()
        {
            return type;
        }

        /// <summary>
        /// Obtains information about the attributes of a member and provides access to member metadata.
        /// </summary>
        public MemberInfo GetMemberInfo()
        {
            return memberInfo;
        }

        public override string ToString()
        {
            return $"Name: [{memberInfo.Name}], Declaring Object: [{declaringObject}], Type: [{type}]";
        }

        private void RecursiveSearch(ref Queue<string> paths)
        {
            string memberName = paths.Dequeue();
            if (memberName.Contains("data["))
            {
                string result = Regex.Match(memberName, @"\d+").Value;
                int index = Convert.ToInt32(result);

                int count = 0;
                if (memberInfo is FieldInfo fieldInfo)
                {
                    object mainObject = fieldInfo.GetValue(declaringObject);
                    IEnumerable enumerable = mainObject as IEnumerable;
                    foreach (object element in enumerable)
                    {
                        if (index == count)
                        {
                            if (element != null)
                            {
                                type = element.GetType();
                            }
                            else if (mainObject.GetType().IsGenericType)
                            {
                                type = mainObject.GetType().GetGenericArguments()[0];
                            }
                            else
                            {
                                type = mainObject.GetType().GetElementType();
                            }

                            declaringObject = element;
                            if (paths.Count > 0)
                            {
                                RecursiveSearch(ref paths);
                                break;
                            }
                        }
                        count++;
                    }
                }

            }


            MemberInfo member = null;
            foreach (MemberInfo item in type.AllMembers())
            {
                if (item.Name == memberName)
                {
                    member = item;
                    break;
                }
            }

            if (member != null)
            {
                memberInfo = member;

                if (memberInfo is FieldInfo fieldInfo)
                {
                    type = fieldInfo.FieldType;
                    if (paths.Count > 0 && ((!type.IsGenericType && !type.IsArray) || (type.IsGenericType && type.GetInterface("IEnumerable`1") == null)))
                    {
                        declaringObject = fieldInfo.GetValue(declaringObject);
                        type = declaringObject.GetType();
                    }
                }
                else if (memberInfo is MethodInfo methodInfo)
                {
                    type = methodInfo.ReturnType;
                }
            }

            if (paths.Count > 0)
            {
                RecursiveSearch(ref paths);
            }
        }

    }
}
