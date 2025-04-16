using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStorage : MonoBehaviour
{
    private object obj;

    public void SetObject<T>(T value)
    {
        obj = value;
    }

    public T GetObject<T>()
    {
        return (T)obj;
    }
}
