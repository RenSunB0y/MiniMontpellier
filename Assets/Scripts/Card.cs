using UnityEngine;
public class Card
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

    public Card(CardSO card)
    {
        this.SO = card;
        this.Name = SO.name;
        this.Effect = SO.effect;
        this.Color = SO.color;
        this.Type = SO.type;
        this.Cost = SO.cost;
        this.Gain = SO.gain;
        this.Dice = SO.dice;
    }

    //M thode pour augmenter gain
    public void ChangeCardGain(int newGain)
    {
        this.Gain = newGain;
    }

    public void Initialisation()
    {
    }

}
