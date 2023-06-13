using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: MonoBehaviour
{
    private static object objLock = new object();

    private static T instance;
    public static T GetInstance
    {
        get
        {
            lock (objLock)
            {
                if (instance == null)
                {
                    GameObject obj = GameObject.Find(typeof(T).Name);
                    if (!obj)
                    {
                        obj = new GameObject(typeof(T).Name);
                        instance = obj.AddComponent<T>();
                    }
                    else
                    {
                        instance = obj.GetComponent<T>();
                    }
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }

        var TtypeObjects = FindObjectsOfType<T>();
        if (TtypeObjects.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        OnAwake();
    }

    protected virtual void OnAwake() { }
}
