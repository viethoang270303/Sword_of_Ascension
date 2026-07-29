using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Thông số di chuyển")]
    public float speed = 5f; // Có public để SkillManager cộng tốc độ

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (moveInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }

        if (animator != null)
        {
            bool isMoving = moveInput.sqrMagnitude > 0;
            // Sửa chữ I viết hoa: "IsMoving" chuẩn 100% theo Animator của bạn
            animator.SetBool("IsMoving", isMoving);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput.normalized * speed;
    }
}