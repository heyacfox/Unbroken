using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayOnChar : MonoBehaviour
{

    public float livingTime = 10f;

    private void Start()
    {
        StartCoroutine(destroyAfterTime());
    }

    IEnumerator destroyAfterTime()
    {
        yield return new WaitForSeconds(livingTime);
        Destroy(this.gameObject);
    }
}
