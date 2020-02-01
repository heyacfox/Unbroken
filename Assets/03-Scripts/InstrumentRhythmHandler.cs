using System.Collections;
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

    public GameObject beatPrefab;

    public float generateBeatEveryXSeconds;

    public float nearnessCheckForBeat = 3;
    public float nearnessCheckForGood = 1f;
    public float nearnessCheckForPerfect = 0.5f;


    private void Start()
    {
        instrumentRhythmBeats = new List<RhythmBeat>();
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
            acInstrumentPlayer.gotMiss();
        }
        else if (distanceToTarget > nearnessCheckForPerfect)
        {
            Debug.Log("Good!");
            acInstrumentPlayer.gotGood();
        }
        else
        {
            Debug.Log("Perfect!");
            acInstrumentPlayer.gotPerfect();
        }



    }

    private void Update()
    {

    }

    public void registerTargetMiss(RhythmBeat rhythmBeat)
    {
        rpgManager.playerMissOccurred();
    }

    private void registerTargetGood(RhythmBeat rhythmBeat)
    {
        rpgManager.playerHitOccurred(0);
        destroyBeat(rhythmBeat);
    }

    private void registerTargetPerfect(RhythmBeat rhythmBeat)
    {
        rpgManager.playerHitOccurred(1);
        destroyBeat(rhythmBeat);
    }

    public void destroyBeat(RhythmBeat rhythmBeat)
    {
        instrumentRhythmBeats.Remove(rhythmBeat);
        Destroy(rhythmBeat.gameObject);
    }

    public void generateBeat()
    {
        InputType chosenInput = (InputType)Random.Range(0, 4);
        switch(chosenInput)
        {
            case InputType.down:
                break;


        }/*
        GameObject beatObject = Instantiate(beatPrefab, rhythmSpawnLocation);
        RhythmBeat rhythmBeat = beatObject.GetComponent<RhythmBeat>();
        Sprite spriteToUse = beatUp;
        switch (chosenInput)
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
        */

    }



}
