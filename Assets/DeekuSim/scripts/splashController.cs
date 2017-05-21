using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class splashController : MonoBehaviour {

  private RawImage image;
  public int nextLevel;
  private float currentBrightness;
  public float fadeInSpeed;

	void Start () {
    image = gameObject.GetComponent ("RawImage") as RawImage;
    currentBrightness = 0f;
	}

  void Update() {
    if (currentBrightness < 1f) {
      currentBrightness += Time.deltaTime * fadeInSpeed;
    } else {
      //Load next level
    }
    image.color = new Color (currentBrightness, currentBrightness, currentBrightness, currentBrightness);
  }
}
