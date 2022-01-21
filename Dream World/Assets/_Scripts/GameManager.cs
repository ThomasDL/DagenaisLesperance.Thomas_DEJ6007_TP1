using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool playerActive = true;
    public Dictionary<string, int> npcDialoguesNodes = new Dictionary<string, int>();
    public List<string> eventNodes = new List<string>();
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
    public void CallDialogueNode(string node)
    {
        dialogueRunner.StartDialogue(node);
    }
    // Returns the correct dialogue node for a NPC
    // If this is the first time talking to the NPC, returns node1
    public void NPCDialogue(string NPC)
    {
        if (!npcDialoguesNodes.ContainsKey(NPC))
        {
            dialogueRunner.StartDialogue(NPC + "Node" + 1);
        }
        else
        {
            dialogueRunner.StartDialogue(NPC + "Node" + npcDialoguesNodes[NPC]);
        }
    }
    // Tracks the progression of the player related to conversations
    [YarnCommand("AddDialogueNodeToDictionary")]
    public void AddDialogueNodeToDictionary(string name, int node)
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
    public void AddEventToList(string eventName)
    {
        if (!eventNodes.Contains(eventName))
        {
            eventNodes.Add(eventName);
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
}
