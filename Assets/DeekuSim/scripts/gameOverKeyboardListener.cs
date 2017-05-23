using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class gameOverKeyboardListener : MonoBehaviour {

  public int menuSceneIndex;
  public InputField nameInput;

  private saveHighScore savehighScoreHandler;
  private bool scoreSent;
	// Use this for initialization
	void Start () {
    savehighScoreHandler = gameObject.GetComponent ("saveHighScore") as saveHighScore;
	}
	
  void OnGUI() {
    GUI.FocusControl ("playerName");
  }

	// Update is called once per frame
	void Update () {
	
    if (Input.GetKeyDown (KeyCode.Alpha1)) {
      SceneManager.LoadScene (menuSceneIndex);
    }

    if (Input.GetKeyDown (KeyCode.Alpha2)) {
      EventSystem.current.SetSelectedGameObject (nameInput.gameObject);
      nameInput.OnPointerClick(new PointerEventData(EventSystem.current));
    }

    if (Input.GetKeyDown (KeyCode.Return)) {
      savehighScoreHandler.saveHighscore ();
    }

	}
}
