using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    [SerializeField]
    RectTransform settingUI = null;

    [SerializeField]
    public AudioMixer audioMixer = null;

    [SerializeField]
    public Slider masterSlider = null;

    [SerializeField]
    public Slider musicSlider = null;

    [SerializeField]
    public Slider effectSlider = null;

    [SerializeField]
    public Settings Settings;

    [SerializeField]
    Image[] sprites;

    [SerializeField]
    Light2D[] lights;

    [SerializeField]
    Material slowMaterial;

    [SerializeField]
    Material fastMaterial;

    private void Start()
    {
        settingUI.localPosition = new Vector2(1440, 0);
    }

    public void OpenSetting()
    {
        settingUI.DOLocalMoveX(0, .5f);
        GameManager.Instance.Tasks.Quit.AddTask(() => settingUI.DOLocalMoveX(1440, .5f));
    }

    public void CloseSetting()
    {
        GameManager.Instance.Tasks.Quit.BackButtonHandler();
    }

    public void Master_IsValueChanged(float value)
    {
        Debug.Log("Master " + value);
        audioMixer.SetFloat("Master", value);
        Settings.MasterVol = value;
    }

    public void Music_IsValueChanged(float value)
    {
        Debug.Log("Music " + value);
        audioMixer.SetFloat("Music", value);
        Settings.MasterVol = value;
    }

    public void Effect_IsValueChanged(float value)
    {
        Debug.Log("Effect " + value);
        audioMixer.SetFloat("Effect", value);
        Settings.EffectVol = value;
    }

    public void FastMode(bool value)
    {
        GameManager.Instance.ClickEffect.FastButtonEffectText.text = GameManager.Instance.Setting.Settings.FastMode ? "최적화 모드 끄기" : "최적화 모드 켜기";
        if (value)
        {
            foreach (var item in sprites)
            {
                item.material = fastMaterial;
            }

            foreach (var item in lights)
            {
                item.enabled = false;
            }

            foreach (var item in GameManager.Instance.Star.Orbits)
            {
                foreach (var item1 in item.SubStars)
                {
                    item1.GetComponent<Light2D>().enabled = false;
                }
            }

            sprites[6].color = new Color(0.212f, 0.208f, 0.38f);
        }
        else
        {
            foreach (var item in sprites)
            {
                item.material = slowMaterial;
            }

            foreach (var item in lights)
            {
                item.enabled = true;
            }

            foreach (var item in GameManager.Instance.Star.Orbits)
            {
                foreach (var item1 in item.SubStars)
                {
                    item1.GetComponent<Light2D>().enabled = true;
                }
            }

            sprites[6].color = new Color(0.294f, 0.286f, 0.549f);
        }
    }
}
