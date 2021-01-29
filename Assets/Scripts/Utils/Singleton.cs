using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: Singleton<T> //prevents creating singletons of objects that aren't meant to be singletons
{
    private static T instance;
    public static T Instance
    {
        get { return instance; } //protecting internally while sharing externally
    }

    public static bool IsInitialized
    {
        get { return instance != null; }
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("[Singleton] Trying to instantiate a second instance of singleton class.");
        }
        else
        {
            instance = (T)this;
        }
    }

    protected virtual void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;
        }
    }
}
