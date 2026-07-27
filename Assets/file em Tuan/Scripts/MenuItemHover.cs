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

    void Start()
    {
        if (label == null)
            label = GetComponentInChildren<TextMeshProUGUI>();

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
        label.color = hoverColor;
        PlaySound(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        label.color = normalColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlaySound(clickSound);
    }

    // Kích hoạt khi được chọn bằng bàn phím/tay cầm
    public void OnSelect(BaseEventData eventData)
    {
        label.color = hoverColor;
        PlaySound(hoverSound);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        label.color = normalColor;
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip, soundVolume);
        }
    }
}