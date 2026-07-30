using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 15f;
    public float lifeTime = 2f; // Tự hủy sau 2 giây nếu không trúng quái

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Bay thẳng theo hướng mặt của đạn
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
    // Ghi chú: Việc trừ máu và tự hủy khi trúng quái đã được xử lý bên EnemyScript.
}