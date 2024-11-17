using System.Collections.Generic;
using GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class EnemyPanelUI : MonoBehaviour
{
    #region Prefab's children
    public TextMeshProUGUI coinsCount;
    public TextMeshProUGUI cardsCount;
    public Transform monuments;
    public Sprite[] monumentsIcon;
    private List<string> monumentsId = new List<string>{"Gare","Centre Commercial","Tour Radio","Parc"};
    public Sprite playerIcon;
    public TextMeshProUGUI playerName;
    #endregion

    private Player player;
    private Dictionary<Card,int> deck;

    public void Init(Player player)
    {
        this.player = player;
        deck = player.GetComponent<Player>().Deck.Pile;
        UpdatePanel();
    }
    public void UpdateSecondaryHand(BaseEventData eventData)
    {
        GameObject.FindGameObjectWithTag("SecondaryHand").GetComponent<MainHandUI>().UpdateMainHand(deck);
    }

    public void UpdatePanel()
    {
        playerName.text = player.playerName;

        int cardsAmount = 0;
        foreach(Card card in deck.Keys)
        {
            cardsAmount += deck[card];
            if(monumentsId.Contains(card.Name))
            {
                monuments.GetChild(monumentsId.IndexOf(card.Name)).GetComponent<Image>().sprite = monumentsIcon[monumentsId.IndexOf(card.Name)];
            }
        }

        coinsCount.text = "x " + player.coins.ToString();
        cardsCount.text = "x" + cardsAmount;

    }
}
