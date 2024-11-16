using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "MiniVilles/Card")]
public class CardSO : ScriptableObject
{
    public GameObject prefab;
    public Sprite sprite;
    public string name;
    public string effect;
    public string color;
    public string type;
    public Sprite mainIcon;
    public Sprite additionalIcon;
    public int cost;
    public int gain;
    public int[] dice;
    public int amount;

    public Card Build()
    {
      var c=  Instantiate(prefab);

        var card = c.GetComponent<Card>();

        return card;
    }

    public virtual void Effect()
    {
    }

}
