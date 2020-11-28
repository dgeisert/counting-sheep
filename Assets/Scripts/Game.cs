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
    List<FencedArea> fencedAreas;
    public float score;
    int step = 3;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        active = true;
        pauseMenu.gameObject.SetActive(false);
        scoreScreen.gameObject.SetActive(false);
        inGameUI.gameObject.SetActive(true);
        fencedAreas = new List<FencedArea>();
        for (int i = 0; i < transform.childCount; i++)
        {
            fencedAreas.Add(transform.GetChild(i).GetComponent<FencedArea>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool win = true;
        foreach (var fa in fencedAreas)
        {
            if (!fa.full)
            {
                win = false;
                break;
            }
        }
        if (active && win)
        {
            active = false;
            Dog.Instance.Win();
            inGameUI.EndGame(true);
            scoreScreen.EndGame(true);
            pauseMenu.gameObject.SetActive(false);
        }
    }
}