using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance;
    public PauseMenu pauseMenu;
    public ScoreScreen scoreScreen;
    public InGameUI inGameUI;
    public bool active = true;
    public static float Score
    {
        get
        {
            if (Instance)
            {
                return Instance.score;
            }
            return -1f;
        }
        set
        {
            if (Instance)
            {
                Instance.score = value;
                Instance.inGameUI.UpdateScore(value);
            }
        }
    }
    public float score;
    int step = 3;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        pauseMenu.gameObject.SetActive(false);
        scoreScreen.gameObject.SetActive(false);
        inGameUI.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (Controls.Pause)
            {
                Pause();
            }
            Score += Time.deltaTime * Random.value * 100;
        }
    }

    public void Pause()
    {
        pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);
    }

    void Next()
    {
        if (step > 0)
        {
            step--;
            if (step == 0)
            {
                inGameUI.EndGame(true);
                scoreScreen.EndGame(true);
                pauseMenu.gameObject.SetActive(false);
            }
        }
    }
}