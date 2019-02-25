using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour {

	[SerializeField] private GameObject pauseMenu;

	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
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
		SceneManager.LoadScene("MainMenu");
	}
}
