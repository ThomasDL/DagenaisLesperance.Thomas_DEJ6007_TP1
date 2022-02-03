using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleCharacter : MonoBehaviour
{
    public GameObject selectionSprite;
    public GameObject commandMenu;
    public Button attackButton;
    public Button[] commandMenuButton;
    public virtual void Attack(int enemy)
    {
        Debug.Log("Attack!");
    }
    public void SelectItem()
    {

    }
    public void Defend()
    {
        Debug.Log("Defend!");
    }
    public void SwitchCharacter(int newCharacter)
    {

    }
    public void ShowSelectionSprite(bool isSelected)
    {
        if (isSelected)
        {
            selectionSprite.SetActive(true);
        } else
        {
            selectionSprite.SetActive(false);
        }
    }
    public void ShowCommandMenu(bool isOpen)
    {
        if (isOpen)
        {
            commandMenu.SetActive(true);
            attackButton.Select();

        }
        else
        {
            commandMenu.SetActive(false);
        }
    }
    public void ExecuteMenuAction()
    {
        foreach(Button button in commandMenuButton)
        {
            if(button == EventSystem.current.currentSelectedGameObject)
            {
                button.onClick.Invoke();
            }
        }
    }
    public void ReturnToCommandMenu()
    {

    }
    private void Update()
    {

    }
}
