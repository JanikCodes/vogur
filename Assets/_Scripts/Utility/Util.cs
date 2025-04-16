using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Util
{
    private static readonly System.Random random = new();

    /// <summary>
    /// Converts a string representation of an enum value to its corresponding enum value.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="enumValue">A string representing an enum value.</param>
    /// <returns>The enum value that matches the input string, or the default value for the enum if not found.</returns>
    public static T ReturnEnumValueFromStringValue<T>(string enumValue)
    {
        // enumValue is empty/null
        if (enumValue == null) { return default(T); }

        T[] test = (T[])Enum.GetValues(typeof(T));

        for (int x = 0; x < test.Length; x++)
        {
            if (test[x].ToString().Equals(enumValue))
            {
                return test[x];
            }
        }

        return default(T);
    }

    /// <summary>
    /// Returns an array of enum values from the given array of string values.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="enumValues">Array of string values to convert to enum values.</param>
    /// <returns>An array of enum values.</returns>
    public static T[] ReturnEnumValuesFromStringValues<T>(string[] enumValues)
    {
        // enumValues is empty/null
        if (enumValues == null) { return default(T[]); }

        T[] result = new T[enumValues.Length];

        for (int i = 0; i < enumValues.Length; i++)
        {
            result[i] = ReturnEnumValueFromStringValue<T>(enumValues[i]);
        }

        return result;
    }

    public static Vector3 GetRandomPositionInRadius(Vector3 basePosition, float radius, float minRadius = 0f)
    {
        float effectiveRadius = UnityEngine.Mathf.Max(radius, minRadius);

        // Generate random angles in radians
        float randomAngle = UnityEngine.Random.Range(0f, UnityEngine.Mathf.PI * 2f);

        // Calculate random distance within the effective radius
        float randomDistance = UnityEngine.Random.Range(minRadius, effectiveRadius);

        // Calculate random X and Z offsets based on the random angle and distance
        float offsetX = UnityEngine.Mathf.Cos(randomAngle) * randomDistance;
        float offsetZ = UnityEngine.Mathf.Sin(randomAngle) * randomDistance;

        // Calculate the random position based on the offsets and the basePosition's Y component
        return new Vector3(basePosition.x + offsetX, basePosition.y, basePosition.z + offsetZ);
    }

    /// <summary>
    /// Returns a random integer from given min and max value.
    /// </summary>
    /// <returns>A random integer.</returns>
    public static int GetRandomValue(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    /// <summary>
    /// Returns a random element from the given array.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The array from which to select a random element.</param>
    /// <returns>A randomly selected element from the array.</returns>
    public static T GetRandomValue<T>(T[] array)
    {
        int randomIndex = random.Next(array.Length);
        return array[randomIndex];
    }

    /// <summary>
    /// Returns a random element from the given list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="array">The list from which to select a random element.</param>
    /// <returns>A randomly selected element from the list.</returns>
    public static T GetRandomValue<T>(List<T> list)
    {
        int randomIndex = random.Next(list.Count);
        return list[randomIndex];
    }

    /// <summary>
    /// Returns a list of random elements from the given list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list from which to select random elements.</param>
    /// <param name="count">The number of random elements to return.</param>
    /// <returns>A list of randomly selected elements from the list.</returns>
    public static List<T> GetRandomValues<T>(List<T> list, int count)
    {
        List<T> randomValues = new();
        for (int i = 0; i < count; i++)
        {
            int randomIndex = random.Next(list.Count);
            randomValues.Add(list[randomIndex]);
        }
        return randomValues;
    }

    /// <summary>
    /// Calculates the position including calculated height of terrain of a given position. 
    /// </summary>
    /// <param name="position">The position to be calculated from</param>
    /// <returns>A newly calculated position taking terrain height into account.</returns>
    public static Vector3 CalculatePositionOnTerrain(Vector3 position)
    {
        float terrainHeightPosition = Terrain.activeTerrain.SampleHeight(position);
        return new Vector3(position.x, terrainHeightPosition, position.z);
    }

    /// <summary>
    /// Returns a float array based on a given position
    /// </summary>
    /// <param name="input">Array of the three inputs representing the position (x,y,z)</param>
    public static float[] GetFloatArray(Vector3 position)
    {
        return new float[] { position.x, position.y, position.z };
    }

    /// <summary>
    /// Returns a float array based on a given quaternion
    /// </summary>
    /// <param name="input">Array of the three inputs representing the quaternion (x,y,z)</param>
    public static float[] GetFloatArray(Quaternion rotation)
    {
        return new float[] { rotation.x, rotation.y, rotation.z };
    }

    /// <summary>
    /// Returns a vector3 based on a given float[]
    /// </summary>
    /// <param name="position">Float Array</param>
    public static Vector3 GetVector3FromFloatArray(float[] position)
    {
        return new Vector3(position[0], position[1], position[2]);
    }

    /// <summary>
    /// Returns a quaternion based on a given float[]
    /// </summary>
    /// <param name="position">Float Array</param>
    public static Quaternion GetQuaternionFromFloatArray(float[] rotation)
    {
        return Quaternion.Euler(rotation[0], rotation[1], rotation[2]);
    }

    /// <summary>
    /// Returns a Color based on a given int[]
    /// </summary>
    /// <param name="colors">Int Array</param>
    public static Color GetColorFromIntArray(int[] colors)
    {
        return new Color(colors[0], colors[1], colors[2]);
    }

    /// <summary>
    /// Destroys all gameobjects children under a root transform
    /// </summary>
    /// <param name="root">Root transform</param>
    public static void ClearChildren(Transform root)
    {
        if (!root) { return; }

        foreach (Transform child in root)
        {
            UnityEngine.Object.Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Checks if the current mouse is over any user interface
    /// </summary>
    public static bool IsMouseOverUI()
    {
        // Check if the EventSystem is null, which can happen if the UI system is not initialized
        if (EventSystem.current == null)
        {
            return false;
        }

        // Create a pointer event data object and set its position to the current mouse position
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        // Create a list to store the results of the raycast
        List<RaycastResult> results = new List<RaycastResult>();

        // Perform the raycast using the current mouse position
        EventSystem.current.RaycastAll(eventData, results);

        // If the results list is not empty, it means the mouse is over a UI element
        return results.Count > 0;
    }

    public static bool WriteValueToField<T>(string type, Type typeObject, T scriptableObject, KeyValuePair<string, object> property)
    {
        FieldInfo fieldInfo = typeObject.GetField(property.Key, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        if (fieldInfo == null)
        {
            Debug.LogWarning($"Field {property.Key} not found in requirement {type}. Using the default value.");
            return false;
        }

        Type fieldInfoType = fieldInfo.FieldType;
        object value = property.Value;

        try
        {
            // Convert the value to the type of the field ( int64 -> int32 )
            value = Convert.ChangeType(value, fieldInfoType);
        }
        catch (InvalidCastException)
        {
            Debug.LogWarning($"Type mismatch for field {property.Key} in requirement {type}. Using the default value.");
            return false;
        }

        fieldInfo.SetValue(scriptableObject, value);

        return true;
    }
}
