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
                Debug.LogWarning(string.Format("[MonoSingleton] �ν��Ͻ�({0})��(��) �̹� �����Ǿ����ϴ�. null�� ��ȯ�մϴ�.", typeof(T)));
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