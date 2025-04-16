using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RenownedGames.Apex;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveService : MonoBehaviour
{
    public static SaveService Instance { get; private set; }

    private List<ISave> saveObjects = new();
    private List<ILoad> loadObjects = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // temporary till we have a proper scene service
        SceneManager.LoadSceneAsync("WorldScene");
    }

    public void Save()
    {
        Save save = new();
        save.Version = 0.4f;

        foreach (ISave saveable in saveObjects)
        {
            saveable.Save(save);
        }

        FileService.SaveToFile(save);
    }

    public void Load()
    {
        Save save = FileService.LoadFromFile();

        foreach (ILoad loadable in loadObjects)
        {
            loadable.Load(save);
        }
    }

    public void CacheAllSaveObjects()
    {
        IEnumerable<ISave> objects = FindObjectsOfType<MonoBehaviour>().OfType<ISave>();
        saveObjects = new List<ISave>(objects);
    }

    public void CacheAllLoadObjects()
    {
        IEnumerable<ILoad> objects = FindObjectsOfType<MonoBehaviour>().OfType<ILoad>();
        loadObjects = new List<ILoad>(objects);
    }
}