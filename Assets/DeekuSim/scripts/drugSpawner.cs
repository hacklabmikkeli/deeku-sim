using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drugSpawner : MonoBehaviour {

  public List<Transform> drugs;
  public float spawnProbability;
  public float spawnAttemptMinDelay;
  public float spawnAttemptMaxDelay;
  public float spawnMinDistance;
  public float spawnMaxDistance;

  private float timeSinceDrugSpawned;
  private float nextDrugSpawn;

	void Start () {
    timeSinceDrugSpawned = 0;
	}

	void Update () {
    if (timeSinceDrugSpawned + Time.deltaTime * 1000 >= nextDrugSpawn) {
      attemptSpawn();
      nextDrugSpawn = Random.Range(spawnAttemptMinDelay, spawnAttemptMaxDelay);
      timeSinceDrugSpawned = 0;
    } else {
      timeSinceDrugSpawned += Time.deltaTime * 1000;
    }
	}

  private void attemptSpawn() {
    float number = Random.Range(0f, 100f);
    if (number <= spawnProbability) {
      Vector3 spawnPosition = Random.insideUnitSphere;
      spawnPosition = (spawnPosition.normalized * Random.Range(spawnMinDistance, spawnMaxDistance)) + transform.position;
      spawnPosition.y = Terrain.activeTerrain.SampleHeight(spawnPosition) + 0.5f;
      Instantiate(drugs[Random.Range(0, drugs.Count)], spawnPosition, Quaternion.identity);
    }
  }
}
