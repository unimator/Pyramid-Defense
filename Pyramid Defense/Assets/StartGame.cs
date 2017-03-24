using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Button StartGameButton;

	// Use this for initialization
	void Start ()
	{
	    StartGameButton.onClick.AddListener(OnClick);
	}

    private void OnClick()
    {
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }

    // Update is called once per frame
	void Update () {
		
	}
}
