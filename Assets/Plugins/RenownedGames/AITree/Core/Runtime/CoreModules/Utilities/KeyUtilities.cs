/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;
using UnityEngine;

namespace RenownedGames.AITree
{
    public static class KeyUtilities
    {
        /// <summary>
        /// Try cast undefined generic value to specified type.
        /// </summary>
        /// <typeparam name="T">Desired type.</typeparam>
        /// <param name="value">Output reference of value.</param>
        /// <returns>True if value has been casted. Otherwise false.</returns>
        public static bool TryCastValueTo<T>(this Key key, out T value)
        {
            if (key.GetValueObject() is T _value)
            {
                value = _value;
                return true;
            }
            value = default(T);
            return false;
        }

        /// <summary>
        /// Try get int value of undefined key.
        /// </summary>
        /// <param name="value">Output int value.</param>
        /// <returns>True if value has been casted. Otherwise false.</returns>
        public static bool TryGetInt(this Key key, out int value)
        {
            if (key is IntKey intKey)
            {
                value = intKey.GetValue();
                return true;
            }
            value = default(int);
            return false;
        }

        /// <summary>
        /// Try get float value of undefined key.
        /// </summary>
        /// <param name="value">Output float value.</param>
        /// <returns>True if value has been casted. Otherwise false.</returns>
        public static bool TryGetFloat(this Key key, out float value)
        {
            if (key is FloatKey floatKey)
            {
                value = floatKey.GetValue();
                return true;
            }
            value = default(float);
            return false;
        }

        /// <summary>
        /// Try get short (Int16) value of undefined key.
        /// </summary>
        /// <param name="value">Output short value.</param>
        /// <returns>True if value has been casted. Otherwise false.</returns>
        public static bool TryGetShort(this Key key, out short value)
        {
            if (key is ShortKey shortKey)
            {
                value = shortKey.GetValue();
                return true;
            }
            value = default(short);
            return false;
        }

        /// <summary>
        /// Try get long (Int64) value of undefined key.
        /// </summary>
        /// <param name="value">Output long value.</param>
        /// <returns>True if value has been casted. Otherwise false.</returns>
        public static bool TryGetLong(this Key key, out long value)
        {
            if (key is LongKey longKey)
            {
                value = longKey.GetValue();
                return true;
            }
            value = default(long);
            return false;
        }

        /// <summary>
        /// Try get bool value of undefined key.
        /// </summary>
        /// <param name="value">Output bool value.</param>
        /// <returns>True if value has been casted. Otherwise false.</returns>
        public static bool TryGetBool(this Key key, out bool value)
        {
            if (key is BoolKey boolKey)
            {
                value = boolKey.GetValue();
                return true;
            }
            value = default(bool);
            return false;
        }

        /// <summary>
        /// Try get string value of undefined key.
        /// </summary>
        /// <param name="value">Output string value.</param>
        /// <returns>True if value has been casted. Otherwise false.</returns>
        public static bool TryGetString(this Key key, out string value)
        {
            if (key is StringKey stringKey)
            {
                value = stringKey.GetValue();
                return true;
            }
            value = default(string);
            return false;
        }

        /// <summary>
        /// Try get vector2 value of undefined key.
        /// </summary>
        /// <param name="value">Output Vector2 value.</param>
        /// <returns>True if value has been casted. Otherwise false.</returns>
        public static bool TryGetVector2(this Key key, out Vector2 value)
        {
            if (key is Vector2Key vector2Key)
            {
                value = vector2Key.GetValue();
                return true;
            }
            value = default(Vector2);
            return false;
        }

        /// <summary>
        /// Try get vector3 value of undefined key.
        /// </summary>
        /// <param name="value">Output Vector3 value.</param>
        /// <returns>True if value has been casted. Otherwise false.</returns>
        public static bool TryGetVector3(this Key key, out Vector3 value)
        {
            if (key is Vector3Key vector3Key)
            {
                value = vector3Key.GetValue();
                return true;
            }
            value = default(Vector3);
            return false;
        }

        /// <summary>
        /// Try get quaternion value of undefined key.
        /// </summary>
        /// <param name="value">Output Quaternion value.</param>
        /// <returns>True if value has been casted. Otherwise false.</returns>
        public static bool TryGetQuaternion(this Key key, out Quaternion value)
        {
            if (key is QuaternionKey quaternionKey)
            {
                value = quaternionKey.GetValue();
                return true;
            }
            value = default(Quaternion);
            return false;
        }

