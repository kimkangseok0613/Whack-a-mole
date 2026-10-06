using UnityEngine;

public class HowToPlayManager : MonoBehaviour
{
    [Header("How To Play Panel")]
    [SerializeField] private GameObject howToPlayPanel;

    private void Start()
    {
        // 게임 시작 시 패널 닫기
        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(false);
        }
    }

    public void OpenHowToPlay()
    {
        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(true);
        }
    }

    public void CloseHowToPlay()
    {
        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(false);
        }
    }
}