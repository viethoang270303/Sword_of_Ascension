using UnityEngine;

public class ExpGem : MonoBehaviour
{
    [Tooltip("Lượng kinh nghiệm cục này cho")]
    public int expAmount = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu người chạm vào là Player
        if (other.CompareTag("Player"))
        {
            // Tìm kịch bản chứa Level của Player
            PlayerLevel playerLevel = other.GetComponent<PlayerLevel>();
            if (playerLevel != null)
            {
                playerLevel.AddExp(expAmount); // Cộng điểm
                Destroy(gameObject);           // Ăn xong thì xóa cục Exp đi
            }
        }
    }
}