using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public GameObject[] battleCharacterPrefabs;
    public GameObject[] battleEnemyPrefabs;

    private BattleCharacter[] character = new BattleCharacter[2];
    public BattleCharacter selectedCharacter;

    public Transform topCharacterSpot;
    public Transform bottomCharacterSpot;

    public List<BattleEnemy> enemyList = new List<BattleEnemy>();
    public enum BattleState { CharacterSelection, CharacterMenu, EnemyAttack, Dialogue }
    public BattleState currentState = BattleState.CharacterSelection;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    public void AddEnemy(int enemy, Vector3 position)
    {
        enemyList.Add(Instantiate(battleEnemyPrefabs[enemy], position, Quaternion.identity).GetComponent<BattleEnemy>());
    }
    public void AddCharacters(int firstCharacter, int secondCharacter)
    {
        character[0] = Instantiate(battleCharacterPrefabs[firstCharacter], topCharacterSpot).GetComponent<BattleCharacter>();
        character[1] = Instantiate(battleCharacterPrefabs[secondCharacter], bottomCharacterSpot).GetComponent<BattleCharacter>();
        selectedCharacter = character[0];
    }
    public void ChangeBattleState(BattleState newBattleState)
    {
        switch (newBattleState)
        {
            case BattleState.CharacterSelection:
                selectedCharacter.ShowSelectionSprite(true);
                break;
            case BattleState.CharacterMenu:
                selectedCharacter.ShowCommandMenu(true);
                break;
            case BattleState.EnemyAttack:
                break;
            case BattleState.Dialogue:
                break;
            default:
                break;
        }
        currentState = newBattleState;
    }
    private void Update()
    {
        switch (currentState)
        {
            case BattleState.CharacterSelection:
                if (Input.GetButtonDown("Submit"))
                {
                    ChangeBattleState(BattleState.CharacterMenu);
                }
                else if (Input.GetButtonDown("Vertical"))
                {
                    SwitchSelectedCharacter();
                } 
                break;
            case BattleState.CharacterMenu:
                if (Input.GetButtonDown("Submit"))
                {
                    selectedCharacter.ExecuteMenuAction();
                }
                if (Input.GetButtonDown("Cancel"))
                {
                    if (selectedCharacter.commandMenu.activeInHierarchy)
                    {
                        ChangeBattleState(BattleState.CharacterSelection);
                        selectedCharacter.ShowCommandMenu(false);
                    }
                    else
                    {
                        selectedCharacter.ReturnToCommandMenu();
                    }
                }
                break;
            case BattleState.EnemyAttack:
                break;
            case BattleState.Dialogue:
                break;
            default:
                break;
        }
    }
    void SwitchSelectedCharacter()
    {
        //Selected character should always be 0 in the array.
        selectedCharacter.ShowSelectionSprite(false);
        character[0] = character[1];
        character[1] = selectedCharacter;
        selectedCharacter = character[0];
        selectedCharacter.ShowSelectionSprite(true);
    }
}
