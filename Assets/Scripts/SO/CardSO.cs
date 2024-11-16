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
    public int cost;
    public int gain;
    public int[] dice;
    public int amount;
}
