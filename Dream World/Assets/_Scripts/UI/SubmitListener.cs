using UnityEngine;
using UnityEngine.UI;

// This class is to make sure the player can't spam the continue button during dialogues.
public class SubmitListener : MonoBehaviour
{
    public Button thisButton;

    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            thisButton.onClick.Invoke();
        }
    }
}
