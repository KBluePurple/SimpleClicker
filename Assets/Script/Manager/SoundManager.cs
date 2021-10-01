using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundEffectType
{
    Tick
}

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    GameObject soundEffectPrefab = null;

    [SerializeField]
    Transform effects = null;

    PoolManager soundEffectPool = null;

    void Start()
    {
        soundEffectPool = new PoolManager(soundEffectPrefab);
    }

    public void PlaySoundEffect(SoundEffectType type)
    {
        switch (type)
        {
            case SoundEffectType.Tick:
                var soundEffectObject = soundEffectPool.GetObject();
                soundEffectObject.transform.SetParent(effects);
                soundEffectObject.GetComponent<AudioSource>().Play();
                StartCoroutine(stopSound(soundEffectObject.GetComponent<AudioSource>(), .3f));
                
                break;
        }
    }

    IEnumerator stopSound(AudioSource sound, float time)
    {
        yield return new WaitForSeconds(time);
        sound.Stop();
        sound.gameObject.SetActive(false);
        soundEffectPool.PutObject(sound.gameObject);
    }
}
