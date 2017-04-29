using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private int delay;
    public static UIManager Instance;
    public GameObject VicotryWindow;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    delay += 1;
        if(delay == 0x30)
            ShowVictoryScreen();
	}

    public void ShowVictoryScreen()
    {
        VicotryWindow.SetActive(true);
    }

    void Awake()
    {
        Instance = this;
    }
}
