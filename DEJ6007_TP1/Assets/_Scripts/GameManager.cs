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
    // Le GameManager contr�le les �l�ments de UI. 
    // C'est probablement pas une bonne structure pour un plus gros projet mais �a marche dans ce cas.
    public TextMeshProUGUI interactionPrompt;
    public TextMeshProUGUI hpText;
    public GameObject gameOverObject;

    // Cette constante repr�sente la limite inf�rieure du tableau.
    public const float bottomMapLimit = -10f;

    // Le GameManager est responsable de garder en m�moire les PV du joueur. �a aurait pu �tre le script 
    // du personnage, mais comme les �l�ments de UI sont controll�s par le GM 
    // �a me semblait plus simple. 
    private const int maxHP = 3;
    public int currentHP { get; private set; }

    // Si jamais le PlayerController doit �tre d�sactiv� (ou r�activ�), n'importe quelle classe
    // peut se servir du GM et de cet �v�nement pour facilement le faire.
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
    // M�thode utilis�e pour recommencer le jeu.
    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
        currentHP = maxHP;
        ShowHP();
        playerActivationChange?.Invoke(true);
        gameOverObject.SetActive(false);
    }
    // �a fait ce que �a dit.
    public void MakePlayerActive()
    {
        playerActivationChange?.Invoke(true);
    }
    // �a fait ce que �a dit.
    public void MakePlayerInactive()
    {
        RemoveInteractionPrompt();
        playerActivationChange?.Invoke(false);
    }
    // �a affiche un message au bas de l'�cran.
    public void CreateInteractionPrompt(string prompt)
    {
        interactionPrompt.gameObject.SetActive(true);
        interactionPrompt.text = prompt;
    }
    // �a enl�ve le message au bas de l'�cran.
    public void RemoveInteractionPrompt()
    {
        interactionPrompt.gameObject.SetActive(false);
    }
    // �a affiche les PV du joueur.
    void ShowHP()
    {
        hpText.text = "PV = " + currentHP;
    }
    // �a change les PV du joueur.
    public void ChangePlayerHP(int modifier)
    {
        currentHP = Mathf.Clamp(currentHP + modifier, 0, maxHP);
        ShowHP();
        if(currentHP == 0) EndoftheGame("Caliss");
    }
    // Cette m�thode met fin � la partie et affiche un texte personnalis�.
    public void EndoftheGame(string endText)
    {
        playerActivationChange?.Invoke(false);
        gameOverObject.SetActive(true);
        gameOverObject.GetComponentInChildren<TextMeshProUGUI>().text = endText;
    }
}
