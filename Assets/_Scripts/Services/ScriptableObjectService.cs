using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableObjectService<T> : MonoBehaviour where T : DataSO
{
    [Header("ScriptableObjects")]
    [SerializeField] protected List<T> scriptableObjects = new();

    public virtual void CreateScriptableObjects()
    {
        Debug.Log("Type of: " + typeof(T).Name);
        Debug.Log("Number of scriptableObjects created: " + scriptableObjects.Count);
    }

    public List<T> GetAllScriptableObjects()
    {
        return scriptableObjects;
    }

    public T GetScriptableObject(string id)
    {
        T obj = scriptableObjects.Find(x => x.Id.Equals(id));
        if (!obj)
        {
            Debug.LogWarning("Unable to find ScriptableObject with ID: " + id);
        }

        return obj;
    }

    public T GetScriptableObject(int index)
    {
        T obj = scriptableObjects[index];
        if (!obj)
        {
            Debug.LogWarning("Unable to find ScriptableObject at index: " + index);
        }

        return obj;
    }

    public U GetOtherService<U>()
    {
        return transform.parent.GetComponentInChildren<U>();
    }
}