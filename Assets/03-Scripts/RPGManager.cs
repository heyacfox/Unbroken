using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGManager : MonoBehaviour
{
    public int playerHealthMax;
    public int playerHealth;
    public int monsterHealthMax;
    public int monsterHealth;

    public int numberOfBuffs;
    public int numberOfDebuffs;
    public RhythmManager rhythmManager;

    private void Start()
    {
        startBattle();
    }

    private void startBattle()
    {
        playerHealth = playerHealthMax;
        monsterHealth = 0;
    }

    public void playerHitOccurred(int extraHeal)
    {
        //eextra heal is for the "perfect" condition.
        monsterHealth = monsterHealth + numberOfBuffs + extraHeal;
        numberOfBuffs = 0;
        checkGameEndCondition();
    }

    public void playerMissOccurred()
    {
        playerHealth = playerHealth - numberOfDebuffs - 1;
        numberOfDebuffs = 0;
        checkGameEndCondition();
    }

    private void checkGameEndCondition()
    {
        if (playerHealth <= 0)
        {
            Debug.Log("You Lose!");
            rhythmManager.gameLost();
        } else if (monsterHealth >= monsterHealthMax)
        {
            Debug.Log("You win!");
            rhythmManager.gameWon();
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
