using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This class is to make sure the player can't spam the continue button during dialogues.
public class SubmitListener : MonoBehaviour
{
    public Button thisButton;

    private void Start()
    {
        thisButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(thisButton.IsInteractable() && Input.GetKeyDown(KeyCode.E))
        {
            thisButton.interactable = false;
            StopCoroutine(WaitBeforeInput());
            thisButton.onClick.Invoke();
        }
    }
    private void OnEnable()
    {
        thisButton.interactable = false;
        StartCoroutine(WaitBeforeInput());
    }
    IEnumerator WaitBeforeInput()
    {
        yield return new WaitForSeconds(0.3f);
        thisButton.interactable = true;
    }
    private void OnDisable()
    {
        thisButton.interactable = false;
    }
}
