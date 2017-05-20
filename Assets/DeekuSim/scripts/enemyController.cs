using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour {

  public float speed;
  public float turnSpeed;
  public AudioSource attackAudio;

  private GameObject player;
  private Animator animator;

	void Start () {
    animator = gameObject.GetComponent ("Animator") as Animator;
    player = GameObject.FindGameObjectWithTag("player");
	}


  void OnCollisionEnter (Collision col) {
    if (col.gameObject.tag == "player") {
      attackAudio.Play();
      playerMovement pm = col.gameObject.GetComponent ("playerMovement") as playerMovement;
      pm.startDamage();
    }
  }

  void OnCollisionExit(Collision col) {
    if (col.gameObject.tag == "player") {
      playerMovement pm = col.gameObject.GetComponent ("playerMovement") as playerMovement;
      pm.stopDamage();
    }
  }


	void Update () {
    animator.SetFloat("distanceToPlayer", Vector3.Distance (transform.position, player.transform.position));
    Vector3 targetDir = player.transform.position - transform.position;
    transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDir,  turnSpeed, 0.0F));
    transform.position = Vector3.MoveTowards (transform.position, player.transform.position, getRunSpeed ());
	}

  private float getRunSpeed() {
    //TODO: make them faster as player gets weaker
    return speed * Time.deltaTime;
  }

}
