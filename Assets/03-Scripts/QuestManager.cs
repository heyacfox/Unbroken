using System.Collections;
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

    public AirConsoleHandler airConsoleHandler;

    private void Start()
    {
        
        StartCoroutine(initialSetupRoutine());
    }

    IEnumerator initialSetupRoutine()
    {
        continueStacks = ContinueStacks.instance.currentStacks;
        rpgManager.PlayerHealthSlider.maxValue = rpgManager.playerHealthMax;
        rpgManager.PlayerHealthSlider.value = rpgManager.PlayerHealthSlider.maxValue;
        winLoseNotif.gameObject.SetActive(true);
        winLoseNotif.text = "Look at your phone to see which part you are, and get ready!";
        StartCoroutine(transitionToNextBattle());
        yield return new WaitForSeconds(timeToTransition);
        airConsoleHandler.FakeStart();
        winLoseNotif.gameObject.SetActive(false);
        

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
        enemySpawnPosition.color = new Color(1, 1, 1, 1);
        //StartCoroutine(fadeInEnemySprite());
        rpgManager.EnemyHealthSlider.maxValue = rpgManager.monsterHealthMax;
        rpgManager.EnemyHealthSlider.value = 0;


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
            //enemySpawnPosition.sprite = null;
            StartCoroutine(fadeEnemySprite());
            StartCoroutine(transitionToNextBattle());
        } 
    }
    
    IEnumerator fadeEnemySprite()
    {
        
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            enemySpawnPosition.color = new Color(1, 1, 1, i);
            yield return null;
        }
        enemySpawnPosition.sprite = null;
        enemySpawnPosition.color = new Color(1, 1, 1, 1);
    }

    IEnumerator fadeInEnemySprite()
    {

        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            enemySpawnPosition.color = new Color(1, 1, 1, 1);
            yield return null;
        }
        enemySpawnPosition.sprite = null;
        enemySpawnPosition.color = new Color(1, 1, 1, 1);
    }





}
