﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentRhythmHandler : MonoBehaviour
{
    public List<RhythmBeat> instrumentRhythmBeats;

    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    public RPGManager rpgManager;

    public Transform upStart;
    public Transform downStart;
    public Transform leftStart;
    public Transform rightStart;

    public Transform rhythmTarget;
    public float rhythmBeatsSpeed;

    public GameObject beatPrefab;

    public float generateBeatEveryXSeconds;

    public float nearnessCheckForBeat = 3;
    public float nearnessCheckForGood = 1f;
    public float nearnessCheckForPerfect = 0.5f;


    private void Start()
    {
        instrumentRhythmBeats = new List<RhythmBeat>();
        StartCoroutine(generateBeatsInTime());
    }

    IEnumerator generateBeatsInTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(generateBeatEveryXSeconds);
            generateBeat();
        }
    }

    public void gameWon()
    {
        StopAllCoroutines();
        destroyAllBeatsOnScreen();
    }

    public void gameLost()
    {
        StopCoroutine(generateBeatsInTime());
        destroyAllBeatsOnScreen();
    }

    private void destroyAllBeatsOnScreen()
    {

        foreach (Transform child in upStart.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in downStart.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in leftStart.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in rightStart.transform)
        {
            Destroy(child.gameObject);
        }
        instrumentRhythmBeats = new List<RhythmBeat>();

    }


    public void receivedPressedTypeInput(InputType inputType, ACInstrumentPlayer acInstrumentPlayer)
    {

        if (instrumentRhythmBeats.Count == 0)
        {
            return;
            //if there aren't any beats ignore the player's input
        }
        RhythmBeat rhythmBeatToCheck = instrumentRhythmBeats[0];
        float distanceToTarget = Vector2.Distance(rhythmBeatToCheck.gameObject.transform.position, rhythmTarget.transform.position);

        Debug.Log($"Distance to target is [{distanceToTarget}s]");


        //if the beat is outside the nearness check, don't check it
        if (distanceToTarget > nearnessCheckForBeat)
        {
            Debug.Log($"Outside range Distance to target is [{distanceToTarget}]");
            //it's outside out range, ignore it
            return;
        }
        else if (distanceToTarget > nearnessCheckForGood)
        {
            Debug.Log("Miss!");
            acInstrumentPlayer.gotMiss();
        }
        else if (inputType != rhythmBeatToCheck.inputType)
        {
            Debug.Log("Miss! Wrong Input!");
            registerTargetMiss();
            acInstrumentPlayer.gotMiss();
        }
        else if (distanceToTarget > nearnessCheckForPerfect)
        {
            Debug.Log("Good!");
            registerTargetGood();
            acInstrumentPlayer.gotGood();
        }
        else
        {
            Debug.Log("Perfect!");
            registerTargetPerfect();
            acInstrumentPlayer.gotPerfect();
        }



    }

    public void registerTargetMiss()
    {
        rpgManager.addDebuffStack();
        
    }

    private void registerTargetGood()
    {
        rpgManager.addBuffStack();
        //destroyBeat(rhythmBeat);
    }

    private void registerTargetPerfect()
    {
        rpgManager.addBuffStack();
        rpgManager.addBuffStack();
        //destroyBeat(rhythmBeat);
    }

    public void destroyBeat(RhythmBeat rhythmBeat)
    {
        instrumentRhythmBeats.Remove(rhythmBeat);
        Destroy(rhythmBeat.gameObject);
    }

    public void generateBeat()
    {
        Debug.Log("Made a ins beat");
        InputType chosenInput = (InputType)Random.Range(0, 4);
        GameObject beatObject = new GameObject() ;
        Sprite spriteToUse = upSprite;
        switch (chosenInput)
        {
            case InputType.down:
                beatObject = Instantiate(beatPrefab, downStart);
                spriteToUse = downSprite;
                beatObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, rhythmBeatsSpeed);
                break;
            case InputType.up:
                beatObject = Instantiate(beatPrefab, upStart);
                spriteToUse = upSprite;
                beatObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -rhythmBeatsSpeed);
                break;
            case InputType.left:
                beatObject = Instantiate(beatPrefab, leftStart);
                spriteToUse = leftSprite;
                beatObject.GetComponent<Rigidbody2D>().velocity = new Vector2(rhythmBeatsSpeed, 0f);
                break;
            case InputType.right:
                beatObject = Instantiate(beatPrefab, rightStart);
                spriteToUse = rightSprite;
                beatObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-rhythmBeatsSpeed, 0f);
                break;


        }
        RhythmBeat rhythmBeat = beatObject.GetComponent<RhythmBeat>();
        
       

        rhythmBeat.setupBeatIns(spriteToUse, rhythmBeatsSpeed, chosenInput, this);
        instrumentRhythmBeats.Add(rhythmBeat);
        

    }



}