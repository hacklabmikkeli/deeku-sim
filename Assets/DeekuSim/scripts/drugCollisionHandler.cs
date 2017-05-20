using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DrugEffect {
  HEAL,
  SPEED_UP,
  SLOW_DOWN,
  INCREASE_JUMP_FORCE,
  POISON
}

public class drugCollisionHandler : MonoBehaviour {

  public DrugEffect drugEffect;
  public AudioSource useAudio;
  public float drugPoints;

  void OnCollisionEnter (Collision col) {
    if(col.gameObject.tag == "player") {
      playerMovement player = col.gameObject.GetComponent ("playerMovement") as playerMovement;
      player.addPoints(drugPoints);
      switch (drugEffect) {
        case DrugEffect.HEAL:
          player.heal ();
          break;
        case DrugEffect.SPEED_UP:
          player.speedUp ();
          break;
        case DrugEffect.SLOW_DOWN:
          player.slowDown ();
          break;
        case DrugEffect.INCREASE_JUMP_FORCE:
          player.increaseJump ();
          break;
        case DrugEffect.POISON:
          player.poison ();
          break;
      }
      useAudio.Play();
      gameObject.GetComponent<Renderer>().enabled = false;
      Destroy(gameObject, useAudio.clip.length);
    }
  }
}
