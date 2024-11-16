using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardTemplateConfig : MonoBehaviour
{
    public List<Sprite> templates;
    [SerializeField]
    private Dictionary<string, int> chooseColor = new Dictionary<string, int>()
    {
        {"Bleu",0},{"Vert",1},{"Rouge",2},{"Violette",3}
    };
    public CardSO card;
    public void Start()
    {
        
    }
    public void Load(CardSO data, bool cost = true)
    {
        card = data;
        transform.GetChild(0).GetComponent<Image>().sprite = data.sprite;
        transform.GetChild(1).GetComponent<Image>().sprite = templates[chooseColor[data.color]];
        transform.GetChild(2).gameObject.SetActive(cost);
        transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = data.cost.ToString();
        transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = data.name;
        transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = $"{data.dice[0]}" + (data.dice.Length > 1 ? $"\n{data.dice[1]}" : "");
    }
}
