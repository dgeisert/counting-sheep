using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreScreen : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameObject victoryDisplay;
    public GameObject defeatDisplay;
    public void EndGame(bool victory = false)
    {
        gameObject.SetActive(true);
    }

    public void Next()
    {
        SceneChanger.LoadNext();
    }
    public void Menu()
    {
        SceneChanger.LoadScene(Scenes.MainMenu);
    }
}