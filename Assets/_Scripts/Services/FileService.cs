using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public class FileService
{
    // Base data
    protected static string GAME_DATA_PATH = Application.streamingAssetsPath;
    protected const string GAME_DATA_DEFAULT_FOLDER_NAME = "Default";
    protected const string DATA_FOLDER_NAME = "Data";
    protected const string ART_BASE_FOLDER_NAME = "Art";
    protected const string DATA_FILE_EXTENSION = ".json";

    // Mod data
    protected static string PERSISTENT_DATA_PATH = Application.persistentDataPath;
    protected const string MOD_FOLDER_NAME = "Mods";

    // Save data
    protected const string SAVE_DATA_FOLDER_NAME = "Saves";

    public static void SaveToFile(Save save)
    {
        save.LastTimePlayed = DateTime.Now.ToBinary();

        string tempPath = Path.Combine(PERSISTENT_DATA_PATH, SAVE_DATA_FOLDER_NAME);
        tempPath = Path.Combine(tempPath, "Save.json");

        //Convert To Json then to bytes
        string jsonData = JsonConvert.SerializeObject(save);
        byte[] jsonByte = Encoding.ASCII.GetBytes(jsonData);

        try
        {
            File.WriteAllBytes(tempPath, jsonByte);
            Debug.Log("Saved Data to: " + tempPath.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed to save Data to: " + tempPath.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }
    }

    public static Save LoadFromFile()
    {
        string tempPath = Path.Combine(PERSISTENT_DATA_PATH, SAVE_DATA_FOLDER_NAME);
        tempPath = Path.Combine(tempPath, "Save.json");

        //Load saved Json
        byte[] jsonByte = null;
        try
        {
            jsonByte = File.ReadAllBytes(tempPath);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Load Save File from: " + tempPath.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }

        //Convert to json string
        string jsonData = Encoding.ASCII.GetString(jsonByte);
        //Convert to Object
        return JsonConvert.DeserializeObject<Save>(jsonData);
    }

    /// <summary>
    /// Creates the 'mod' and 'save' folder in the persistantDataPath if they have not been created yet.
    /// </summary>
    public static void CreateDirectories()
    {
        string persistentDataPath = Path.Combine(Application.persistentDataPath);
        string modsFolderPath = Path.Combine(persistentDataPath, MOD_FOLDER_NAME);
        string saveFolderPath = Path.Combine(persistentDataPath, SAVE_DATA_FOLDER_NAME);

        // Create Directory if it does not exist
        if (!Directory.Exists(modsFolderPath))
        {
            Debug.Log("Created mods directory");
            Directory.CreateDirectory(modsFolderPath);
        }

        if (!Directory.Exists(saveFolderPath))
        {
            Debug.Log("Created save directory");
            Directory.CreateDirectory(saveFolderPath);
        }
    }

    protected static bool DoesDirectoryExist(string path)
    {
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Debug.LogWarning("Directory does not exist at path: " + path);
            return false;
        }

        return true;
    }

    protected static bool DoesFileExist(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning("File does not exist at path: " + path);
            return false;
        }

        return true;
    }

    protected static List<T> LoadDataGeneric<T>(string path)
    {
        if (!DoesDirectoryExist(path))
        {
            return null;
        }

        if (!DoesFileExist(path))
        {
            return null;
        }

        byte[] jsonByte = null;

        try
        {
            jsonByte = File.ReadAllBytes(path);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Load Data from: " + path.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }

        string jsonData = Encoding.ASCII.GetString(jsonByte);

        List<T> resultValue = JsonConvert.DeserializeObject<List<T>>(jsonData);

        return resultValue;
    }
}
