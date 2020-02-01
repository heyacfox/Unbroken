using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

    public GameObject enemy1Prefab;
    public GameObject enemy2Prefab;
    public GameObject enemy3Prefab;

    public SpriteRenderer enemySpawnPosition;

    public int currentBattleIndex = 0;

    public RhythmManager rhythmManager;
    public RPGManager rpgManager;

    public InstrumentRhythmHandler pianoHandler;
    public InstrumentRhythmHandler drumHandler;
    public InstrumentRhythmHandler bassHandler;
    public InstrumentRhythmHandler trumpetHandler;

    public float timeToTransition;

    private void Start()
    {
        StartCoroutine(transitionToNextBattle());
    }

    IEnumerator transitionToNextBattle()
    {
        yield return new WaitForSeconds(timeToTransition);
        startBattle();
    }

    public void startBattle()
    {
        Debug.Log("Battle Started");
        if (currentBattleIndex == 0)
        {
            startBattleWithEnemy(enemy1Prefab.GetComponent<EnemyCharacter>());
        }
        else if (currentBattleIndex == 1)
        {
            startBattleWithEnemy(enemy2Prefab.GetComponent<EnemyCharacter>());
        } else if (currentBattleIndex == 2)
        {
            startBattleWithEnemy(enemy3Prefab.GetComponent<EnemyCharacter>());
        }
    }

    public void startBattleWithEnemy(EnemyCharacter character)
    {
        rpgManager.monsterHealth = 0;
        rpgManager.monsterHealthMax = character.enemyHPMax;
        rhythmManager.startGenerationCoroutine();
        pianoHandler.startGenerationCoroutine();
        drumHandler.startGenerationCoroutine();
        bassHandler.startGenerationCoroutine();
        trumpetHandler.startGenerationCoroutine();
        enemySpawnPosition.sprite = character.enemySprite;


    }

    public void loseCurrentBattle()
    {
        currentBattleIndex = 0;
        //if you lose, you restart from beginning and get +10% health
        rpgManager.playerHealthMax = rpgManager.playerHealthMax + (int)(rpgManager.playerHealthMax / 10);
        rpgManager.playerHealth = rpgManager.playerHealthMax;
        StartCoroutine(transitionToNextBattle());

    }

    public void winCurrentBattle()
    {
        if (currentBattleIndex == 2)
        {
            Debug.Log("Go to continue/retire screen");
        } else 
        {
            currentBattleIndex++;
            StartCoroutine(transitionToNextBattle());
        } 
    }




    
}
