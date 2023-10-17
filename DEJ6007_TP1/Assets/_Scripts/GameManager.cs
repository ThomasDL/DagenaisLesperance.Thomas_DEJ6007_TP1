using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Singleton pattern
    public static GameManager instance;
    // Le GameManager contrôle les éléments de UI. 
    // C'est probablement pas une bonne structure pour un plus gros projet mais ça marche dans ce cas.
    public TextMeshProUGUI interactionPrompt;
    public TextMeshProUGUI hpText;
    public GameObject gameOverObject;

    // Cette constante représente la limite inférieure du tableau.
    public const float bottomMapLimit = -10f;

    // Le GameManager est responsable de garder en mémoire les PV du joueur. Ça aurait pu être le script 
    // du personnage, mais comme les éléments de UI sont controllés par le GM 
    // ça me semblait plus simple. 
    private const int maxHP = 3;
    public int currentHP { get; private set; }

    // Si jamais le PlayerController doit être désactivé (ou réactivé), n'importe quelle classe
    // peut se servir du GM et de cet événement pour facilement le faire.
    public delegate void PlayerActivationChange(bool isActive);
    public static event PlayerActivationChange playerActivationChange;

    void Awake()
    {
        //Singleton pattern
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
    // Méthode utilisée pour recommencer le jeu.
    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
        currentHP = maxHP;
        ShowHP();
        playerActivationChange?.Invoke(true);
        gameOverObject.SetActive(false);
    }
    // Ça fait ce que ça dit.
    public void MakePlayerActive()
    {
        playerActivationChange?.Invoke(true);
    }
    // Ça fait ce que ça dit.
    public void MakePlayerInactive()
    {
        RemoveInteractionPrompt();
        playerActivationChange?.Invoke(false);
    }
    // Ça affiche un message au bas de l'écran.
    public void CreateInteractionPrompt(string prompt)
    {
        interactionPrompt.gameObject.SetActive(true);
        interactionPrompt.text = prompt;
    }
    // Ça enlève le message au bas de l'écran.
    public void RemoveInteractionPrompt()
    {
        interactionPrompt.gameObject.SetActive(false);
    }
    // Ça affiche les PV du joueur.
    void ShowHP()
    {
        hpText.text = "PV = " + currentHP;
    }
    // Ça change les PV du joueur.
    public void ChangePlayerHP(int modifier)
    {
        currentHP = Mathf.Clamp(currentHP + modifier, 0, maxHP);
        ShowHP();
        if(currentHP == 0) EndoftheGame("Caliss");
    }
    // Cette méthode met fin à la partie et affiche un texte personnalisé.
    public void EndoftheGame(string endText)
    {
        playerActivationChange?.Invoke(false);
        gameOverObject.SetActive(true);
        gameOverObject.GetComponentInChildren<TextMeshProUGUI>().text = endText;
    }
}
