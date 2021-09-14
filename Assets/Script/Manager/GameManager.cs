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
    TaskManager taskManager = null;
    PopupManager popupManager = null;
    UIManager uIManager = null;

    public DataManager Data
    {
        get
        {
            if(dataManager == null) dataManager = new DataManager();
            return dataManager;
        }
    }

    public TaskManager Tasks
    {
        get
        {
            if (taskManager == null) taskManager = GetComponent<TaskManager>();
            return taskManager;
        }
    }

    public PopupManager Popup
    {
        get
        {
            if (popupManager == null) popupManager = GetComponent<PopupManager>();
            return popupManager;
        }
    }
    public UIManager UI
    {
        get
        {
            if (uIManager == null) uIManager = GetComponent<UIManager>();
            return uIManager;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        StarImage.alphaHitTestMinimumThreshold = 0.5f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PopupButton button1 = new PopupButton("�׽�Ʈ ����", () => Popup.Hide());
            PopupButton button2 = new PopupButton("�׽�Ʈ ������", () => Popup.Hide());
            Popup.Show("�׽�Ʈ �˾�", "�׽�Ʈ �˾� ����~~~", button1, button2);
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
            PopupButton button1 = new PopupButton("��...", () => QuitGame(true));
            PopupButton button2 = new PopupButton("�ƴ�!", () => Popup.Hide(), ButtonType.Black);
            Popup.Show("���� ������ �ǰ���...?", "�����Ŵٸ� ������ �ʰ����� ���� ������ ���� �� ���׿�...\n\n������ �� ��¼�ڳ��� �����Ŵٴµ�...\n\n�� ����� ������ �����ؿ�...!", button1, button2);
        }
    }
}
