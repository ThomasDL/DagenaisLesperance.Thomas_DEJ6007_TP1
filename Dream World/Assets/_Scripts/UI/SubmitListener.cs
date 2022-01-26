using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This class is to make sure the player can't spam the continue button during dialogues.
public class SubmitListener : MonoBehaviour
{
    Button thisButton;
    bool canContinue = false;

    void Start()
    {
        thisButton = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canContinue && Input.GetAxis("Submit") > 0.5f)
        {
            canContinue = false;
            thisButton.onClick.Invoke();
        }
    }
    private void OnEnable()
    {
        StartCoroutine(WaitBeforeInput());
    }
    IEnumerator WaitBeforeInput()
    {
        yield return new WaitForSeconds(0.7f);
        canContinue = true;
    }
}
