using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void PlayerSelection()
    {
        SceneManager.LoadScene("PlayerSelection");

        //SoundManager.instance.onMainMenuExit.Invoke(); 
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Game Scene");

        //SoundManager.instance.onMainMusic.Invoke(); //test

        //SoundManager.instance.onMainMenuExit.Invoke();
        //SoundManager.instance.onGameMusicPlay.Invoke();
    }
}
