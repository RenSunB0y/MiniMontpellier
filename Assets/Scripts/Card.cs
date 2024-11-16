using UnityEngine;
public class Card : MonoBehaviour
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

    private void Start()
    {
        Initialisation();
    }

    //M thode pour augmenter gain
    public void ChangeCardGain(int newGain)
    {
        this.Gain = newGain;
    }

    public void Initialisation()
    {
        this.Name = SO.name;
        this.Effect = SO.effect;
        this.Color = SO.color;
        this.Type = SO.type;
        this.Cost = SO.cost;
        this.Gain = SO.gain;
        this.Dice = SO.dice;
    }

}
