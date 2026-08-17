using UnityEngine;

public class AutoSlash : MonoBehaviour
{
    [Header("--- Huyết Đao ---")]
    public GameObject autoSlashPrefab;

    [Header("--- Sát thương ---")]
    public int damage = 15;

    [Header("--- Cài đặt chém ---")]
    public float attackRate = 0.8f;
    public float spawnDistance = 1.5f;
    public float slashLifeTime = 0.2f;

    private float nextAttack = 0f;
    private bool switchSide = false;

    void Update()
    {
        if (Time.time >= nextAttack)
        {
            nextAttack = Time.time + attackRate;
            SpawnSlash();
        }
    }

    void SpawnSlash()
    {
        if (autoSlashPrefab == null)
        {
            Debug.LogWarning("Chưa gán AutoSlash Prefab!");
            return;
        }

        switchSide = !switchSide;

        Vector3 spawnPosition;
        Quaternion rotation;

        if (switchSide)
        {
            // Chém bên trái
            spawnPosition = transform.position +
                            new Vector3(-spawnDistance, 0f, 0f);

            rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            // Chém bên phải
            spawnPosition = transform.position +
                            new Vector3(spawnDistance, 0f, 0f);

            rotation = Quaternion.identity;
        }

        GameObject slash = Instantiate(
            autoSlashPrefab,
            spawnPosition,
            rotation
        );

        // Tìm Collider của prefab Huyết Đao
        Collider2D slashCollider =
            slash.GetComponent<Collider2D>();

        if (slashCollider != null)
        {
            Collider2D[] enemies =
                Physics2D.OverlapCircleAll(
                    slash.transform.position,
                    1f
                );

            foreach (Collider2D enemy in enemies)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    enemy.SendMessage(
                        "TakeDamage",
                        damage,
                        SendMessageOptions.DontRequireReceiver
                    );

                    Debug.Log(
                        "Huyết Đao chém trúng: " +
                        enemy.gameObject.name
                    );
                }
            }
        }

        Destroy(slash, slashLifeTime);
    }
}