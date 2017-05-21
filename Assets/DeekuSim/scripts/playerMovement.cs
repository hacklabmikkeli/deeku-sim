using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerMovement : MonoBehaviour {

  public float jumpForce;
  public float runSpeed;
  public float turnSpeed;
  public int maxHealth;
  public float healthDecreaseInterval;
  public float powerUpDelay;
  public float damageDelay;
  public float crippledThreshold;
  public AudioSource hurtAudioSource;
  public int nextLevel;

  private Rigidbody rb;
  private Text healthText;
  private Light mainLight;
  private Animator animator;

	private Boolean doubleJumped = false;
  private int currentHealth;
  private float timeSinceHealthLastUpdated;
  private float timeSinceTakenDamage;

  private float timeSinceSpeedUpActivated;
  private float timeSinceSlowDownActivated;
  private float timeSinceIncreaseJumpActivated;
  private float timeSincePoisonActivated;

  private Boolean speedUpActive;
  private Boolean slowDownActive;
  private Boolean increaseJumpActive;
  private Boolean poisonActive;
  private Boolean underAttack;
  private float points;

	void Start () {
    points = 0f;
    underAttack = false;
    speedUpActive = false;
    slowDownActive = false;
    increaseJumpActive = false;
    poisonActive = false;

    animator = gameObject.GetComponent("Animator") as Animator;
    mainLight = GameObject.FindGameObjectWithTag("valo").GetComponent ("Light") as Light;
		rb = gameObject.GetComponent("Rigidbody") as Rigidbody;
    healthText = GameObject.FindGameObjectWithTag("healthText").GetComponent("Text") as Text;
    currentHealth = maxHealth;
    Screen.lockCursor = true;
	}
	
	void Update () {
    checkPowerUps();

    points += Time.deltaTime;

    healthText.text = "Score: " + points.ToString();

    if (currentHealth <= 0) {
      DeekuSimData.playerScore = points;
      Screen.lockCursor = false;
      SceneManager.LoadScene(nextLevel);
    }

    if (timeSinceHealthLastUpdated >= getHealthDecreaseInterval()) {
      currentHealth--;
      timeSinceHealthLastUpdated = 0f;
    } else {
      timeSinceHealthLastUpdated += Time.deltaTime * 1000;
    }

    updateLights();

		if (isGrounded ()) {
		  doubleJumped = false;
		}

		if (Input.GetButtonDown ("Jump") && (isGrounded() || doubleJumped == false)) {
			if (!isGrounded () && doubleJumped == false) {
				doubleJumped = true;
			}
      rb.AddForce (new Vector3 (0, getJumpForce(), 0));
		}

    float animationSpeed = Input.GetAxis("Vertical") * getRunSpeed();
    float x = Input.GetAxis("Horizontal") * Time.deltaTime * turnSpeed;
    float z = animationSpeed * Time.deltaTime;
    float speedMulti = 1f;

    if (animationSpeed > 10) {
      speedMulti = 2f;
    }

    animator.SetFloat("speed", animationSpeed);
    animator.SetFloat("speedMulti", speedMulti);

		transform.Rotate(0, x, 0);
		transform.Translate(0, 0, z);
	}

  public void heal() {
    this.currentHealth += 15;
    if (this.currentHealth > maxHealth) {
      this.currentHealth = maxHealth;
    }
  }

  public void speedUp() {
    speedUpActive = true;
    timeSinceSpeedUpActivated = 0f;
  }

  public void slowDown() {
    slowDownActive = true;
    timeSinceSlowDownActivated = 0f;
  }

  public void increaseJump() {
    increaseJumpActive = true;
    timeSinceIncreaseJumpActivated = 0f;
  }

  public void poison() {
    poisonActive = true;
    timeSincePoisonActivated = 0f;
  }

  public void startDamage() {
    this.currentHealth -= 10;
    hurtAudioSource.Play();
    this.underAttack = true;
    timeSinceTakenDamage = 0f;
  }

  public void stopDamage() {
    this.underAttack = false;
  }

  private void updateLights() {
    float healthValue = currentHealth;
    if (healthValue < 1) {
      healthValue = 1;
    }

    mainLight.spotAngle = 179 / (maxHealth / healthValue);
  }

  public void addPoints(float pointsToAdd) {
    this.points += pointsToAdd;
  }

	private Boolean isGrounded() {
	  return Physics.Raycast (transform.position, -Vector3.up, 0.1f);
	}

  private void checkPowerUps() {
    if (underAttack) {
      if (timeSinceTakenDamage + Time.deltaTime * 1000f > damageDelay) {
        timeSinceTakenDamage = 0f;
        this.currentHealth -= 10;
        hurtAudioSource.Play();
      } else {
        timeSinceTakenDamage += Time.deltaTime * 1000f;
      }
    }

    if (speedUpActive) {
      if (timeSinceSpeedUpActivated + Time.deltaTime * 1000f > powerUpDelay) {
        speedUpActive = false;
      } else {
        timeSinceSpeedUpActivated += Time.deltaTime * 1000f;
      }
    }

    if (slowDownActive) {
      if (timeSinceSlowDownActivated + Time.deltaTime * 1000f > powerUpDelay) {
        slowDownActive = false;
      } else {
        timeSinceSlowDownActivated += Time.deltaTime * 1000f;
      }
    }

    if (increaseJumpActive) {
      if (timeSinceIncreaseJumpActivated + Time.deltaTime * 1000f > powerUpDelay) {
        increaseJumpActive = false;
      } else {
        timeSinceIncreaseJumpActivated += Time.deltaTime * 1000f;
      }
    }

    if (poisonActive) {
      if (timeSincePoisonActivated + Time.deltaTime * 1000f > powerUpDelay) {
        poisonActive = false;
      } else {
        timeSincePoisonActivated += Time.deltaTime * 1000f;
      }
    }
  }

  private float getHealthDecreaseInterval() {
    if (poisonActive) {
      float interval = healthDecreaseInterval - 50f;
      if (interval < 1) {
        interval = 1;
      }
      return interval;
    } else {
      return healthDecreaseInterval;
    }
  }

  private float getRunSpeed() {
    float speed = runSpeed;
    if (speedUpActive) {
      speed += speed * 2;
    }
    if (slowDownActive) {
      speed = speed / 2;
    }
    if (this.currentHealth <= crippledThreshold) {
      speed = speed / 2;
    }

    return speed;
  }

  private float getJumpForce() {
    float force = jumpForce;
    if (increaseJumpActive) {
      force = force * 2;
    }

    return force;
  }
}
