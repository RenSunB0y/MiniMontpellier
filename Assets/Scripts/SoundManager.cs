using UnityEngine;
using UnityEngine.Events;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //---- Singleton
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        //---------------


    }

    //------- MUSIC
    public UnityEvent onMainMenuPlay = new UnityEvent();
    public UnityEvent onMainMenuExit = new UnityEvent();

    public UnityEvent onGameMusicPlay = new UnityEvent();
    public UnityEvent onGameMusicExit = new UnityEvent();

    public UnityEvent onShopEnter = new UnityEvent();
    public UnityEvent onShopExit = new UnityEvent();

    //------- SOUND EFFECT

    //Coin
    public UnityEvent onCoinSound_1 = new UnityEvent();
    public UnityEvent onCoinSound_2 = new UnityEvent();

    //Dice
    public UnityEvent onDicePick_1 = new UnityEvent();
    public UnityEvent onDiceRoll_1 = new UnityEvent();
    public UnityEvent onDiceRoll_2 = new UnityEvent();
    public UnityEvent onDiceShake = new UnityEvent();

    //Gain/Lose
    public UnityEvent onGain = new UnityEvent();
    public UnityEvent onLose = new UnityEvent();

    //Cards

    //Button
    public UnityEvent onButtonSound = new UnityEvent();
    public UnityEvent onWhoosh = new UnityEvent();
}
