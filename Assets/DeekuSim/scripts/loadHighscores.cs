using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

[Serializable]
public class Highscore
{
  public string _id;
  public int __v;
  public string name;
  public float score;
  public string created;
}

public class loadHighscores : MonoBehaviour {

  public string highscoreServerUrl;
  public Text highscoreText;
  public GameObject highscorePanel;

  void Start () {
  }

  public void closeHighscore() {
    highscorePanel.SetActive (false);
  }

  public void loadHighscore() {
    highscorePanel.SetActive (true);

    ServicePointManager.ServerCertificateValidationCallback = certificateValidationCallback;
    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create (highscoreServerUrl);
    httpWebRequest.ContentType = "application/json";
    httpWebRequest.Method = "GET";

    HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse ();
    using (StreamReader streamReader = new StreamReader (httpResponse.GetResponseStream ())) {
      string json = streamReader.ReadToEnd ();
      Highscore[] highscores = JsonHelper.FromJson<Highscore>("{\"Items\":" + json + "}");//JsonUtility.FromJson<Highscore>(json);
      int index = 1;
      string text = "";
      foreach(Highscore score in highscores) {
        string indexHuman = index.ToString ();
        if (index == 1) {
          indexHuman += "ST";
        } else if (index == 2) {
          indexHuman += "ND";
        } else if (index == 3) {
          indexHuman += "RD";
        } else {
          indexHuman += "TH";
        }
        index++;
        text += indexHuman + "\t\t" + score.name + "\t\t" + score.score.ToString ("0.00") + "\n";
      }
      highscoreText.text = text;
    }
  }

  public bool certificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
    bool isOk = true;
    if (sslPolicyErrors != SslPolicyErrors.None) {
      for (int i=0; i<chain.ChainStatus.Length; i++) {
        if (chain.ChainStatus [i].Status != X509ChainStatusFlags.RevocationStatusUnknown) {
          chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
          chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
          chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan (0, 1, 0);
          chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
          bool chainIsValid = chain.Build ((X509Certificate2)certificate);
          if (!chainIsValid) {
            isOk = false;
          }
        }
      }
    }
    return isOk;
  }
}
