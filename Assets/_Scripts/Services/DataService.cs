using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataService : FileService
{
    public static T CreateSingleDataFromFilesAndMods<T>(string dataFileName)
    {
        return CreateDataFromFilesAndMods<T>(dataFileName).FirstOrDefault();
    }

    public static List<T> CreateDataFromFilesAndMods<T>(string dataFileName)
    {
        string dataPath = Path.Combine(GAME_DATA_DEFAULT_FOLDER_NAME, DATA_FOLDER_NAME, dataFileName + DATA_FILE_EXTENSION);
        string basePath = Path.Combine(GAME_DATA_PATH, dataPath);

        // Base game data
        List<T> baseData = LoadDataGeneric<T>(basePath);

        // Loop trough the mods and check if id exists, if not then create new data
        // If exist, update it.
        foreach (string folder in Directory.GetDirectories(Path.Combine(PERSISTENT_DATA_PATH, MOD_FOLDER_NAME)))
        {
            string moddedFolderName = Path.GetFileName(folder);

            Debug.Log($"Found mod folder: {moddedFolderName}");

            string modPath = Path.Combine(PERSISTENT_DATA_PATH, MOD_FOLDER_NAME, moddedFolderName, dataPath);
            List<T> moddedData = LoadDataGeneric<T>(modPath);

            if (moddedData == null) { continue; }

            Debug.Log($"Found modded file: {dataFileName}");

            // Loop over mod data
            for (int x = 0; x < moddedData.Count; x++)
            {
                if (moddedData[x] is not BaseData) { continue; }

                BaseData moddedDataInfo = moddedData[x] as BaseData;
                bool foundBaseData = false;

                // Loop over base data
                for (int y = 0; y < baseData.Count; y++)
                {
                    if (baseData[y] is not BaseData) { continue; }

                    BaseData newDataId = baseData[y] as BaseData;

                    // Replace base game data with mod data
                    if (newDataId.Id.Equals(moddedDataInfo.Id))
                    {
                        baseData[y] = moddedData[x];
                        foundBaseData = true;
                        Debug.Log($"Replaced base data (id: {newDataId.Id}) with modded data");
                    }
                }

                // We couldn't find the id in base data, so we add it new
                if (!foundBaseData)
                {
                    baseData.Add(moddedData[x]);
                    Debug.Log($"No basedata found for {moddedData[x]}, creating new data entry");
                }
            }

            Debug.Log($"Finished mod folder: {moddedFolderName}");
        }

        Debug.Log($"Finished getting data for {dataFileName}");

        return baseData;
    }
}
