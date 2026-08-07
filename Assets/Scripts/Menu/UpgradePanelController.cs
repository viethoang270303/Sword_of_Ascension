using UnityEngine;

public class UpgradePanelController : MonoBehaviour
{
    public GameObject upgradePanel;

    public void OpenUpgrade()
    {
        upgradePanel.SetActive(true);
    }

    public void CloseUpgrade()
    {
        upgradePanel.SetActive(false);
    }
}