using PopupType;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    Image StarImage = null;

    DataManager dataManager = null;
    public DataManager Data { get { return dataManager ??= GetComponent<DataManager>(); } }

    TaskManager taskManager = null;
    public TaskManager Tasks { get { return taskManager ??= GetComponent<TaskManager>(); } }

    PopupManager popupManager = null;
    public PopupManager Popup { get { return popupManager ??= GetComponent<PopupManager>(); } }

    UIManager uIManager = null;
    public UIManager UI { get { return uIManager ??= GetComponent<UIManager>(); } }

    StarManager starManager = null;
    public StarManager Star { get { return starManager ??= GetComponent<StarManager>(); } }

    ShopManager shopManager = null;
    public ShopManager Shop { get { return shopManager ??= GetComponent<ShopManager>(); } }

    ClickEffectManager clickEffect = null;
    public ClickEffectManager ClickEffect { get { return clickEffect ??= GetComponent<ClickEffectManager>(); } }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        StarImage.alphaHitTestMinimumThreshold = 0.5f;
        Data.LoadData();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            foreach (Orbit orbit in Star.Orbits)
            {
                if (orbit.Enabled)
                    orbit.AddSubStar();
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Star.AddOrbit();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            UI.VibrateForTime(1f);
        }
    }

    public GameObject InstantiateObj(GameObject gameObject)
    {
        return Instantiate(gameObject);
    }

    public Vector3 TouchPoint()
    {
        if (Input.touches.Length > 0)
        {
            return Input.touches[0].position;
        }
        else
        {
            return Input.mousePosition;
        }
    }

    public void QuitGame(bool isForce = false)
    {
        if (isForce)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif
        }
        else
        {
            PopupButton button1 = new PopupButton("네...", () => QuitGame(true));
            PopupButton button2 = new PopupButton("아뇨!", () => Popup.Hide(), ButtonType.Black);
            Popup.Show("정말 떠나실 건가요...?", "떠나신다면 막지는 않겠지만 정말 마음이 아플 것 같네요...\n\n하지만 뭐 어쩌겠나요 떠나신다는데...\n\n전 당신의 선택을 존중해요...!", button1, button2);
        }
    }
}
