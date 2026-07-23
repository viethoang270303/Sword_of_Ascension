using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [Header("Thông số viên đạn")]
    [Tooltip("Tốc độ bay của đạn - Chỉnh số NHỎ đi để đạn bay CHẬM lại")]
    public float speed = 3f;

    [Tooltip("Thời gian tự hủy nếu không trúng ai (giây)")]
    public float lifeTime = 5f;

    void Start()
    {
        // Tự động xóa viên đạn sau vài giây để không làm nặng máy
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Lệnh bay thẳng về phía trước theo hướng nòng súng
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}