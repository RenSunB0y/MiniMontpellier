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
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        //---------------


    }

    //------- MUSIC
    public static UnityEvent onMainMenuPlay = new UnityEvent();
    public static UnityEvent onMainMenuExit = new UnityEvent();

    public static UnityEvent onGameMusicPlay = new UnityEvent();
    public static UnityEvent onGameMusicExit = new UnityEvent();

    public static UnityEvent onShopEnter = new UnityEvent();
    public static UnityEvent onShopExit = new UnityEvent();

    //------- SOUND EFFECT

    //Coin
    public static UnityEvent onCoinSound_1 = new UnityEvent();
    public static UnityEvent onCoinSound_2 = new UnityEvent();

    //Dice
    public static UnityEvent onDicePick_1 = new UnityEvent();
    public static UnityEvent onDiceRoll_1 = new UnityEvent();
    public static UnityEvent onDiceRoll_2 = new UnityEvent();
    public static UnityEvent onDiceShake = new UnityEvent();

    //Gain/Lose
    public  static UnityEvent onGain = new UnityEvent();
    public  static UnityEvent onLose = new UnityEvent();

    //Cards

    //Button
    public  static UnityEvent onButtonSound = new UnityEvent();
}
