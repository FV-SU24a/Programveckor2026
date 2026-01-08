using UnityEngine;
using System.Collections;
public class enemyspawn : MonoBehaviour
{
    //THIS SCRIPT WILL BE CALLED IN WAVE MANAGER
    public GameObject enemyPrefab;
    public Transform player;
    public float spawnRate = 1f;
    public float spawnOffset = 1f; //distance outside left camera edge

    public void SpawnWave(int waveNumber)
    {
        StartCoroutine(SpawnWaveCoroutine(waveNumber));
    }

    private IEnumerator SpawnWaveCoroutine(int waveNumber)
    {
        int enemiesToSpawn = Mathf.CeilToInt(3 * Mathf.Pow(1.5f, waveNumber)); //scalling..can be changed

        for(int i = 0; i < enemiesToSpawn; i++)
        {
            Vector3 spawnPos = player.position + Vector3.left * (Camera.main.orthographicSize * Camera.main.aspect + spawnOffset);
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(spawnRate);
        }
    }

}
