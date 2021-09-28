using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Canvas UICanvas = null;

    [SerializeField]
    RectTransform shopUI = null;

    [SerializeField]
    Image shopButton = null;

    [SerializeField]
    public Transform star = null;

    [SerializeField]
    public RectTransform starImage = null;

    [SerializeField]
    Image starLine = null;

    [SerializeField]
    Transform getEffect = null;

    [SerializeField]
    GameObject getEffectPrefab = null;

    [SerializeField]
    Text MoneyText = null;
    
    [SerializeField]
    RectTransform SETextPos = null;

    [SerializeField]
    Text MoneyStackText = null;

    [SerializeField]
    RectTransform OrbitsRTF = null;

    [SerializeField]
    Canvas StarCanvas = null;

    [SerializeField]
    Canvas OrbitCanvas = null;

    [SerializeField]
    RectTransform ArrowTransform = null;

    [SerializeField]
    CanvasGroup shopCanvasGroup = null;

    [SerializeField]
    Text shopOrbitText = null;
    
    private int MoneyStack = 0;
    private Vector2 originalScale;
    public bool isShopOpened = false;
    PoolManager getMoneyEffectPool = null;
    float starYPos;

    public float ShakeStrength = 1;
    public float ShakeAmount = 1;
    float ShakeTime = 0;
    Vector2 initialPosition = new Vector2(0, 0);

    private void Start()
    {
        getMoneyEffectPool = new PoolManager(getEffectPrefab);
        starYPos = ArrowTransform.position.y;
        shopUI.anchoredPosition = new Vector2(0, -UICanvas.GetComponent<RectTransform>().sizeDelta.y);
        shopCanvasGroup.alpha = 0;
        UpdateUI();
    }

    private void Update()
    {
        if (ShakeTime > 0)
        {
            var randomShake = Random.insideUnitCircle * ShakeTime;
            StarCanvas.transform.position = randomShake;
            OrbitCanvas.transform.position = randomShake;
            ShakeTime -= Time.deltaTime;
        }
        else
        {
            ShakeTime = 0.0f;
            StarCanvas.transform.position = initialPosition;
            OrbitCanvas.transform.position = initialPosition;
            StarCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            OrbitCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        }
    }

    public void VibrateForTime(float time)
    {
        ShakeTime = time;
        Vibration.Vibrate((long)(time * 1000));
        StarCanvas.renderMode = RenderMode.WorldSpace;
        OrbitCanvas.renderMode = RenderMode.WorldSpace;
    }

    public void OpenShop()
    {
        originalScale = star.localScale;
        Sequence sequence = DOTween.Sequence();
        Sequence sequence2 = DOTween.Sequence();
        sequence.Join(starImage.DOLocalMoveY(-200, .5f));
        sequence.Join(OrbitsRTF.DOLocalMoveY(-200, .5f));
        sequence.AppendInterval(.05f);
        sequence.Append(starImage.DOMove(new Vector2(0, starYPos), .5f));
        sequence.Join(star.DOScale(new Vector2(1f, 1f), .5f));
        sequence.Join(shopUI.DOAnchorPosY(0, .5f));
        sequence.Join(shopButton.DOFade(0, .25f));
        sequence.Join(starLine.DOFade(0, .25f));
        sequence2.AppendInterval(.75f);
        sequence2.Append(shopCanvasGroup.DOFade(1, .5f));
        sequence2.Join(shopOrbitText.DOFade(1, .5f));
        sequence.Join(OrbitsRTF.DOLocalMove(Vector3.zero, .5f));
        sequence.AppendCallback(() => isShopOpened = true);
        GameManager.Instance.Tasks.Quit.AddTask(() => CloseShop());
    }

    private void SubStarLight(bool turnOn)
    {
        
    }

    public void CloseShop()
    {
        isShopOpened = false;
        Sequence sequence =  DOTween.Sequence();
        sequence.Join(shopUI.DOAnchorPosY(-UICanvas.GetComponent<RectTransform>().sizeDelta.y, .5f));
        sequence.Join(starImage.DOMove(Vector2.zero, .5f));
        sequence.Join(star.DOScale(originalScale, .5f));
        sequence.Join(shopOrbitText.DOFade(0, .5f));
        sequence.Join(shopButton.DOFade(1, .5f));
        sequence.Join(starLine.DOFade(1, .5f));
        sequence.Join(shopCanvasGroup.DOFade(0, .25f));
    }

    public void CloseShopButton()
    {
        GameManager.Instance.Tasks.Quit.BackButtonHandler();
    }

    int timer = 0;
    bool isStackLooping = false;
    private void showStackText()
    {
        timer = 0;
        if (MoneyStack == 0)
        {
            SETextPos.DOAnchorPosY(30, .5f).From(Vector2.zero);
            MoneyStackText.GetComponent<RectTransform>().anchoredPosition = new Vector2(-492, -226);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(MoneyStackText.DOFade(1f, .5f).From(0));
            sequence.AppendCallback(() => {
                if (!isStackLooping) StartCoroutine(StackTimerLoop());
            });
        }
    }

    private void hideStackText()
    {
        // TODO 나중에 시간 되면 펑 하면서 들어가는거 만들기
        GameManager.Instance.Data.Player.StarEnergy += MoneyStack;
        UpdateUI();
        MoneyStack = 0;
        Sequence sequence = DOTween.Sequence();
        sequence.Join(MoneyStackText.DOFade(0f, .5f).From(1));
        sequence.Join(SETextPos.DOAnchorPosY(0, .5f));
    }

    private IEnumerator StackTimerLoop()
    {
        if (isStackLooping) yield break;
        while(timer <= 10)
        {
            timer++;
            yield return new WaitForSeconds(0.1f);
        }
        isStackLooping = false;
        hideStackText();
    }

    public void GetMoneyEffect(int value, Vector3 startPos, bool isDirect = false)
    {
        if (isShopOpened)
        {
            GameManager.Instance.Data.Player.StarEnergy += value;
            UpdateUI();
            return;
        }
        
        var EffectObject = getMoneyEffectPool.GetObject();
        EffectObject.transform.SetParent(getEffect);
        EffectObject.transform.localScale = Vector3.one;
        EffectObject.GetComponent<Image>().color += new Color(0, 0, 1);
        var EffectPos = startPos;
        EffectPos.z = 100;
        EffectObject.transform.position = EffectPos;
        Sequence sequence = DOTween.Sequence();
        var RandomPos = EffectPos + (Random.insideUnitSphere * 0.5f);
        sequence.Append(EffectObject.transform.DOMove(RandomPos, .5f));
        sequence.Append(EffectObject.GetComponent<Image>().DOFade(0, .5f));
        if (!isDirect)
        {
            sequence.Join(EffectObject.transform.DOMove(MoneyStackText.transform.position, .5f));
            sequence.AppendCallback(() => {
                getMoneyEffectPool.PutObject(EffectObject);
                showStackText();
                MoneyStack += value;
                UpdateUI();
            });
        }
        else
        {
            VibrateForTime(0.05f);
            sequence.Join(EffectObject.transform.DOMove(MoneyText.transform.position, .5f));
            sequence.AppendCallback(() => {
                GameManager.Instance.Data.Player.StarEnergy += value;
                UpdateUI();
            });
        }
    }

    public void UpdateUI()
    {
        MoneyStackText.text = $"+ {MoneyStack.ToString("N0")} SE";
        MoneyText.text = $"{GameManager.Instance.Data.Player.StarEnergy.ToString("N0")} SE";
        MoneyText.GetComponent<NumberCounter>().Value = GameManager.Instance.Data.Player.StarEnergy;
    }
}
