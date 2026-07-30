using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRate = 2f;
    public float spawnRadius = 10f;
    private float timer;
    private Transform player;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnRate && player != null)
        {
            Vector2 randomPos = (Vector2)player.position + Random.insideUnitCircle.normalized * spawnRadius;
            Instantiate(enemyPrefab, randomPos, Quaternion.identity);
            timer = 0;
        }
    }
}