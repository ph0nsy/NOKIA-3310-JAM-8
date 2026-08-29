using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CinematicSequence : MonoBehaviour
{
    public enum CinematicType : int { None = 0, Intro = 1, Win = 2, Lose = 3 };

    public List<Sprite> CinematicFrames;
    public List<Vector2> IntroCinematic;
    public List<Vector2> LoseCinematic;
    public List<Vector2> WinCinematic;

    private Image img;
    private float currentTimer = 0f;
    private List<Vector2> activeCinematic = new List<Vector2>();
    private bool lastFrame = false;
    CinematicType type = CinematicType.None;

    public event Action<int> OnCinematicEnd;
    public static CinematicSequence Instance {get; private set;}

    void Awake() {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        img = GetComponent<Image>();
        img.enabled = false;
    }

    void Update()
    {
        if (!lastFrame && activeCinematic.Count <= 0) { return; }
        
        if (currentTimer <= 0)
        {
            if(lastFrame)
            {
                StopCinematic();
                return;
            }
            Vector2 currentBeat = activeCinematic[0];
            img.sprite = CinematicFrames[(int) currentBeat.x];
            currentTimer = currentBeat.y;
            activeCinematic.RemoveAt(0);
            lastFrame = activeCinematic.Count <= 0;
        }

        currentTimer -= Time.deltaTime;
    }

    public void PlayCinematic(int _t)
    {
        img.enabled = true;
        CinematicType t = (CinematicType)_t;
        switch (t)
        {
            case CinematicType.Intro:
                type = t;
                activeCinematic = new List<Vector2>(IntroCinematic);
                break;
            case CinematicType.Win:
                type = t;
                activeCinematic = new List<Vector2>(WinCinematic);
                break;
            case CinematicType.Lose:
                type = t;
                activeCinematic = new List<Vector2>(LoseCinematic);
                break;
            default:
                type = CinematicType.None;
                if (activeCinematic.Count <= 0) { activeCinematic.Clear(); }
                break;
        }
    }

    public void StopCinematic()
    {
        OnCinematicEnd?.Invoke((int) type);

        img.enabled = false;
        lastFrame = false;
        type = 0;
        
        if (activeCinematic.Count <= 0) { activeCinematic.Clear(); }
    }
}
