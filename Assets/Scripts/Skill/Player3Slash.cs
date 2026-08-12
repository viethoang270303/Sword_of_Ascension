using UnityEngine;

public class AutoSlash : MonoBehaviour
{
    [Header("--- Cài đặt Chém Tự Động Theo Nhịp ---")]
    public GameObject slashPrefab;    // Kéo Prefab nhát chém vào đây
    public float attackRate = 0.2f;   // Tốc độ chém (càng nhỏ chém càng nhanh)

    [Header("--- Khoảng cách vệt chém ---")]
    [Tooltip("Khoảng cách từ người Player ra đến vị trí vệt chém xuất hiện")]
    public float spawnDistance = 1.5f; // Bác có thể chỉnh số này trực tiếp ở Inspector

    private float nextAttack = 0f;
    private bool switchSide = false;  // Biến luân phiên chém trái / phải

    void Update()
    {
        // Cứ đến hẹn lại vung kiếm liên tục vô điều kiện
        if (Time.time >= nextAttack)
        {
            nextAttack = Time.time + attackRate;
            PerformContinuousSlash();
        }
    }

    void PerformContinuousSlash()
    {
        Vector3 spawnPos;
        Quaternion rot;

        // Luân phiên đổi bên chém: Trái - Phải
        switchSide = !switchSide;

        if (switchSide)
        {
            // Chém bên TRÁI (Dùng biến spawnDistance để cách xa người ra)
            spawnPos = transform.position + new Vector3(-spawnDistance, 0f, 0f);
            rot = Quaternion.Euler(0, 180f, 0);
        }
        else
        {
            // Chém bên PHẢI
            spawnPos = transform.position + new Vector3(spawnDistance, 0f, 0f);
            rot = Quaternion.Euler(0, 0f, 0);
        }

        // Sinh ra nhát chém
        Instantiate(slashPrefab, spawnPos, rot);
    }
}