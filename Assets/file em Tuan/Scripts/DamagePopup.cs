using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [Header("Cài đặt Hiệu ứng")]
    public float moveSpeed = 2f;
    public float destroyTime = 1f;

    private TextMeshPro textMesh;
    private Color textColor;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        textColor = textMesh.color;

        // --- ÉP CHỮ LUÔN HIỆN LÊN TRÊN CÙNG ---
        // Dòng này giúp chữ đè lên cỏ, quái vật và cả nhân vật (khỏi cần chỉnh tay)
        textMesh.sortingOrder = 100;

        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        textColor.a -= (1f / destroyTime) * Time.deltaTime;
        textMesh.color = textColor;
    }

    public void Setup(int damageAmount)
    {
        textMesh.text = "-" + damageAmount.ToString();
    }
}