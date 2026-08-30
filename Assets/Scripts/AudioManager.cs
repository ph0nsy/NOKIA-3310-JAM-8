using System.Collections;
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
    Intro,
    Level,
    Win,
    Lose
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource adudiSourceSFX;
    public AudioSource adudiSourceBGM;

    [Header("Samurai SFX")]
    public AudioClip pressButtonSFX;
    public AudioClip hurtSFX;
    public AudioClip slashSFX;
    public AudioClip parrySFX;

    [Header("Cowboy SFX")]
    public AudioClip gunSFX;
    public AudioClip surprisedSFX;

    [Header("Level BGM")]
    public AudioClip levelBGM;
    public AudioClip introBGM;
    public AudioClip winBGM;
    public AudioClip loseBGM;

    private Coroutine sfxCoroutine;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PlaySFX(ESourceSFX src)
    {
        AudioClip clip = GetSFXClip(src);

        if (clip == null)
            return;

        // Stop the currently playing SFX, if any.
        if (sfxCoroutine != null)
        {
            StopCoroutine(sfxCoroutine);
            sfxCoroutine = null;
        }

        adudiSourceSFX.Stop();

        // Play the new SFX and temporarily stop the BGM.
        sfxCoroutine = StartCoroutine(PlaySFXCoroutine(clip));
    }

    private IEnumerator PlaySFXCoroutine(AudioClip clip)
    {
        // Remember whether BGM was playing.
        bool wasBGMPlaying = adudiSourceBGM.isPlaying;

        // Stop BGM.
        if (wasBGMPlaying)
            adudiSourceBGM.Pause();

        // Play SFX.
        adudiSourceSFX.clip = clip;
        adudiSourceSFX.Play();

        // Wait until SFX finishes.
        yield return new WaitForSeconds(clip.length);

        // Stop SFX.
        adudiSourceSFX.Stop();

        // Resume BGM.
        if (wasBGMPlaying)
            adudiSourceBGM.UnPause();

        sfxCoroutine = null;
    }

    private AudioClip GetSFXClip(ESourceSFX src)
    {
        switch (src)
        {
            case ESourceSFX.Button:
                return pressButtonSFX;

            case ESourceSFX.Hurt:
                return hurtSFX;

            case ESourceSFX.Slash:
                return slashSFX;

            case ESourceSFX.Parry:
                return parrySFX;

            case ESourceSFX.Gun:
                return gunSFX;

            case ESourceSFX.Surprised:
                return surprisedSFX;

            default:
                return null;
        }
    }

    public void PlayBGM(ESourceBGM src)
    {
        // Don't interrupt an SFX.
        if (adudiSourceSFX.isPlaying)
            return;

        adudiSourceBGM.Stop();

        switch (src)
        {
            case ESourceBGM.Level:
                adudiSourceBGM.clip = levelBGM;
                break;

            case ESourceBGM.Intro:
                adudiSourceBGM.clip = introBGM;
                break;

            case ESourceBGM.Win:
                adudiSourceBGM.clip = winBGM;
                break;

            case ESourceBGM.Lose:
                adudiSourceBGM.clip = loseBGM;
                break;

            default:
                return;
        }

        adudiSourceBGM.Play();
    }

    public void SetVolumeBGM(float value)
    {
        if (value <= 0)
            value = 0.0001f;

        adudiSourceBGM.outputAudioMixerGroup.audioMixer.SetFloat(
            "BGM_Volume",
            Mathf.Log10(value) * 20
        );
    }

    public void SetVolumeSFX(float value)
    {
        if (value <= 0)
            value = 0.0001f;

        adudiSourceSFX.outputAudioMixerGroup.audioMixer.SetFloat(
            "SFX_Volume",
            Mathf.Log10(value) * 20
        );
    }
}
