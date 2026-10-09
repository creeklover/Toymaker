using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Collections.AllocatorManager;

public class MainMenu : MonoBehaviour
{
  public void PlayGame()
  {
	  //You can change '1' with scene name; ("Level 1")
	  SceneManager.LoadSceneAsync("Intro BlockOut");
  }

  public void QuitGame()
  {
	  Application.Quit();
  }
}
