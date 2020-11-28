using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
{
    Preloader,
    MainMenu,
    Game
}

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance;
    public static void LoadScene(Scenes scene)
    {
        Instance.loadScene((int) scene);
    }
    public static void LoadScene(int scene)
    {
        Instance.loadScene(scene);
    }
    public static void LoadNext()
    {
        int scene = SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1 ?
            1 :
            SceneManager.GetActiveScene().buildIndex + 1;
        LoadScene(scene);
    }
    public static void Restart()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public UnityEngine.UI.Image topLid, bottomLid, cover;
    public Vector3 topPos, bottomPos;
    public void Start()
    {
        topPos = topLid.transform.position;
        bottomPos = bottomLid.transform.position;
        Instance = this;
        loadScene((int) Scenes.MainMenu);
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(LateSet());
    }
    IEnumerator LateSet()
    {
        yield return null;
        yield return null;
        yield return null;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        StartCoroutine(DoLoadIn());
    }
    bool isLoading = false;

    public void loadScene(int scene)
    {
        if (!isLoading)
        {
            isLoading = true;
            StartCoroutine(DoLoadScene(scene));
        }
    }
    IEnumerator DoLoadIn()
    {
        topLid.gameObject.SetActive(false);
        bottomLid.gameObject.SetActive(false);
        cover.gameObject.SetActive(true);
        cover.color = Color.black;
        while (cover.color.a > 0)
        {
            Color c = cover.color;
            c.a -= 0.005f;
            cover.color = c;
            yield return null;
        }
        cover.gameObject.SetActive(false);
        isLoading = false;
    }
    IEnumerator DoLoadScene(int scene)
    {
        topLid.gameObject.SetActive(true);
        bottomLid.gameObject.SetActive(true);
        topLid.transform.position = topPos;
        bottomLid.transform.position = bottomPos;
        topLid.color = new Color(0, 0, 0, 0.5f);
        bottomLid.color = new Color(0, 0, 0, 0.5f);
        float startTime = Time.time;
        while (topLid.color.a < 1)
        {
            topLid.transform.position -= Vector3.up * 3;
            bottomLid.transform.position += Vector3.up * 3;
            Color c = topLid.color;
            c.a += 0.0025f;
            topLid.color = c;
            bottomLid.color = c;
            yield return null;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }

}