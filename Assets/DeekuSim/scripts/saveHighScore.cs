using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

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

      ServicePointManager.ServerCertificateValidationCallback = certificateValidationCallback;
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
