using PopupType;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    Image StarImage = null;

    DataManager dataManager = null;
    public DataManager Data { get { return dataManager ??= new DataManager(); } }

    TaskManager taskManager = null;
    public TaskManager Tasks { get { return taskManager ??= GetComponent<TaskManager>(); } }

    PopupManager popupManager = null;
    public PopupManager Popup { get { return popupManager ??= GetComponent<PopupManager>(); } }
    
    UIManager uIManager = null;
    public UIManager UI { get { return uIManager ??= GetComponent<UIManager>(); } }
    
    StarManager starManager = null;
    public StarManager Star { get { return starManager ??= GetComponent<StarManager>(); } }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        StarImage.alphaHitTestMinimumThreshold = 0.5f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Star.AddOrbit();
            // PopupButton button1 = new PopupButton("테스트 왼쪽", () => Popup.Hide());
            // PopupButton button2 = new PopupButton("테스트 오른쪽", () => Popup.Hide());
            // Popup.Show("테스트 팝업", "테스트 팝업 설명~~~", button1, button2);
        }
    }

    public GameObject InstantiateObj(GameObject gameObject)
    {
        return Instantiate(gameObject);
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
