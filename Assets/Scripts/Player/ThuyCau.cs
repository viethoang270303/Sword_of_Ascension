using UnityEngine;

public class DefaultThuyCau : MonoBehaviour
{
    [Header("--- Thủy Cầu ---")]
    public GameObject thuyCauPrefab;

    [Header("--- Cài đặt bay ---")]
    public float orbitRadius = 1.5f;
    public float orbitSpeed = 100f;

    [Header("--- Số lượng ---")]
    public int currentCount = 1;
    public int maxCount = 3;

    private GameObject[] thuyCaus;

    void Start()
    {
        thuyCaus = new GameObject[maxCount];

        SpawnAllThuyCau();
    }

    void Update()
    {
        RotateThuyCau();
    }

    public void AddThuyCau()
    {
        if (currentCount >= maxCount)
        {
            Debug.Log("Thủy Cầu đã đạt tối đa 3 quả!");
            return;
        }

        currentCount++;

        SpawnAllThuyCau();

        Debug.Log(
            "Đã thêm Thủy Cầu! Số lượng: " +
            currentCount + "/" + maxCount
        );
    }

    void SpawnAllThuyCau()
    {
        if (thuyCauPrefab == null)
        {
            Debug.LogWarning("Chưa gán Thủy Cầu Prefab!");
            return;
        }

        // Xóa các quả cũ
        for (int i = 0; i < thuyCaus.Length; i++)
        {
            if (thuyCaus[i] != null)
            {
                Destroy(thuyCaus[i]);
                thuyCaus[i] = null;
            }
        }

        // Tạo lại theo số lượng hiện tại
        for (int i = 0; i < currentCount; i++)
        {
            thuyCaus[i] = Instantiate(
                thuyCauPrefab,
                transform.position,
                Quaternion.identity
            );
        }
    }

    void RotateThuyCau()
    {
        if (thuyCaus == null)
            return;

        float angleStep = 360f / currentCount;

        for (int i = 0; i < currentCount; i++)
        {
            if (thuyCaus[i] == null)
                continue;

            float angle =
                (Time.time * orbitSpeed) +
                (i * angleStep);

            float rad = angle * Mathf.Deg2Rad;

            Vector3 offset = new Vector3(
                Mathf.Cos(rad) * orbitRadius,
                Mathf.Sin(rad) * orbitRadius,
                0f
            );

            thuyCaus[i].transform.position =
                transform.position + offset;
        }
    }
}