using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MenuItemHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler
{
    [Header("Hover Settings")]
    public TextMeshProUGUI label;
    public Color normalColor = Color.white;
    public Color hoverColor = new Color(1f, 0.85f, 0.3f);

    [Header("Sound")]
    public AudioClip hoverSound;
    public AudioClip clickSound;
    [Range(0f, 1f)] public float soundVolume = 1f;

    private static AudioSource audioSource;

    // ĐỔI TỪ Start() THÀNH Awake() ĐỂ CHẠY TRƯỚC TIÊN
    void Awake()
    {
        if (label == null)
            label = GetComponentInChildren<TextMeshProUGUI>();

        if (label != null)
            label.color = normalColor;

        if (audioSource == null)
        {
            GameObject audioObj = new GameObject("MenuAudioSource");
            audioSource = audioObj.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (label != null) label.color = hoverColor;
        PlaySound(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (label != null) label.color = normalColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlaySound(clickSound);
    }

    // Kích hoạt khi được chọn bằng bàn phím/tay cầm hoặc Auto Select
    public void OnSelect(BaseEventData eventData)
    {
        // Thêm kiểm tra null cho chắc ăn tuyệt đối
        if (label != null) label.color = hoverColor;
        PlaySound(hoverSound);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (label != null) label.color = normalColor;
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip, soundVolume);
        }
    }
}