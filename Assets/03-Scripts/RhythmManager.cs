using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{
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
 

    public void Start()
    {
        StartCoroutine(generateBeatsInTime());
    }

    IEnumerator generateBeatsInTime()
    {
        while(true)
        {
            yield return new WaitForSeconds(generateBeatEveryXSeconds);


            generateBeat();
        }
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
            Debug.Log("Outside range");
            //it's outside out range, ignore it
            return;
        } else if (distanceToTarget > accuracyRequirementGood)
        {
            Debug.Log("Outside range");
            registerTargetMiss(rhythmBeatToCheck);
        } else if (distanceToTarget > accuracyRequirementPerfect)
        {
            Debug.Log("Outside range");
            registerTargetGood(rhythmBeatToCheck);
        } else
        {
            Debug.Log("Outside range");
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

    }

    private void registerTargetGood(RhythmBeat rhythmBeat)
    {

    }

    private void registerTargetPerfect(RhythmBeat rhythmBeat)
    {

    }

    public void destroyBeat(RhythmBeat rhythmBeat)
    {
        generatedP1Beats.Remove(rhythmBeat);
        Destroy(rhythmBeat.gameObject);
    }

    public void generateBeat()
    {
        GameObject beatObject = Instantiate(beatPrefab, rhythmSpawnLocation);
        RhythmBeat rhythmBeat = beatObject.GetComponent<RhythmBeat>();
        InputType chosenInput = (InputType)Random.Range(0, 3);
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

    }



    

}

public enum InputType
{
    up,
    down,
    left,
    right
}
