using UnityEngine;

public class SwordOrbit : MonoBehaviour
{
    [Header("Cài đặt Xoay")]
    [Tooltip("Tốc độ xoay quanh nhân vật (càng lớn xoay càng nhanh)")]
    public float rotateSpeed = 250f;

    [Tooltip("Khoảng cách từ kiếm đến nhân vật")]
    public float orbitRadius = 1.5f;

    [Header("Sát thương")]
    public int damage = 2;

    private Transform playerTransform;
    private Transform swordSpriteTransform;

    void Start()
    {
        // Tìm nhân vật Player1
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }

        // Lấy hình ảnh kiếm (vật thể con bên trong)
        if (transform.childCount > 0)
        {
            swordSpriteTransform = transform.GetChild(0);
            // Đẩy kiếm ra xa tâm một đoạn bằng orbitRadius
            swordSpriteTransform.localPosition = new Vector3(orbitRadius, 0f, 0f);
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            // 1. Luôn đi theo vị trí của Player
            transform.position = playerTransform.position;

            // 2. Tự xoay quanh trục Z để kiếm quay vòng tròn
            transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        }
    }

    // Khi kiếm chém trúng quái
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.GetComponent<EnemyScript>() != null)
        {
            EnemyScript enemy = other.GetComponent<EnemyScript>();
            if (enemy != null)
            {
                enemy.health -= damage;
                Debug.Log("Kiếm xoay chém trúng quái! -2 Máu");

                // Nếu quái hết máu thì tiêu diệt
                if (enemy.health <= 0)
                {
                    if (enemy.expGemPrefab != null)
                    {
                        Instantiate(enemy.expGemPrefab, enemy.transform.position, Quaternion.identity);
                    }
                    Destroy(enemy.gameObject);
                }
            }
        }
    }
}