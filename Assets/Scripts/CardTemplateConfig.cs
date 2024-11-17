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
    [SerializeField]
    private bool loadOnStart = false;
    #region Prefab's children
    public Image art;
    public Image cardTemplate;
    public List<Color> templatesColors;
    public Image icon;
    public Image monumentIcon;
    public Image subTemplate;
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
    public Sprite cantPurchaseSprite;
    public Sprite selectedSprite;
    private List<string> templatesChoices = new List<string>{"Bleu","Vert","Rouge","Violette","Jaune","Grise"};
    private Dictionary<string,Color> colorChoice = new Dictionary<string, Color>();
    public CardSO cardSO;
    public int exemplaries;
    public string parentScriptTag;
    public bool isInShop;
    public bool interactable;

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
        if(loadOnStart)
            Load(cardSO,-1,true,"Shop");
    }
    public void Load(CardSO dataSO, int exemp, bool inShop, string tag)
    {
        cardSO = dataSO;
        exemplaries = exemp;
        parentScriptTag = tag;
        isInShop = inShop;
        // isInShop = inShop;
        art.sprite = cardSO.sprite; 
        icon.sprite = cardSO.mainIcon;
        cardTemplate.sprite = templates[exemp==-1 ? 5 : templatesChoices.IndexOf(cardSO.color)];

        if(isInShop)
        {
            if(cardSO.amount==0) // ou joueur ne peut pas acheter;
            {
                cardTemplate.sprite = templates[5];
                art.color = new Color(0.7f,0.7f,0.7f);
            }

            amount.text = exemp > 0 ? cardSO.amount.ToString() : "";
            cost.text = cardSO.cost.ToString();
        }
        else
        {
            for(int i=0; i<amountVisual.childCount; i++)
            {
                amountVisual.GetChild(i).GetComponent<Image>().color = templatesColors[templatesColors.IndexOf(colorChoice[cardSO.color])];
                amountVisual.GetChild(i).gameObject.SetActive(i<exemp && exemp>=2);
            }
            amountVisual.GetComponent<Image>().enabled = exemp>=2;
        }

        amount.gameObject.SetActive(isInShop);
        cost.transform.parent.gameObject.SetActive(inShop);
        nameText.text = cardSO.name;
        effect.text = cardSO.effect;
        if(cardSO.dice.Length>0)
        {
            dice.text = $"{cardSO.dice[0]}" + (cardSO.dice.Length > 1 ? $"\n{cardSO.dice[1]}" : "");
            dice.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            dice.text = "";
            monumentIcon.sprite = cardSO.mainIcon;
            dice.transform.GetChild(0).gameObject.SetActive(true);
        }

        // Affichage du gain : 
        // Récupérer le gain total par code -> cv être chaud..
        // if(data.gain==0) 
        //     gain.text = data.gain.ToString(); // 
        interactable = isInteractable();
        if(!interactable)
        {
            subTemplate.sprite = cantPurchaseSprite;
            subTemplate.enabled = true;
        }
    }

    private bool isInteractable()
    {
        if(cardSO.amount<=0) // ou joueur n'a plus assez de pièces
            return false;
        return true;
    }

    public void SelectCard(bool state)
    {
        subTemplate.sprite = selectedSprite;
        subTemplate.enabled = state;
    }
}
