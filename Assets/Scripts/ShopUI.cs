using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using GameLogic;

public class ShopUI : MonoBehaviour, IPointerEvents
{
    private GameObject SelectedCardZoom;
    private Vector3 _defaultScale;

    private const float SELECTED_CARD_SCALE_COEF = 1.3f;
    private const float SELECTED_CARD_DURATION = 0.2f;

    void Start()
    {
        Init();
    }

    void Init()
    {
        int id = 0;
        List<CardSO> cardsSorted = GameManager.Instance.cards;
        for(int i=0; i<4; i++)
            cardsSorted.RemoveAt(cardsSorted.Count-1);

        cardsSorted = cardsSorted.OrderBy(card => card.dice[0]).ThenBy(card=>card.dice.Length).ToList();
        foreach(CardSO card in cardsSorted)
        {
            var c = Instantiate(card.prefab, transform);
            c.GetComponent<CardTemplateConfig>().Load(card,card.amount,true,transform.tag);
            _defaultScale = c.transform.localScale;
            id++;
        }
    }

    public void MouseOnCard(GameObject sender)
    {
        SelectedCardZoom = Instantiate(sender, GameObject.FindGameObjectWithTag("Canvas").transform);
        SelectedCardZoom.GetComponent<Image>().enabled = false;
        SelectedCardZoom.transform.position = sender.transform.position;
        SelectedCardZoom.transform.localScale = sender.transform.localScale;
        SelectedCardZoom.transform.DOScale(SelectedCardZoom.transform.localScale * SELECTED_CARD_SCALE_COEF, SELECTED_CARD_DURATION);
    }

    public void MouseLeavesCard(GameObject sender)
    {
        Destroy(SelectedCardZoom);
    }

    public void MouseClickCard(GameObject sender)
    {
        GameManager.Instance.currentPlayer.GetComponent<Player>().Deck.AddCard(new Card(sender.GetComponent<CardTemplateConfig>().cardSO), true, GameManager.Instance.currentPlayer.GetComponent<Player>());
    }
}
