using UnityEngine;

public class Player2SkillManager : MonoBehaviour
{
    [Header("Kéo Prefab Quả Cầu Nước vào đây")]
    public GameObject waterOrbPrefab;

    // Hàm này sẽ gắn vào On Click của nút "Thêm Cầu Nước"
    public void LearnWaterOrbSkill()
    {
        // Tự động tìm Player 2 đang chạy trên sân
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && waterOrbPrefab != null)
        {
            // Đẻ ra 1 quả cầu nước ngay tại vị trí của Player
            Instantiate(waterOrbPrefab, player.transform.position, Quaternion.identity);
            Debug.Log("Player 2 đã gọi thêm 1 Quả Cầu Nước!");
        }
        else
        {
            Debug.LogWarning("Thiếu mục tiêu hoặc chưa kéo Prefab Cầu Nước!");
        }

        // TODO: Viết thêm code ẩn cái Bảng Chọn Chiêu đi và cho game chạy tiếp (Time.timeScale = 1)
    }
}