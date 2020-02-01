using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayer : MonoBehaviour
{
    public RhythmManager rhythmManager;
    public void ButtonInput(string input)
    {

        switch (input)
        {
            case "right":
                rhythmManager.p1PressedTypeInput(InputType.right);
                break;
            case "left":
                rhythmManager.p1PressedTypeInput(InputType.left);
                break;
            case "up":
                rhythmManager.p1PressedTypeInput(InputType.up);
                break;
            case "down":
                rhythmManager.p1PressedTypeInput(InputType.down);
                break;


        }
    }
}
