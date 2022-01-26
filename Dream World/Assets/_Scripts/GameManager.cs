using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Dictionary<string, int> npcDialoguesNodes = new Dictionary<string, int>();
    public List<string> eventNodes = new List<string>();

    public TextMeshProUGUI interactionPrompt;

    public bool playerActive = true;

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

    public void AddEventToList(string eventName)
    {
        if (!eventNodes.Contains(eventName))
        {
            eventNodes.Add(eventName);
        }
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
        interactionPrompt.text = "";
        playerActive = false;
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
        playerActive = true;
    }
}
