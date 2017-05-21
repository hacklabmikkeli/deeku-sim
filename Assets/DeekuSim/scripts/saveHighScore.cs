using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.IO;

public class saveHighScore : MonoBehaviour {

  public string scoreServerUrl;
  public string scoreServerApiKey;
  public Button saveButton;
  public Text inputText;
  public Text statusText;

  void Start () {
  }

  public void saveHighscore() {
    string name = inputText.text;
    if (!string.IsNullOrEmpty (name)) {
      if(statusText != null) {
        statusText.text = "Saving";
      }
      HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create (scoreServerUrl + "/" + scoreServerApiKey);
      httpWebRequest.ContentType = "application/json";
      httpWebRequest.Method = "POST";

      using (StreamWriter streamWriter = new StreamWriter (httpWebRequest.GetRequestStream ())) {
        string json = "{\"name\":\"" + name + "\", \"score\":" + DeekuSimData.playerScore.ToString () + " }";

        streamWriter.Write (json);
      }

      HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse ();
      using (StreamReader streamReader = new StreamReader (httpResponse.GetResponseStream ())) {
        var result = streamReader.ReadToEnd ();
        if(statusText != null) {
          statusText.text = "Saved successfully.";
        }
        Destroy (saveButton);
      }
    }
  }
}
