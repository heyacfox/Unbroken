using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACInstrumentPlayer : MonoBehaviour
{
    public int deviceTracker;
    public InstrumentRhythmHandler insRhythmHandler;
    public bool holdFlag = false;
    public float waitingTimeToNextInput = 1f;

    public void ButtonInput(string input)
    {
        if (holdFlag == false)
        {
            switch (input)
            {
                case "right":
                    insRhythmHandler.receivedPressedTypeInput(InputType.right, this);
                    break;
                case "left":
                    insRhythmHandler.receivedPressedTypeInput(InputType.left, this);
                    break;
                case "up":
                    insRhythmHandler.receivedPressedTypeInput(InputType.up, this);
                    break;
                case "down":
                    insRhythmHandler.receivedPressedTypeInput(InputType.down, this);
                    break;


            }

            StartCoroutine(restrictInput());
        }

    }

    IEnumerator restrictInput()
    {
        holdFlag = true;
        yield return new WaitForSeconds(waitingTimeToNextInput);
        holdFlag = false;
    }

    public void gotMiss()
    {
        Debug.Log($"Player [{deviceTracker}] missed an input");
    }

    public void gotPerfect()
    {
        Debug.Log($"Player [{deviceTracker}] perfected an input");
    }

    public void gotGood()
    {

    }
}
