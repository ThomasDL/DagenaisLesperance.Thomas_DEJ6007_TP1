using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBattleInitializer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BattleManager.instance.AddCharacters(0, 1);
        BattleManager.instance.AddEnemy(0, new Vector3(5, 0, 0));
        BattleManager.instance.ChangeBattleState(BattleManager.BattleState.CharacterSelection);
    }
}
