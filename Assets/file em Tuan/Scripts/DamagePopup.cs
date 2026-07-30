using UnityEngine;
using TMPro; // Dùng TextMeshPro cho xịn

public class DamagePopup : MonoBehaviour
{
    public float destroyTime = 0.5f;
    public float floatSpeed = 2f;

    [Header("Kéo TextMeshPro vào đây")]
    public TextMeshPro textMesh;

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        // Chữ tự động bay lơ lửng lên trên
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
    }

    // Hàm này để quái vật truyền số sát thương vào
    public void Setup(int damageAmount)
    {
        if (textMesh != null)
        {
            textMesh.text = "-" + damageAmount.ToString();
        }
    }
}