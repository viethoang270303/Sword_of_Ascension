using UnityEngine;
using Unity.Cinemachine;

public class CameraFollowMe : MonoBehaviour
{
    void Start()
    {
        // Tự động đi tìm cái Camera trong màn chơi
        CinemachineCamera vcam = FindFirstObjectByType<CinemachineCamera>();

        if (vcam != null)
        {
            // Bắt Camera phải bám theo chính cái nhân vật đang chứa script này
            vcam.Target.TrackingTarget = this.transform;

            // Ép Camera nhảy ngay đến vị trí nhân vật (chống giật màn hình)
            vcam.ForceCameraPosition(transform.position, Quaternion.identity);
        }
    }
}