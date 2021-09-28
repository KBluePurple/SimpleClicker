using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    RectTransform items = null;

    [SerializeField]
    Text[] orbitTitles = null;

    [SerializeField]
    Button[] arrows = null;

    [SerializeField]
    Text valueText = null;

    int curOrbitIndex = 0;
    int curItemIndex = 1;
    bool isPlayingAnimation = false;

    private void Start()
    {
        for (int i = 0; i < items.childCount; i++)
        {
            int temp = i;
            items.GetChild(temp).GetComponent<Button>().onClick.AddListener(() =>
            {
                GameManager.Instance.ClickEffect
                .ClickEffectObject(items.GetChild(temp).GetChild(0).GetChild(0), 3f);

                onClickItem(temp);
            });
        }

        orbitTitles[0].text = "Core\nStar";
        orbitTitles[0].GetComponent<RectTransform>().DOLocalMoveX(0, .5f);

        arrows[0].interactable = false;
        arrows[0].GetComponent<CanvasGroup>().alpha = 0;
        items.GetComponent<CanvasGroup>().alpha = 0;
        items.anchoredPosition = new Vector2(-450, items.anchoredPosition.y);
        items.GetChild(0).GetComponent<CanvasGroup>().alpha = 0f;
        items.GetChild(0).GetComponent<Button>().interactable = false;
        items.GetChild(1).GetComponent<CanvasGroup>().alpha = 0f;
        items.GetChild(1).GetComponent<Button>().interactable = false;
        items.GetChild(2).GetComponent<CanvasGroup>().alpha = 1f;
        items.GetChild(2).GetComponent<Button>().interactable = true;
        items.GetChild(2).GetChild(0)
        .GetComponent<RectTransform>().localScale = Vector2.one;
        curItemIndex = 2;
        items.GetComponent<CanvasGroup>().alpha = 1;

        arrows[0].onClick.AddListener(() => changeOrbitSelect(true));
        arrows[1].onClick.AddListener(() => changeOrbitSelect(false));
    }

    private void changeOrbitSelect(bool isLeft)
    {
        if (isPlayingAnimation) return;
        isPlayingAnimation = true;
        Sequence sequence = DOTween.Sequence();
        if (isLeft && curOrbitIndex > 0)
        {
            curOrbitIndex--;
            if (curOrbitIndex == 0)
                orbitTitles[1].text = "Core\nStar";
            else
                orbitTitles[1].text = $"{curOrbitIndex}st\nOrbit";

            orbitTitles[1].GetComponent<RectTransform>().localPosition = new Vector2(-500, 0);
            sequence.Join(orbitTitles[0].GetComponent<RectTransform>().DOLocalMoveX(500, .5f));
            sequence.Join(orbitTitles[1].GetComponent<RectTransform>().DOLocalMoveX(0, .5f));
            sequence.AppendCallback(() => {
                orbitTitles[0].text = orbitTitles[1].text;
                orbitTitles[0].GetComponent<RectTransform>().localPosition = orbitTitles[1].GetComponent<RectTransform>().localPosition;
                orbitTitles[1].GetComponent<RectTransform>().localPosition = new Vector2(-500, 0);
            });
        }
        else if (!isLeft && curItemIndex < 4)
        {
            curOrbitIndex++;
            orbitTitles[1].text = $"{curOrbitIndex}st\nOrbit";

            orbitTitles[1].GetComponent<RectTransform>().localPosition = new Vector2(500, 0);
            sequence.Join(orbitTitles[0].GetComponent<RectTransform>().DOLocalMoveX(-500, .5f));
            sequence.Join(orbitTitles[1].GetComponent<RectTransform>().DOLocalMoveX(0, .5f));
            sequence.AppendCallback(() => {
                orbitTitles[0].text = orbitTitles[1].text;
                orbitTitles[0].GetComponent<RectTransform>().localPosition = orbitTitles[1].GetComponent<RectTransform>().localPosition;
                orbitTitles[1].GetComponent<RectTransform>().localPosition = new Vector2(-500, 0);
            });
        }

        if (curOrbitIndex == 0)
        {
            arrows[0].interactable = false;
            sequence.Join(arrows[0].GetComponent<CanvasGroup>().DOFade(0, .25f));
            Sequence sequence1 = DOTween.Sequence();
            sequence1.Join(items.GetComponent<CanvasGroup>().DOFade(0, .25f));
            sequence1.AppendCallback(() => {
                items.anchoredPosition = new Vector2(-450, items.anchoredPosition.y);
                items.GetChild(0).GetComponent<CanvasGroup>().alpha = 0f;
                items.GetChild(0).GetComponent<Button>().interactable = false;
                items.GetChild(1).GetComponent<CanvasGroup>().alpha = 0f;
                items.GetChild(1).GetComponent<Button>().interactable = false;
                items.GetChild(2).GetComponent<CanvasGroup>().alpha = 1f;
                items.GetChild(2).GetComponent<Button>().interactable = true;
                items.GetChild(2).GetChild(0)
                .GetComponent<RectTransform>().localScale = Vector2.one;
                curItemIndex = 2;
            });
            sequence1.Join(items.GetComponent<CanvasGroup>().DOFade(1, .25f));
        }
        else if (curOrbitIndex == 4)
        {
            arrows[1].interactable = false;
            sequence.Join(arrows[1].GetComponent<CanvasGroup>().DOFade(0, .25f));
            Sequence sequence1 = DOTween.Sequence();
            sequence1.Join(items.GetComponent<CanvasGroup>().DOFade(0, .25f));
            sequence1.AppendCallback(() => {
                items.anchoredPosition = new Vector2(0, items.anchoredPosition.y);
                items.GetChild(0).GetComponent<CanvasGroup>().alpha = .5f;
                items.GetChild(0).GetComponent<Button>().interactable = true;
                items.GetChild(0).GetChild(0)
                .GetComponent<RectTransform>().localScale = new Vector2(.8f, .8f);
                items.GetChild(1).GetComponent<CanvasGroup>().alpha = 1f;
                items.GetChild(1).GetComponent<Button>().interactable = true;
                items.GetChild(1).GetChild(0)
                .GetComponent<RectTransform>().localScale = Vector2.one;
                items.GetChild(2).GetComponent<CanvasGroup>().alpha = .5f;
                items.GetChild(2).GetComponent<Button>().interactable = true;
                items.GetChild(2).GetChild(0)
                .GetComponent<RectTransform>().localScale = new Vector2(.8f, .8f);
                curItemIndex = 1;
            });
            sequence1.Join(items.GetComponent<CanvasGroup>().DOFade(1, .25f));
        }
        else
        {
            arrows[0].interactable = true;
            sequence.Join(arrows[0].GetComponent<CanvasGroup>().DOFade(1, .25f));
            arrows[1].interactable = true;
            sequence.Join(arrows[1].GetComponent<CanvasGroup>().DOFade(1, .25f));
            Sequence sequence1 = DOTween.Sequence();
            sequence1.Join(items.GetComponent<CanvasGroup>().DOFade(0, .25f));
            sequence1.AppendCallback(() => {
                items.anchoredPosition = new Vector2(0, items.anchoredPosition.y);
                items.GetChild(0).GetComponent<CanvasGroup>().alpha = .5f;
                items.GetChild(0).GetComponent<Button>().interactable = true;
                items.GetChild(0).GetChild(0)
                .GetComponent<RectTransform>().localScale = new Vector2(.8f, .8f);
                items.GetChild(1).GetComponent<CanvasGroup>().alpha = 1f;
                items.GetChild(1).GetComponent<Button>().interactable = true;
                items.GetChild(1).GetChild(0)
                .GetComponent<RectTransform>().localScale = Vector2.one;
                items.GetChild(2).GetComponent<CanvasGroup>().alpha = .5f;
                items.GetChild(2).GetComponent<Button>().interactable = true;
                items.GetChild(2).GetChild(0)
                .GetComponent<RectTransform>().localScale = new Vector2(.8f, .8f);
                curItemIndex = 1;
            });
            sequence1.Join(items.GetComponent<CanvasGroup>().DOFade(1, .25f));
        }
        
        sequence.AppendInterval(.25f);
        sequence.AppendCallback(() => {
            isPlayingAnimation = false;
        });
        UpdateShopUI();
    }

    public void UpdateShopUI()
    {
        valueText.GetComponent<NumberCounter>().Value += 10;
    }

    public void onClickItem(int index)
    {
        float animationTime = 0.5f;
        switch (index)
        {
            case 0:
                if (curItemIndex == 1)
                {
                    items.DOAnchorPosX(450, .5f);
                    items.GetChild(0).GetComponent<CanvasGroup>().DOFade(1f, animationTime);
                    items.GetChild(0).GetChild(0)
                    .GetComponent<RectTransform>().DOScale(Vector2.one, .5f);
                    items.GetChild(1).GetComponent<CanvasGroup>().DOFade(.5f, animationTime);
                    items.GetChild(1).GetChild(0)
                    .GetComponent<RectTransform>().DOScale(new Vector2(.8f, .8f), .5f);
                    items.GetChild(2).GetComponent<CanvasGroup>().DOFade(0, animationTime);
                    curItemIndex = 0;
                }
                break;
            case 1:
                if (curItemIndex != 1)
                {
                    items.DOAnchorPosX(0, .5f);
                    items.GetChild(0).GetComponent<CanvasGroup>().DOFade(.5f, animationTime);
                    items.GetChild(0).GetChild(0)
                    .GetComponent<RectTransform>().DOScale(new Vector2(.8f, .8f), .5f);
                    items.GetChild(1).GetComponent<CanvasGroup>().DOFade(1f, animationTime);
                    items.GetChild(1).GetChild(0)
                    .GetComponent<RectTransform>().DOScale(Vector2.one, .5f);
                    items.GetChild(2).GetComponent<CanvasGroup>().DOFade(.5f, animationTime);
                    items.GetChild(2).GetChild(0)
                    .GetComponent<RectTransform>().DOScale(new Vector2(.8f, .8f), .5f);
                    curItemIndex = 1;
                }
                break;
            case 2:
                if (curItemIndex == 1)
                {
                    items.DOAnchorPosX(-450, .5f);
                    items.GetChild(0).GetComponent<CanvasGroup>().DOFade(0, animationTime);
                    items.GetChild(1).GetComponent<CanvasGroup>().DOFade(.5f, animationTime);
                    items.GetChild(1).GetChild(0)
                    .GetComponent<RectTransform>().DOScale(new Vector2(.8f, .8f), .5f);
                    items.GetChild(2).GetComponent<CanvasGroup>().DOFade(1f, animationTime);
                    items.GetChild(2).GetChild(0)
                    .GetComponent<RectTransform>().DOScale(Vector2.one, .5f);
                    curItemIndex = 2;
                }
                break;
        }
        UpdateShopUI();
    }
}
