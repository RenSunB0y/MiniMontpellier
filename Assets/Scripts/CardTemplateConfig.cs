using System.Collections.Generic;
using System.Xml.Xsl;
using NUnit.Framework.Internal;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;

public class CardTemplateConfig : MonoBehaviour
{
    #region Prefab's children
    public Image art;
    public Image cardTemplate;
    public List<Color> templatesColors;
    public Image icon;
    public Image monumentIcon;
    public TextMeshProUGUI gain;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI amount;
    public Transform amountVisual;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI effect;
    public TextMeshProUGUI dice;
    #endregion

    public List<Sprite> templates;
    public List<Sprite> icons;
    private List<string> templatesChoices = new List<string>{"Bleu","Vert","Rouge","Violette","Jaune","Grise"};
    private Dictionary<string,Color> colorChoice = new Dictionary<string, Color>();
    public CardSO card;
    public bool isInShop;

    public const int test = 1;
    public void Awake()
    {
        colorChoice = new Dictionary<string, Color>()
        {
            {"Bleu",templatesColors[0]},
            {"Vert",templatesColors[1]},
            {"Rouge",templatesColors[2]},
            {"Violette",templatesColors[3]},
            {"Jaune",templatesColors[4]},
            {"Grise",templatesColors[5]},
        };
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
        icon.sprite = data.mainIcon;
        cardTemplate.sprite = templates[templatesChoices.IndexOf(data.color)]; // Template

        int t = Random.Range(1,7);

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
        else
        {
            for(int i=0; i<amountVisual.childCount; i++)
            {
                amountVisual.GetChild(i).GetComponent<Image>().color = templatesColors[templatesColors.IndexOf(colorChoice[data.color])];
                amountVisual.GetChild(i).gameObject.SetActive(i<t && t>=2);
            }
            amountVisual.GetComponent<Image>().enabled = t>=2;
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
            monumentIcon.sprite = card.mainIcon;
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
