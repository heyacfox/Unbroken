using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContinueRetire : MonoBehaviour
{
    public string con = "MainBattle";

    public string retire = "Credits";

    public Text savedText;
    // Start is called before the first frame update
    void Start()
    {
        int myCount = ContinueStacks.instance.currentStacks * 3;
        savedText.text = "Number Saved: " + myCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Continue()
    {
        ContinueStacks.instance.currentStacks++;
        SceneManager.LoadScene(con);
    }
    public void Retire()
    {
        SceneManager.LoadScene(retire);
    }
}
