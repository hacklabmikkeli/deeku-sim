using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class splashController : MonoBehaviour {

  private RawImage image;
  public int nextLevel;
  public float fadeInSpeed;
  public float fullLogoDelay;
  public AudioSource splashAudio;

  private float currentBrightness;
  private float fullLogoVisibleSince;
  private bool soundStarted;
	void Start () {
    soundStarted = false;
    image = gameObject.GetComponent ("RawImage") as RawImage;
    currentBrightness = 0f;
    fullLogoVisibleSince = 0f;
	}

  void Update() {
    if (currentBrightness < 1f) {
      currentBrightness += Time.deltaTime * fadeInSpeed;
    } else {
      if (!soundStarted) {
        soundStarted = true;
        splashAudio.Play ();
      }
      fullLogoVisibleSince += Time.deltaTime * 1000;
      if (fullLogoVisibleSince > fullLogoDelay) {
        SceneManager.LoadScene(nextLevel);
      }
    }
    image.color = new Color (currentBrightness, currentBrightness, currentBrightness, currentBrightness);
  }
}
