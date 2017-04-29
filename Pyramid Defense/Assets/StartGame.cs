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
        SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }

    private void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode loadSceneMode)
    {
        Debug.Log(arg0.name);
    }

    // Update is called once per frame
	void Update () {
		
	}
}
