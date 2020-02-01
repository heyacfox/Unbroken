using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmBeat : MonoBehaviour
{
    public InputType inputType;
    RhythmManager rm;
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Rigidbody2D>().velocity = new Vector2(-0.5f, 0f);
    }

    public void setupBeat(Sprite sprite, float speedToMove, InputType inputType, RhythmManager rhythmManager)
    {
        this.inputType = inputType;
        this.GetComponent<SpriteRenderer>().sprite = sprite;
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(-speedToMove, 0f);
        rm = rhythmManager;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "deleter")
        {
            rm.registerTargetMiss(this);
            rm.destroyBeat(this);
        }
    }


}
