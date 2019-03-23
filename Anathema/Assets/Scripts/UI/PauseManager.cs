using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Anathema.SceneLoading;

public class PauseManager : MonoBehaviour {

	[SerializeField] private GameObject pauseMenu;
	[SerializeField] private string loadingScene;
	[SerializeField] private string playerScene;
	[SerializeField] private string mainMenuScene;
	
	void Update ()
	{
		if(Input.GetButtonDown("Pause"))
			Pause();
		
	}

	public void Pause()
	{
		if(pauseMenu.activeInHierarchy)
		{
			pauseMenu.SetActive(false);
			Time.timeScale = 1f;
		}
		else
		{
			pauseMenu.SetActive(true);
			Time.timeScale = 0f;
		}
	}

	public void Exit()
	{
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            
            if (scene.name != playerScene)
            {
                SceneLoader loader = new SceneLoader(loadingScene);
                
                loader.ScenesToUnload.Add(playerScene);
                loader.ScenesToUnload.Add(scene.name);
                loader.ScenesToLoad.Add(mainMenuScene);

                Time.timeScale = 1f;
                loader.FadeScenes();
                break;
            }
        }
	}
}
