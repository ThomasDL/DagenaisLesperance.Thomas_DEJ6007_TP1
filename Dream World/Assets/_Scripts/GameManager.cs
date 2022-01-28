using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI interactionPrompt;

    public bool isPlayerActive = true;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void LoadNewScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void MakePlayerActive()
    {
        StartCoroutine(MakePlayerActiveCoroutine());
    }
    public void MakePlayerInactive()
    {
        RemoveInteractionPrompt();
        isPlayerActive = false;
    }
    public void CreateInteractionPrompt(string prompt)
    {
        interactionPrompt.text = prompt;
    }
    public void RemoveInteractionPrompt()
    {
        interactionPrompt.text = "";
    }
    IEnumerator MakePlayerActiveCoroutine()
    {
        yield return null;
        isPlayerActive = true;
    }
}
