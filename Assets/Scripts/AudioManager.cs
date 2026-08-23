using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESourceSFX
{
    Button,
    Slash,
    Gun,
    Parry,
    Hurt,
    Surprised
}

public enum ESourceBGM
{
    Menu,
    Level
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource adudiSourceSFX;
    public AudioSource adudiSourceBGM;

    [Header("Samurai SFX")]
    public AudioClip hurtSFX;
    public AudioClip slashSFX;
    public AudioClip parrySFX;

    [Header("Cowboy SFX")]
    public AudioClip gunSFX;
    public AudioClip surprisedSFX;


    [Header("Level BGM")]
    public AudioClip levelBGM;

    [Header("User Interface")]
    public AudioClip pressButtonSFX;
    public AudioClip menuBGM;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Can only play one at a time
    public void PlaySFX(ESourceSFX _src)
    {
        adudiSourceSFX.Stop();
        switch(_src)
        {
            case ESourceSFX.Button:
                adudiSourceSFX.PlayOneShot(pressButtonSFX);
                break;
            case ESourceSFX.Hurt:
                adudiSourceSFX.PlayOneShot(hurtSFX);
                break;
            case ESourceSFX.Slash:
                adudiSourceSFX.PlayOneShot(slashSFX);
                break;
            case ESourceSFX.Parry:
                adudiSourceSFX.PlayOneShot(parrySFX);
                break;
                
            case ESourceSFX.Gun:
                adudiSourceSFX.PlayOneShot(gunSFX);
                break;
            case ESourceSFX.Surprised:
                adudiSourceSFX.PlayOneShot(surprisedSFX);
                break;
            default: return;
        }
    }

    public void PlayBGM(ESourceBGM _src)
    {
        adudiSourceBGM.Stop();
        switch (_src)
        {
            case ESourceBGM.Level:
                adudiSourceBGM.clip = levelBGM;
                break;
            case ESourceBGM.Menu:
                adudiSourceBGM.clip = menuBGM;
                break;
            default: return;
        }
        adudiSourceBGM.Play();

    }

    public void SetVolumeBGM(float _value)
    {
        adudiSourceBGM.outputAudioMixerGroup.audioMixer.SetFloat("BGM_Volume", Mathf.Log10(_value) * 20);
    }

    public void SetVolumeSFX(float _value)
    {
        adudiSourceSFX.outputAudioMixerGroup.audioMixer.SetFloat("SFX_Volume", Mathf.Log10(_value) * 20);
    }
}