using System.Collections.Generic;
using NUnit.Framework.Internal;
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
    public Image monumentIcon;
    public TextMeshProUGUI gain;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI amount;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI effect;
    public TextMeshProUGUI dice;
    #endregion

    public List<Sprite> templates;
    public List<Sprite> icons;
    private List<string> templatesChoices = new List<string>{"Bleu","Vert","Rouge","Violette","Jaune","Grise"};
    private List<string> iconsChoices = new List<string>{"Agriculture","Élevage","Commerce","Consommation","Ressource","Spécial","Usine","Marché"};
    public CardSO card;
    public bool isInShop;

    public const int test = 1;
    public void Awake()
    {
        if(card!=null)
            Load(card,isInShop);
    }
    public void Load(CardSO data, bool inShop)
    {
        Debug.Log(isInShop);
        isInShop = inShop;
        card = data;
        art.sprite = data.sprite; // Illustration
        string iconName = data.type == "Monument" ? "Spécial" : data.type;
        icon.sprite = icons[iconsChoices.IndexOf(iconName)];
        // if(data.type=="Monument")
        //     monumentIcon.sprite = data.additionalIcon;
        cardTemplate.sprite = templates[templatesChoices.IndexOf(data.color)]; // Template
        if(inShop)
        {
            if(data.amount==0)
            {
                cardTemplate.sprite = templates[5];
                art.color = new Color(0.7f,0.7f,0.7f);
                inShop = false;
            }

            amount.text = data.amount.ToString();
            cost.text = data.cost.ToString();
        }
        amount.gameObject.SetActive(inShop);
        cost.transform.parent.gameObject.SetActive(inShop);
        nameText.text = data.name;
        effect.text = data.effect;
        if(card.dice.Length>0)
        {
            dice.text = $"{data.dice[0]}" + (data.dice.Length > 1 ? $"\n{data.dice[1]}" : "");
            dice.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            dice.text = "";
            monumentIcon.sprite = card.additionalIcon;
            dice.transform.GetChild(0).gameObject.SetActive(true);
        }

        // Affichage du gain : 
        // Récupérer le gain total par code -> cv être chaud..
        // if(data.gain==0) 
        //     gain.text = data.gain.ToString(); // 
    }

    public bool PrintInShop()
    {
        if(card.amount>0 && isInShop)
            return true;
        else
            return false;
    }
}
