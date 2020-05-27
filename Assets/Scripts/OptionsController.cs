using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class OptionsController : MonoBehaviour
{
    public static OptionsController instance;

    [Header("Default Parameters : ")]
    public int defaultMapSize = 10;
    public int defaultNumberOfObstacles = 18;
    public int defaultXStart = 0;
    public int defaultYStart = 4;
    public int defaultXEnd = 9;
    public int defaultYEnd = 4;

    [HideInInspector]
    public int MapSize;
    [HideInInspector]
    public int NumberOfObstacles;
    [HideInInspector]
    public int XStart;
    [HideInInspector]
    public int YStart;
    [HideInInspector]
    public int XEnd;
    [HideInInspector]
    public int YEnd;


    private void Awake()
    {
        //Singleton pattern, we know that only one exists
        instance = this;

        //Keep this object through scenes, since we use it as a holder for data
        if(SceneManager.GetActiveScene().name == "Menu")
            DontDestroyOnLoad(this.gameObject);
    }
}
