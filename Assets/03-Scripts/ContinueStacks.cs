using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueStacks : MonoBehaviour
{
    public static ContinueStacks instance;

    public int currentStacks = 1;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
