using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class MoveDialogueBoxTo : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    void MoveDialogueBoxToCharacter(string talker, string listener)
    {
        GameObject talkerObject = GameObject.Find(talker);
        GameObject listenerObject = GameObject.Find(listener);
        float cameraOffsetDown = 1.5f;
        float cameraOffsetUp = 2.2f;
        if (talkerObject != null && listenerObject != null)
        {
            if (talkerObject.transform.position.y > listenerObject.transform.position.y)
            {
                dialogueRunner.transform.position = talkerObject.transform.position + Vector3.up * cameraOffsetUp;
            }
            else
            {
                dialogueRunner.transform.position = talkerObject.transform.position + Vector3.down * cameraOffsetDown;
            }
        }
    }
}
