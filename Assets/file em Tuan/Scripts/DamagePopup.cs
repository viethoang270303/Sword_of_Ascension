using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public float destroyTime = 0.5f;
    public float floatSpeed = 2f;

    private TextMeshPro textMesh;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
    }

    // Nâng cấp: Thêm biến isPlayerDamage để phân biệt
    public void Setup(int damageAmount, bool isPlayerDamage = false)
    {
        if (textMesh != null)
        {
            textMesh.text = "-" + damageAmount.ToString();

            if (isPlayerDamage)
            {
                // Player mất máu -> Hiện chữ màu ĐỎ
                textMesh.color = Color.red;
            }
            else
            {
                // Quái nhảy dame -> Hiện chữ màu TRẮNG
                textMesh.color = Color.white;
            }
        }
    }
}