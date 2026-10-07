using UnityEngine;

public class HowToPlayManager : MonoBehaviour
{
    [Header("How To Play Panel")]
    [SerializeField] private GameObject howToPlayPanel;
    private void Awake()
    {
        CloseHowToPlay();
    }

    public void OpenHowToPlay()
    {
        SetPanel(true);
    }

    public void CloseHowToPlay()
    {
        SetPanel(false);
    }

    private void SetPanel(bool active)
    {
        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(active);
        }
    }
}