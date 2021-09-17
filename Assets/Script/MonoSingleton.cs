using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool shuttingDown = false;
    private static object locker = new object();

    private static T instance = null;

    public static T Instance
    {
        get
        {
            if (shuttingDown)
            {
                Debug.LogWarning(string.Format("[MonoSingleton] 인스턴스({0})은(는) 이미 삭제되었습니다. null을 반환합니다.", typeof(T)));
                return null;
            }

            lock (locker)
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();

                    if (instance == null)
                    {
                        instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                        DontDestroyOnLoad(instance);
                    }
                }
                return instance;
            }
        }
    }

    private void OnApplicationQuit()
    {
        shuttingDown = true;
    }

    private void OnDestroy()
    {
        shuttingDown = true;
    }
}