using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Mono单例类基类
/// </summary>
public class MonoSingleton<T> : MonoBehaviour where T: MonoBehaviour 
{
    public bool IsGlobal = false;

    private static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance =(T)FindObjectOfType<T>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if(IsGlobal)
        {
            if(instance != null && instance != this.gameObject.GetComponent<T>())
            {
                Destroy(this.gameObject);
                return;
            }
            DontDestroyOnLoad(this.gameObject);
            instance = this.gameObject.GetComponent<T>();
        }
        this.OnStart();
    }

    protected virtual void OnStart()
    {

    }
}
