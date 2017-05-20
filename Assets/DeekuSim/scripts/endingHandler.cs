using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class endingHandler : MonoBehaviour {

  public Text scoreText;

	void Start () {
    scoreText.text = "You gots " + ApplicationData.score.ToString() + " points.";
	}
}
