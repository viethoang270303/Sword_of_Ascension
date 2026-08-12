using UnityEngine;

public class WaterOrbit : MonoBehaviour
{
    [Header("--- Cài đặt vòng xoay ---")]
    public Transform playerTarget;      // Quanh ai? (Tự tìm Player nếu để trống)
    public float rotateSpeed = 250f;    // Tốc độ xoay (độ/giây)
    public float orbitRadius = 2f;      // Khoảng cách từ tâm Player đến quả cầu

    [Header("--- Cài đặt sát thương ---")]
    public int damage = 10;             // Lượng máu quái bị mất khi chạm phải

    private float angle = 0f;

    void Start()
    {
        // Tự động dò tìm nhân vật Player trên bản đồ
        if (playerTarget == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTarget = player.transform;
            }
        }

        // TẠO GÓC NGẪU NHIÊN: Đảm bảo đẻ 10 quả cầu thì 10 quả nằm rải rác trên vòng tròn
        angle = Random.Range(0f, 360f);
    }

    void Update()
    {
        // Liên tục tính toán để xoay quanh Player
        if (playerTarget != null)
        {
            angle += rotateSpeed * Time.deltaTime;

            // Dùng lượng giác để tính vị trí X, Y trên đường tròn
            float x = Mathf.Sin(angle * Mathf.Deg2Rad) * orbitRadius;
            float y = Mathf.Cos(angle * Mathf.Deg2Rad) * orbitRadius;

            transform.position = playerTarget.position + new Vector3(x, y, 0);
        }
    }

    // ==========================================
    // XỬ LÝ VA CHẠM & GÂY SÁT THƯƠNG
    // ==========================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Chạm trúng đối tượng có Tag là "Enemy"
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Bơm sát thương vào hàm TakeDamage của con quái
            collision.gameObject.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
        }
    }
}