using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueRetire : MonoBehaviour
{
    public string con = "Continue";

    public string retire = "Retire";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Continue()
    {
        SceneManager.LoadScene(con);
    }
    public void Retire()
    {
        SceneManager.LoadScene(retire);
    }
}
