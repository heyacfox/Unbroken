﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    public RPGManager rpgManager;
    public List<RhythmBeat> generatedP1Beats;

    public Transform rhythmTarget;
    public Transform rhythmSpawnLocation;

    public float nearnessCheckForBeat = 3f;
    public float accuracyRequirementGood = 1f;
    public float accuracyRequirementPerfect = 0.5f;
    public float rhythmBeatsSpeed = 0.5f;
    public float generateBeatEveryXSeconds = 2f;

    public int playerHP;
    public int enemyHealAmount;

    public Sprite beatUp;
    public Sprite beatDown;
    public Sprite beatLeft;
    public Sprite beatRight;

    public GameObject beatPrefab;
    public GameObject perfectIndicatorPrefab;
    public GameObject goodIndicatorPrefab;
    public GameObject missIndicatorPrefab;

    public InstrumentRhythmHandler pianoRhythmHandler;
    public InstrumentRhythmHandler drumsRhythmHandler;
    public InstrumentRhythmHandler bassRhythmHandler;
    public InstrumentRhythmHandler trumpetRhythmHandler;


    public void Start()
    {
        //StartCoroutine(generateBeatsInTime());
    }

    IEnumerator generateBeatsInTime()
    {
        pianoRhythmHandler.myAudioOrigin.Play();
        drumsRhythmHandler.myAudioOrigin.Play();
        bassRhythmHandler.myAudioOrigin.Play();
        trumpetRhythmHandler.myAudioOrigin.Play();
        while (true)
        {
            yield return new WaitForSeconds(generateBeatEveryXSeconds);
            generateBeat();
            //Debug.Log($"MainBeatGenerated at [{Time.realtimeSinceStartup}]");
            if (pianoRhythmHandler.numberofActivePlayers > 0)
            {
                pianoRhythmHandler.generateBeat();
            }
            if (drumsRhythmHandler.numberofActivePlayers > 0)
            {
                drumsRhythmHandler.generateBeat();
            }
            if (bassRhythmHandler.numberofActivePlayers > 0)
            {
                bassRhythmHandler.generateBeat();
            }
            if (trumpetRhythmHandler.numberofActivePlayers > 0)
            {
                trumpetRhythmHandler.generateBeat();
            }
        }
    }

    public void startGenerationCoroutine()
    {
        StartCoroutine(generateBeatsInTime());
    }

    public void gameWon()
    {
        StopAllCoroutines();
        destroyAllBeatsOnScreen();
        pianoRhythmHandler.gameWon();
        drumsRhythmHandler.gameWon();
        bassRhythmHandler.gameWon();
        trumpetRhythmHandler.gameWon();
        rhythmTarget.gameObject.GetComponent<SpriteRenderer>().sprite = null;
    }

    public void gameLost()
    {
        StopAllCoroutines();
        destroyAllBeatsOnScreen();
        pianoRhythmHandler.gameLost();
        drumsRhythmHandler.gameLost();
        bassRhythmHandler.gameLost();
        trumpetRhythmHandler.gameLost();
        rhythmTarget.gameObject.GetComponent<SpriteRenderer>().sprite = null;
    }

    public int getAllBuffs()
    {
        return pianoRhythmHandler.battleBuffAccumulated +
            drumsRhythmHandler.battleBuffAccumulated +
            bassRhythmHandler.battleBuffAccumulated +
            trumpetRhythmHandler.battleBuffAccumulated;
    }
    public int getAllDebuffs()
    {
        return pianoRhythmHandler.battleDebuffAccumulated +
            drumsRhythmHandler.battleDebuffAccumulated +
            bassRhythmHandler.battleDebuffAccumulated +
            trumpetRhythmHandler.battleDebuffAccumulated;
    }

    private void destroyAllBeatsOnScreen()
    { 
        foreach (Transform child in rhythmSpawnLocation.transform)
        {
            Destroy(child.gameObject);
        }
        generatedP1Beats = new List<RhythmBeat>();
        
    }


    public void p1PressedTypeInput(InputType inputType)
    {

        if (generatedP1Beats.Count == 0)
        {
            return;
            //if there aren't any beats ignore the player's input
        }
        RhythmBeat rhythmBeatToCheck = generatedP1Beats[0];
        float distanceToTarget = Vector2.Distance(rhythmBeatToCheck.gameObject.transform.position, rhythmTarget.transform.position);

        Debug.Log($"Distance to target is [{distanceToTarget}]");


        //if the beat is outside the nearness check, don't check it
        if (distanceToTarget > nearnessCheckForBeat)
        {
            Debug.Log($"Outside range Distance to target is [{distanceToTarget}]");
            //it's outside out range, ignore it
            return;
        } else if (distanceToTarget > accuracyRequirementGood)
        {
            Debug.Log("Miss!");
            registerTargetMiss(rhythmBeatToCheck);
            
        }
        else if (inputType != rhythmBeatToCheck.inputType)
        {
            Debug.Log("Miss! Wrong Input!");
            registerTargetMiss(rhythmBeatToCheck);
        }
        else if (distanceToTarget > accuracyRequirementPerfect)
        {
            Debug.Log("Good!");
            registerTargetGood(rhythmBeatToCheck);
        } else
        {
            Debug.Log("Perfect!");
            registerTargetPerfect(rhythmBeatToCheck);
        }

       

    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            InputType myInput = InputType.down;
            myInput = InputType.down;
            p1PressedTypeInput(myInput);
        } else if  (Input.GetKeyDown(KeyCode.UpArrow))
        {
            InputType myInput = InputType.down;
            myInput = InputType.up;
            p1PressedTypeInput(myInput);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            InputType myInput = InputType.down;
            myInput = InputType.left;
            p1PressedTypeInput(myInput);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            InputType myInput = InputType.down;
            myInput = InputType.right;
            p1PressedTypeInput(myInput);
        }

        
    }

    public void registerTargetMiss(RhythmBeat rhythmBeat)
    {
        rpgManager.playerMissOccurred();
        Instantiate(missIndicatorPrefab, rhythmTarget);
    }

    private void registerTargetGood(RhythmBeat rhythmBeat)
    {
        rpgManager.playerHitOccurred(0);
        destroyBeat(rhythmBeat);
        Instantiate(goodIndicatorPrefab, rhythmTarget);
    }

    private void registerTargetPerfect(RhythmBeat rhythmBeat)
    {
        rpgManager.playerHitOccurred(1);
        destroyBeat(rhythmBeat);
        Instantiate(perfectIndicatorPrefab, rhythmTarget);
    }

    public void destroyBeat(RhythmBeat rhythmBeat)
    {
        generatedP1Beats.Remove(rhythmBeat);
        Destroy(rhythmBeat.gameObject);
        if (generatedP1Beats.Count > 0)
        {
            RhythmBeat nextBeat = generatedP1Beats[0];
            rhythmTarget.gameObject.GetComponent<SpriteRenderer>().sprite = nextBeat.GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void generateBeat()
    {
        
        GameObject beatObject = Instantiate(beatPrefab, rhythmSpawnLocation);
        RhythmBeat rhythmBeat = beatObject.GetComponent<RhythmBeat>();
        InputType chosenInput = (InputType)Random.Range(0, 4);
        Sprite spriteToUse = beatUp;
        switch(chosenInput)
        {
            case InputType.up:
                spriteToUse = beatUp;
                break;
            case InputType.down:
                spriteToUse = beatDown;
                break;
            case InputType.left:
                spriteToUse = beatLeft;
                break;
            case InputType.right:
                spriteToUse = beatRight;
                break;
        }

        rhythmBeat.setupBeat(spriteToUse, rhythmBeatsSpeed, chosenInput, this);
        generatedP1Beats.Add(rhythmBeat);
        if (rhythmTarget.gameObject.GetComponent<SpriteRenderer>().sprite == null)
        {
            RhythmBeat nextBeat = generatedP1Beats[0];
            rhythmTarget.gameObject.GetComponent<SpriteRenderer>().sprite = nextBeat.GetComponent<SpriteRenderer>().sprite;
        }

    }

    



    

}

public enum InputType
{
    up,
    down,
    left,
    right
}
