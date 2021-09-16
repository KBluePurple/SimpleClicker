using PopupType;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

namespace PopupType
{
    public enum ButtonType
    {
        While,
        Black
    }

    public class PopupButton
    {
        public string Text;
        public Action OnPress;
        public ButtonType Type;

        public PopupButton(string text, Action onPress, ButtonType type = ButtonType.While)
        {
            Text = text;
            OnPress = onPress;
            Type = type;
        }
    }
}

public class PopupManager : MonoBehaviour
{
    [SerializeField]
    Canvas popupCanvas = null;

    [SerializeField]
    Text titleText = null;

    [SerializeField]
    Text descriptionText = null;

    [SerializeField]
    Button button1 = null;

    [SerializeField]
    Text button1Text = null;

    [SerializeField]
    Button button2 = null;

    [SerializeField]
    Text button2Text = null;

    [SerializeField]
    CanvasGroup popupGroup = null;

    [SerializeField]
    Image bluePanel = null;

    [SerializeField]
    Sprite[] buttons = null;

    private void Start()
    {
        popupCanvas.gameObject.SetActive(false);
        button1.onClick.AddListener(() => Hide());
    }

    public void Show(string title, string description, PopupButton button1, PopupButton button2)
    {
        if (popupCanvas.gameObject.activeSelf) return;

        GameManager.Instance.Tasks.Quit.AddTask(() =>
        {
            bluePanel.material.DOFloat(0, "_Size", .3f);
            Sequence fadeoutSequence = DOTween.Sequence();
            fadeoutSequence.Append(popupGroup.DOFade(0, .5f));
            fadeoutSequence.AppendCallback(() => popupCanvas.gameObject.SetActive(false));
            bluePanel.DOFade(0, .5f);
        });
        
        popupGroup.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(1864.8f, 0);

        bluePanel.material.SetFloat("_Size", 0);
        popupGroup.alpha = 0;
        popupCanvas.gameObject.SetActive(true);
        titleText.text = title;
        descriptionText.text = description;

        this.button1.onClick.RemoveAllListeners();
        this.button1.onClick.AddListener(button1.OnPress.Invoke);
        button1Text.text = button1.Text;
        this.button1.GetComponent<Image>().sprite = button1.Type switch
        {
            ButtonType.Black => buttons[1],
            _ => buttons[0]
        };
        button1Text.color = button1.Type switch
        {
            ButtonType.Black => new Color(255f / 255f, 255f / 255f, 255f / 255f),
            _ => new Color(56f / 255f, 62f / 255f, 105f / 255f)
        };

        this.button2.onClick.RemoveAllListeners();
        this.button2.onClick.AddListener(button2.OnPress.Invoke);
        button2Text.text = button2.Text;
        this.button2.GetComponent<Image>().sprite = button2.Type switch
        {
            ButtonType.Black => buttons[1],
            _ => buttons[0]
        };
        button2Text.color = button2.Type switch
        {
            ButtonType.Black => new Color(255f / 255f, 255f / 255f, 255f / 255f),
            _ => new Color(56f / 255f, 62f / 255f, 105f / 255f)
        };

        popupGroup.GetComponent<Image>().rectTransform.DOSizeDelta(new Vector2(1864.8f, 1727.3f), .3f);
        popupGroup.DOFade(1, .5f);

        bluePanel.DOFade(.3f, .5f);
    }

    public void Hide()
    {
        GameManager.Instance.Tasks.Quit.BackButtonHandler();
    }
}
