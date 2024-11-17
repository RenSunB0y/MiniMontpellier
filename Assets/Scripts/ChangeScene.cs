using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void PlayerSelection()
    {
        SceneManager.LoadScene("PlayerSelection");
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Game Scene");
    }
}
