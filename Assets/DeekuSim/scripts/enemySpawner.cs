using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour {

  public List<Transform> enemies;
  public float spawnProbability;
  public float spawnAttemptMinDelay;
  public float spawnAttemptMaxDelay;
  public float maxSpawnDistanceBase;
  public float spawnDistanceRandom;

  private Light playerLight;
  private float timeSinceEnemySpawned;
  private float nextEnemySpawn;

	void Start () {
    playerLight = GameObject.FindGameObjectWithTag("valo").GetComponent ("Light") as Light;
    timeSinceEnemySpawned = 0;
	}

	void Update () {
    if (timeSinceEnemySpawned + Time.deltaTime * 1000 >= nextEnemySpawn) {
      attemptSpawn();
      nextEnemySpawn = Random.Range(spawnAttemptMinDelay, spawnAttemptMaxDelay);
      timeSinceEnemySpawned = 0;
    } else {
      timeSinceEnemySpawned += Time.deltaTime * 1000;
    }
	}

  private void attemptSpawn() {
    float number = Random.Range(0f, 100f);
    if (number <= spawnProbability) {
      float spawnMinDistance = Mathf.Min ((playerLight.spotAngle - 30f), maxSpawnDistanceBase);
      float spawnMaxDistance = spawnMinDistance + spawnDistanceRandom;

      Vector3 spawnPosition = Random.insideUnitSphere;
      spawnPosition = (spawnPosition.normalized * Random.Range(spawnMinDistance, spawnMaxDistance)) + transform.position;
      spawnPosition.y = Terrain.activeTerrain.SampleHeight(spawnPosition) + 0.5f;
      Instantiate(enemies[Random.Range(0, enemies.Count)], spawnPosition, Quaternion.identity);
    }
  }
}
