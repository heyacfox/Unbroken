using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RPGManager : MonoBehaviour
{
    public int playerHealthMax;
    public int playerHealth;
    public int monsterHealthMax;
    public int monsterHealth;
    public Slider EnemyHealthSlider;
    public Slider PlayerHealthSlider;

    public int numberOfBuffs;
    public int numberOfDebuffs;
    public RhythmManager rhythmManager;
    public QuestManager questManager;
    public Transform creaturePosition;
    public Transform mainCharacterPosition;
    public GameObject healParticlePrefab;
    public float increaseParticleSizePerBuffAmount = 0.001f;
    public AudioSource healMonsterSound;

    public GameObject healAnimationObject;
    public GameObject hitAnimationObject;
    

    private void Start()
    {
        startBattle();
    }

    private void startBattle()
    {
        playerHealth = playerHealthMax;
        monsterHealth = 0;
        EnemyHealthSlider.value = monsterHealth;
        StartCoroutine(doPlayerIsHitEffects());
    }

    
        public void playerHitOccurred(int extraHeal)
    {
        //eextra heal is for the "perfect" condition.
        monsterHealth = monsterHealth + rhythmManager.getAllBuffs() + extraHeal;
        numberOfBuffs = 0;
        EnemyHealthSlider.value = monsterHealth;
        StartCoroutine(doFriendIsHealedEffects());
        checkGameEndCondition();
    }

    IEnumerator doPlayerIsHitEffects()
    {
        /*
        GameObject hitGenerated = Instantiate(hitAnimationObject, creaturePosition);
        float newScale = hitGenerated.transform.localScale.x + (increaseParticleSizePerBuffAmount * rhythmManager.getAllDebuffs());
        hitGenerated.transform.localScale = new Vector3(newScale, newScale, newScale);
        */
        yield return new WaitForSeconds(1f);
    }   

    IEnumerator doFriendIsHealedEffects()
    {
        /*
        GameObject particleGenerated = Instantiate(healParticlePrefab, creaturePosition);
        float newScale = particleGenerated.transform.localScale.x + (increaseParticleSizePerBuffAmount * rhythmManager.getAllBuffs());
        particleGenerated.transform.localScale = new Vector3(newScale, newScale, newScale);
        */
        GameObject healGenerated = Instantiate(healAnimationObject, creaturePosition);
        float newScale = healGenerated.transform.localScale.x + (increaseParticleSizePerBuffAmount * rhythmManager.getAllBuffs());
        healGenerated.transform.position = new Vector3(healGenerated.transform.position.x, healGenerated.transform.position.x + 3f);
        healGenerated.transform.localScale = new Vector3(newScale, newScale, newScale);
        
        yield return new WaitForSeconds(1f);
    }

    public void playerMissOccurred()
    {
        playerHealth = playerHealth - rhythmManager.getAllDebuffs() - 1;
        numberOfDebuffs = 0;
        PlayerHealthSlider.value = playerHealth;
        checkGameEndCondition();
    }

    private void checkGameEndCondition()
    {
        if (playerHealth <= 0)
        {
            Debug.Log("You Lose!");
            rhythmManager.gameLost();

            questManager.loseCurrentBattle();
        } else if (monsterHealth >= monsterHealthMax)
        {
            Debug.Log("You win!");
            rhythmManager.gameWon();
            healMonsterSound.Play();
            questManager.winCurrentBattle();
        }
        
    }

    public void addBuffStack()
    {
        numberOfBuffs++;
    }

    public void addDebuffStack()
    {
        numberOfDebuffs++;
    }

}
