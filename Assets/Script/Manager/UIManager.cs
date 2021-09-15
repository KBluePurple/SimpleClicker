using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    RectTransform shopUI = null;

    [SerializeField]
    Image shopButton = null;

    [SerializeField]
    Transform star = null;

    [SerializeField]
    RectTransform starImage = null;

    [SerializeField]
    CanvasGroup ArrowButtons = null;

    private Vector2 originalScale;

    public void OpenShop()
    {
        originalScale = star.localScale;
        GameManager.Instance.Tasks.Quit.AddTask(() => CloseShop());
        shopUI.DOLocalMoveY(2200, .5f);
        starImage.DOLocalMoveY(520, .5f);
        star.DOScale(new Vector2(1f, 1f), .5f);
        shopButton.DOFade(0, .25f);
        ArrowButtons.DOFade(0, .25f);
    }

    public void CloseShop()
    {
        shopUI.DOLocalMoveY(0, .5f);
        starImage.DOLocalMoveY(0, .5f);
        star.DOScale(originalScale, .5f);
        shopButton.DOFade(1, .5f);
        ArrowButtons.DOFade(1, .5f);
    }

    public void CloseShopButton()
    {
        GameManager.Instance.Tasks.Quit.BackButtonHandler();
    }
}
