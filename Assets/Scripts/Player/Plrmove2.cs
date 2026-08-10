using UnityEngine;

public class Plrmove2 : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim; // Thêm biến Animator

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>(); // Lấy Animator gắn trên nhân vật
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(moveX, moveY).normalized;
        rb.linearVelocity = movement * speed;

        // Lật ảnh khi quay trái/phải
        if (moveX < 0) sr.flipX = false;
        else if (moveX > 0) sr.flipX = true;

        // --- CODE ANIMATION ---
        // Nếu nhân vật đang di chuyển (độ lớn vector > 0) -> Bật IsMoving = true
        if (anim != null)
        {
            if (movement.magnitude > 0)
            {
                anim.SetBool("IsMoving", true);
            }
            else
            {
                anim.SetBool("IsMoving", false);
            }
        }
    }
}