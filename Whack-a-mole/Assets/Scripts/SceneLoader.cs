using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene("Game");
    }
}