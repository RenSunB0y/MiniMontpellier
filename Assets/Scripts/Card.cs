using UnityEngine;
public class Cards : MonoBehaviour
{
    //Attributs
    public CardSO SO;
    public string Name;
    public string Effect;
    public string Color;
    public string Type;
    public int Cost;
    public int Gain; //peut changer
    public int[] Dice;

    //Constructeur
    public Cards(CardSO cardSO)
    {
        SO = cardSO;
        this.Name = cardSO.name;
        this.Effect = cardSO.effect;
        this.Color = cardSO.color;
        this.Cost = cardSO.cost;
        this.Gain = cardSO.gain;
        this.Dice = cardSO.dice;
        //this.type = cardSO.type;
    }

    //M thode pour augmenter gain
    public void ChangeCardGain(int newGain)
    {
        this.Gain = newGain;
    }

}
