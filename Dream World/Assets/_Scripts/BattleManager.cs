using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public GameObject[] battleCharacterPrefabs;
    public Dictionary<string, GameObject> battleEnemyPrefabs = new Dictionary<string, GameObject>();

    private BattleCharacter topCharacter;
    private BattleCharacter bottomCharacter;

    public Transform topCharacterSpot;
    public Transform middleCharacterSpot;
    public Transform bottomCharacterSpot;

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
    public void AddEnemy(string name, Vector3 position)
    {

    }
    public void StartBattle (int character)
    {
        topCharacter = Instantiate(battleCharacterPrefabs[character], middleCharacterSpot).GetComponent<BattleCharacter>();
    }
    public void StartBattle(int firstCharacter, int secondCharacter)
    {
        topCharacter = Instantiate(battleCharacterPrefabs[firstCharacter], topCharacterSpot).GetComponent<BattleCharacter>();
        bottomCharacter = Instantiate(battleCharacterPrefabs[secondCharacter], bottomCharacterSpot).GetComponent<BattleCharacter>();
    }
}
