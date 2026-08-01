using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public Slider volumeSlider;

    void Start()
    {
        // Lấy âm lượng đã lưu lần trước (nếu có), mặc định 0.5
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        musicSource.volume = savedVolume;
        volumeSlider.value = savedVolume;

        // Lắng nghe khi người dùng kéo slider
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float value)
    {
        musicSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value); // lưu lại để lần sau mở game vẫn giữ nguyên
    }
}