using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [Header("Phạm vi hút vật phẩm")]
    public float pickupRange = 2f;

    private CircleCollider2D pickupCollider;

    void Awake()
    {
        pickupCollider = gameObject.AddComponent<CircleCollider2D>();
        pickupCollider.isTrigger = true;
        pickupCollider.radius = pickupRange;
    }

    public void IncreasePickupRange(float percent)
    {
        pickupRange *= (1f + percent);
        pickupCollider.radius = pickupRange;
    }
}
