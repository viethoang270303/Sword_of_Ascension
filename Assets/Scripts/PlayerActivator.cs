using UnityEngine;
using Unity.Cinemachine;

public class PlayerActivator : MonoBehaviour
{
    [Header("Keo dung thu tu Player1, Player2, Player3")]
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;

    [Header("Keo CinemachineCamera trong scene vao day")]
    public CinemachineCamera vcam;

    [Header("Keo diem spawn cho player vao day")]
    public Transform spawnPoint;

    private GameObject activePlayer;

    void Awake()
    {
        int index = GameSession.SelectedCharacterIndex;

        if (player1 != null) player1.SetActive(index == 0);
        if (player2 != null) player2.SetActive(index == 1);
        if (player3 != null) player3.SetActive(index == 2);

        activePlayer = index switch
        {
            0 => player1,
            1 => player2,
            2 => player3,
            _ => player1
        };

        if (activePlayer != null && spawnPoint != null)
        {
            activePlayer.transform.position = spawnPoint.position;
        }
    }

    void Start()
    {
        if (vcam == null)
        {
            vcam = FindFirstObjectByType<CinemachineCamera>();
        }

        if (vcam != null && activePlayer != null)
        {
            // 1. Gắn mục tiêu bám theo cho Cinemachine 3.x
            vcam.Target.TrackingTarget = activePlayer.transform;

            // 2. Ép Camera dịch chuyển thẳng đến vị trí Player ngay lập tức (không bị giật góc)
            vcam.ForceCameraPosition(activePlayer.transform.position, Quaternion.identity);
        }
    }
}