using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputResultIndicator : MonoBehaviour
{
    public float upSpeed = 1f;
    public float timeUntilDestroy = 2f;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, upSpeed);
    }

    IEnumerator destroyAfterTime()
    {
        yield return new WaitForSeconds(timeUntilDestroy);
        Destroy(this);
    }

    
}
