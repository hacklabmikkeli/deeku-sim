using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuKeyboardListener : MonoBehaviour {

  public int GameSceneIndex;

  private loadHighscores highscoreHandler;

  public void Start() {
    highscoreHandler = gameObject.GetComponent("loadHighscores") as loadHighscores;
  }

  public void Update() {
    if (Input.GetKeyDown (KeyCode.Alpha1)) {
      SceneManager.LoadScene (GameSceneIndex);
    }

    if (Input.GetKeyDown (KeyCode.Escape)) {
      Application.Quit();
    }

    if (Input.GetKeyDown (KeyCode.Alpha2)) {
      highscoreHandler.loadHighscore ();
    }

    if (Input.GetKeyDown (KeyCode.Backspace)) {
      highscoreHandler.closeHighscore();
    }

  }
}
