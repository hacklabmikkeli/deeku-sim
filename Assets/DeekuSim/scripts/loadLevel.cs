using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadLevel : MonoBehaviour {

  public void NextLevelButton(int index) {
    SceneManager.LoadScene (index);
  }

  public void ExitDeekuSim() {
    Application.Quit();
  }

}
