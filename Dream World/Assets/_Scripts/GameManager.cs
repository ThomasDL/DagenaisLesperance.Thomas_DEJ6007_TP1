using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool playerActive = true;
    public Dictionary<string, int> npcDialoguesNodes = new Dictionary<string, int>();
    public DialogueRunner dialogueRunner;

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
    }

    public string DialogueChoice(string NPC)
    {
        if (!npcDialoguesNodes.ContainsKey(NPC))
        {
            return NPC + "Node" + 1;
        }
        else
        {
            return NPC + "Node" + npcDialoguesNodes[NPC];
        }
    }
    [YarnCommand("AddEventToDictionary")]
    public void AddEventToDictionary(string name, int node)
    {
        if (!npcDialoguesNodes.ContainsKey(name))
        {
            npcDialoguesNodes.Add(name, node);
        }
        else
        {
            npcDialoguesNodes[name] = node;
        }
    }
    [YarnCommand("LoadNewScene")]
    public void LoadNewScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void MakePlayerActive()
    {
        playerActive = true;
    }
    public void MakePlayerInactive()
    {
        playerActive = false;
    }
    [YarnCommand("MoveDialogueBoxTo")]
    void MoveDialogueBoxTo(string talker, string listener)
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
    void MoveDialogueBoxTo(string talker)
    {
        GameObject talkerObject = GameObject.Find(talker);
        float cameraOffsetDown = 1.5f;
        if (talkerObject != null)
        {
            dialogueRunner.transform.position = talkerObject.transform.position + Vector3.down * cameraOffsetDown;
        }
    }
}
