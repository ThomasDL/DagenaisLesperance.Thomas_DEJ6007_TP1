using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TestCutscene : MonoBehaviour
{
    public PlayableDirector testCutsceneDirector;
    void Start()
    {
        if (!GameManager.instance.eventNodes.Contains("testCutscene"))
        {
            testCutsceneDirector.Play();
        }
    }

    public void TestCutsceneDialogue()
    {
        GameManager.instance.CallDialogueNode("TestCutscene1");
    }
}
