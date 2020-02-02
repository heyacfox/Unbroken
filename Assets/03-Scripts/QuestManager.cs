﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public int continueStacks;

    public Text winLoseNotif;


    private void Start()
    {
        continueStacks = ContinueStacks.instance.currentStacks;
        rpgManager.PlayerHealthSlider.maxValue = rpgManager.playerHealthMax;
        rpgManager.PlayerHealthSlider.value = rpgManager.PlayerHealthSlider.maxValue;
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
        winLoseNotif.gameObject.SetActive(false);
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
        rpgManager.monsterHealthMax = character.enemyHPMax * continueStacks;
        rhythmManager.startGenerationCoroutine();
        /*
        pianoHandler.startGenerationCoroutine();
        drumHandler.startGenerationCoroutine();
        bassHandler.startGenerationCoroutine();
        trumpetHandler.startGenerationCoroutine();
        */
        enemySpawnPosition.sprite = character.enemySprite;
        rpgManager.EnemyHealthSlider.maxValue = character.enemyHPMax;
        

    }

    public void loseCurrentBattle()
    {
        currentBattleIndex = 0;
        winLoseNotif.gameObject.SetActive(true);
        winLoseNotif.text = "Oh no, they beat you! Get your team ready and try again!";
        enemySpawnPosition.sprite = null;
        //if you lose, you restart from beginning and get +10% health
        rpgManager.playerHealthMax = rpgManager.playerHealthMax + (int)(rpgManager.playerHealthMax / 10);
        rpgManager.PlayerHealthSlider.maxValue = rpgManager.playerHealthMax;
        rpgManager.PlayerHealthSlider.value = rpgManager.PlayerHealthSlider.maxValue;
        rpgManager.playerHealth = rpgManager.playerHealthMax;
        StartCoroutine(transitionToNextBattle());

    }

    public void winCurrentBattle()
    {
        if (currentBattleIndex == 2)
        {
            SceneManager.LoadScene("Continue-Retire");
        } else 
        {
            winLoseNotif.gameObject.SetActive(true);
            winLoseNotif.text = "You healed them! Get ready for the next monster!";
            currentBattleIndex++;
            enemySpawnPosition.sprite = null;
            StartCoroutine(transitionToNextBattle());
        } 
    }




    
}