        /// <summary>
        /// Try get transform value of undefined key.
        /// </summary>
        /// <param name="value">Output Transform value.</param>
        /// <returns>True if value has been casted. Otherwise false.</returns>
        public static bool TryGetTransform(this Key key, out Transform value)
        {
            if (key is TransformKey transformKey)
            {
                value = transformKey.GetValue();
                return true;
            }
            value = default(Transform);
            return false;
        }

        /// <summary>
        /// Try get position (transform, vector2 or vector3) value of undefined key.
        /// </summary>
        /// <param name="space"> The coordinate space in which to operate (transform only).</param>
        /// <param name="value">Output vector3 value.</param>
        /// <returns>True if value has been casted. Otherwise false.</returns>
        public static bool TryGetPosition(this Key key, Space space, out Vector3 value)
        {
            if (key is TransformKey transformKey)
            {
                if(transformKey.GetValue() != null)
                {
                    if (space == Space.World)
                    {
                        value = transformKey.GetValue().position;
                    }
                    else
                    {
                        value = transformKey.GetValue().localPosition;
                    }
                    return true;
                }
            }
            else if(key is Vector3Key vector3Key)
            {
                value = vector3Key.GetValue();
                return true;
            }
            else if (key is Vector2Key vector2Key)
            {
                value = vector2Key.GetValue();
                return true;
            }
            value = default(Vector3);
            return false;
        }

        /// <summary>
        /// Try get position (transform or vector2) value of undefined key.
        /// </summary>
        /// <param name="space"> The coordinate space in which to operate (transform only).</param>
        /// <param name="value">Output vector2 value.</param>
        /// <returns>True if value has been casted. Otherwise false.</returns>
        public static bool TryGetPosition2D(this Key key, Space space, out Vector2 value)
        {
            if (key is TransformKey transformKey)
            {
                if (transformKey.GetValue() != null)
                {
                    if (space == Space.World)
                    {
                        value = transformKey.GetValue().position;
                    }
                    else
                    {
                        value = transformKey.GetValue().localPosition;
                    }
                    return true;
                }
            }
            else if (key is Vector2Key vector2Key)
            {
                value = vector2Key.GetValue();
                return true;
            }
            value = default(Vector2);
            return false;
        }

        /// <summary>
        /// Try get position (transform or vector3) value of undefined key.
        /// </summary>
        /// <param name="space"> The coordinate space in which to operate (transform only).</param>
        /// <param name="value">Output vector3 value.</param>
        /// <returns>True if value has been casted. Otherwise false.</returns>
        public static bool TryGetPosition3D(this Key key, Space space, out Vector3 value)
        {
            if (key is TransformKey transformKey)
            {
                if (transformKey.GetValue() != null)
                {
                    if (space == Space.World)
                    {
                        value = transformKey.GetValue().position;
                    }
                    else
                    {
                        value = transformKey.GetValue().localPosition;
                    }
                    return true;
                }
            }
            else if (key is Vector3Key vector3Key)
            {
                value = vector3Key.GetValue();
                return true;
            }
            value = default(Vector3);
            return false;
        }

        /// <summary>
        /// Try get position (transform world coordinate, vector2 or vector3) value of undefined key.
        /// </summary>
        /// <param name="value">Output vector3 value.</param>
        /// <returns>True if value has been casted. Otherwise false.</returns>
        public static bool TryGetPosition(this Key key, out Vector3 value)
        {
            return key.TryGetPosition(Space.World, out value);
        }

        /// <summary>
        /// Try get position (transform world coordinate or vector2) value of undefined key.
        /// </summary>
        /// <param name="value">Output vector2 value.</param>
        /// <returns>True if value has been casted. Otherwise false.</returns>
        public static bool TryGetPosition2D(this Key key, out Vector2 value)
        {
            return key.TryGetPosition2D(Space.World, out value);
        }

        /// <summary>
        /// Try get position (transform world coordinate or vector3) value of undefined key.
        /// </summary>
        /// <param name="value">Output vector3 value.</param>
        /// <returns>True if value has been casted. Otherwise false.</returns>
        public static bool TryGetPosition3D(this Key key, out Vector3 value)
        {
            return key.TryGetPosition3D(Space.World, out value);
        }
    }
}
