using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardTemplateConfig : MonoBehaviour
{
    #region Prefab's children
    public Image art;
    public Image cardTemplate;
    public Image icon;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI effect;
    public TextMeshProUGUI dice;
    #endregion
    public List<Sprite> templates;
    public List<Sprite> icons;
    private List<string> templatesChoices = new List<string>{"Bleu","Vert","Rouge","Violette"};
    private List<string> iconsChoices = new List<string>{"Agriculture","Élevage","Commerce","Consommation","Ressource","Spécial","Usine","Marché"};
    public CardSO card;
    public void Start()
    {
        
    }
    public void Load(CardSO data, bool showCost = true)
    {
        card = data;
        art.sprite = data.sprite; // Illustration
        icon.sprite = icons[iconsChoices.IndexOf(data.type)];
        cardTemplate.sprite = templates[templatesChoices.IndexOf(data.color)]; // Template
        if(showCost)
            transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = data.cost.ToString();
        cost.transform.parent.gameObject.SetActive(showCost);
        nameText.text = data.name;
        effect.text = data.effect;
        dice.text = $"{data.dice[0]}" + (data.dice.Length > 1 ? $"\n{data.dice[1]}" : "");
    }
}
