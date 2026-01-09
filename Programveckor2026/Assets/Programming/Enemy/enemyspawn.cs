using UnityEngine;
using System.Collections;
public class enemyspawn : MonoBehaviour
{
    //THIS SCRIPT WILL BE CALLED IN WAVE MANAGER
    public GameObject enemyPrefab;
    public Transform player;
    public float spawnRate = 1f;
    public float spawnOffset = 1f; //distance outside left camera edge

    public void SpawnWave(int waveNumber, WaveEnemy waveEnemy)
    {
        StartCoroutine(SpawnWaveCoroutine(waveNumber, waveEnemy));
    }

    private IEnumerator SpawnWaveCoroutine(int waveNumber, WaveEnemy waveEnemy)
    {
        int waveSinceStart = Mathf.Max(0, waveNumber - waveEnemy.startWave);
        int enemiesToSpawn = waveEnemy.baseCount + (waveSinceStart * waveEnemy.perWaveIncrease);

        for(int i = 0; i < enemiesToSpawn; i++)
        {
            //raandom Y offset around the player
            float randomY = player.position.y + Random.Range(-4f, 2f); //can be adjutsted

            //calculate spawn position jut outside the left of the camera
            float spawnX = player.position.x - (Camera.main.orthographicSize * Camera.main.aspect + spawnOffset);

            Vector3 spawnPos = new Vector3(spawnX, randomY, 0f);

            //spawn the enemy
            Instantiate(waveEnemy.enemyPrefab, spawnPos, Quaternion.identity);

            //ait before spawning the next one
            yield return new WaitForSeconds(spawnRate);
        }

    }

}
