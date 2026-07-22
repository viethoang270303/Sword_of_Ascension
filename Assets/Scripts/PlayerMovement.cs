using UnityEngine;
using UnityEngine.InputSystem; // Thư viện mới để nhận diện tay cầm chuẩn xác

public class PlayerMovement : MonoBehaviour
{
    [Header("Thông số di chuyển")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private Vector2 movement;
    private InputAction moveAction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 1. Cấu hình các nút bấm và tay cầm ngay trong code
        moveAction = new InputAction("Move");

        // Thêm cần Analog trái và D-pad của tay cầm Xbox
        moveAction.AddBinding("<Gamepad>/leftStick");
        moveAction.AddBinding("<Gamepad>/dpad");

        // Thêm nhóm phím WASD
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        // Thêm nhóm phím Mũi tên
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/rightArrow");

        // Bật bộ thu tín hiệu
        moveAction.Enable();
    }

    void Update()
    {
        // 2. Đọc giá trị hướng đi (Tự động nhận diện thiết bị bạn đang bấm)
        movement = moveAction.ReadValue<Vector2>();

        // 3. Xử lý Animation và lật ảnh (Flip)
        if (movement != Vector2.zero)
        {
            anim.SetBool("IsMoving", true);

            if (movement.x < 0) spriteRenderer.flipX = true;
            else if (movement.x > 0) spriteRenderer.flipX = false;
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }
    }

    void FixedUpdate()
    {
        // 4. Di chuyển vật lý (Cú pháp chuẩn cho Unity 6 LTS)
        rb.linearVelocity = movement * moveSpeed;
    }

    void OnDestroy()
    {
        // Tắt hành động khi nhân vật bị hủy hoặc chuyển màn chơi để tránh lỗi
        if (moveAction != null) moveAction.Disable();
    }
}