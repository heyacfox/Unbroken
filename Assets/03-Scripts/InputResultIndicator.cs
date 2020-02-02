using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputResultIndicator : MonoBehaviour
{
    public float upSpeed = 1f;
    public float timeUntilDestroy = 1f;
    public float ySpawnOffset = 2f;
    public float xSpawnOffset = -1f;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, upSpeed);
        this.transform.position = new Vector2(this.transform.position.x + xSpawnOffset, transform.position.y + ySpawnOffset);
        StartCoroutine(destroyAfterTime());
    }

    IEnumerator destroyAfterTime()
    {
        yield return new WaitForSeconds(timeUntilDestroy);
        Destroy(this.gameObject);
    }

    
}
