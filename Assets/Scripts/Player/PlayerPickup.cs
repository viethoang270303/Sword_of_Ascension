using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [Header("Phạm vi nhặt EXP")]
    public float pickupRange = 2f;

    private CircleCollider2D pickupCollider;

    void Awake()
    {
        pickupCollider = GetComponent<CircleCollider2D>();

        if (pickupCollider == null)
        {
            pickupCollider = gameObject.AddComponent<CircleCollider2D>();
        }

        pickupCollider.isTrigger = true;
        pickupCollider.radius = pickupRange;
    }

    public void IncreasePickupRange(float percent)
    {
        pickupRange *= (1f + percent);

        if (pickupCollider != null)
            pickupCollider.radius = pickupRange;

        Debug.Log(
            "Pickup Range hiện tại: " +
            pickupRange
        );
    }
}