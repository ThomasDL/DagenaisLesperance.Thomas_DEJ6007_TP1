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
    public TextMeshProUGUI hpText;
    public GameObject gameOverObject;

    public const float bottomMapLimit = -10f;

    private const int maxHP = 3;
    private int currentHP;

    public delegate void PlayerActivationChange(bool isActive);
    public static event PlayerActivationChange playerActivationChange;

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
        ShowHP();
        playerActivationChange?.Invoke(true);
        gameOverObject.SetActive(false);
        SceneManager.LoadScene(0);
    }
    public void MakePlayerActive()
    {
        playerActivationChange?.Invoke(true);
    }
    public void MakePlayerInactive()
    {
        RemoveInteractionPrompt();
        playerActivationChange?.Invoke(false);
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
    void ShowHP()
    {
        hpText.text = "HP = " + currentHP;
    }
    public void ChangePlayerHP(int modifier)
    {
        currentHP = Mathf.Clamp(currentHP + modifier, 0, maxHP);
        ShowHP();
        if(currentHP == 0) EndoftheGame("Game Over");
    }
    public void EndoftheGame(string endText)
    {
        playerActivationChange?.Invoke(false);
        gameOverObject.SetActive(true);
        gameOverObject.GetComponentInChildren<TextMeshProUGUI>().text = endText;
    }
}
