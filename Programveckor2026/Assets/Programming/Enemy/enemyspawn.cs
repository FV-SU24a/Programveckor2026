using UnityEngine;
using System.Collections;

public class enemyspawn : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;
    public float spawnRate = 1f;
    public float spawnOffset = 1f; // distance outside left camera edge
    public float spawnY = 0;

    public void SpawnWave(int waveNumber, WaveEnemy waveEnemy)
    {
        StartCoroutine(SpawnWaveCoroutine(waveNumber, waveEnemy));
    }

    private IEnumerator SpawnWaveCoroutine(int waveNumber, WaveEnemy waveEnemy)
    {
        int waveSinceStart = Mathf.Max(0, waveNumber - waveEnemy.startWave);
        int enemiesToSpawn = waveEnemy.baseCount + (waveSinceStart * waveEnemy.perWaveIncrease);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // STOP spawning normal enemies if a boss wave is active
            if (WaveManager.Instance.isBossWaveThisRound && !waveEnemy.enemyPrefab.CompareTag("Boss"))
                yield break;

            float spawnX = player.position.x - (Camera.main.orthographicSize * Camera.main.aspect + spawnOffset);
            Vector3 spawnPos = new Vector3(spawnX, spawnY, 0f);

            Instantiate(waveEnemy.enemyPrefab, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(spawnRate);
        }
    }
}
