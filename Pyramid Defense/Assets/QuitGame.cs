using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    public Button QuitButton;

	// Use this for initialization
	void Start () {
	    QuitButton.onClick.AddListener(OnClick);  
	}

    private void OnClick()
    {
        Application.Quit();
    }


    // Update is called once per frame
	void Update () {
		
	}
}
