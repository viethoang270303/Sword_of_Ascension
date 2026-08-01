using UnityEngine;
using UnityEngine.SceneManagement;

public class MapLoader : MonoBehaviour
{
    public void MoManChoi(string tenScene)
    {
        SceneManager.LoadScene(tenScene);
    }
}
