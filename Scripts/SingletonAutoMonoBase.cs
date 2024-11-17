using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonAutoMonoBase<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = typeof(T).ToString();
                DontDestroyOnLoad(obj); // 支持跨scene
                instance = obj.AddComponent<T>();
            }
            return instance;
        }
    }
}

