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
    public TextMeshProUGUI lifeText;
    public GameObject gameOverObject;
    public GameObject youWonObject;

    public bool isPlayerActive = true;

    private const int maxHP = 3;
    private int currentHP;

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
        currentHP = maxHP;
    }

    public void ReloadScene()
    {
        currentHP = maxHP;
        isPlayerActive = true;
        gameOverObject.SetActive(false);
        SceneManager.LoadScene(0);
    }
    public void MakePlayerActive()
    {
        isPlayerActive = true;
    }
    public void MakePlayerInactive()
    {
        RemoveInteractionPrompt();
        isPlayerActive = false;
    }
    public void CreateInteractionPrompt(string prompt)
    {
        interactionPrompt.gameObject.SetActive(true);
        interactionPrompt.text = prompt;
    }
    public void RemoveInteractionPrompt()
    {
        interactionPrompt.gameObject.SetActive(false);
    }
    void ShowLifePoints()
    {
        lifeText.text = "HP = " + currentHP;
    }
    public void ChangeLifePoints(int modifier)
    {
        currentHP = Mathf.Clamp(currentHP + modifier, 0, maxHP);
        ShowLifePoints();
        if(currentHP == 0) PlayerDead();
    }
    void PlayerDead()
    {
        isPlayerActive = false;
        gameOverObject.SetActive(true);
    }
    public void PlayerWon()
    {
        isPlayerActive = false;
        youWonObject.SetActive(true);
    }
}
