using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ShopUI : MonoBehaviour, IPointerEvents
{
    public GameObject SelectedCardZoom;
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
        SelectedCardZoom.transform.position = sender.transform.position;
        SelectedCardZoom.transform.localScale = _defaultScale;
        SelectedCardZoom.transform.DOScale(_defaultScale * SELECTED_CARD_SCALE_COEF, SELECTED_CARD_DURATION);
        for(int i=0; i<SelectedCardZoom.transform.childCount; i++)
            SelectedCardZoom.transform.GetChild(i).gameObject.SetActive(true);
        SelectedCardZoom.GetComponent<CardTemplateConfig>().Load(sender.GetComponent<CardTemplateConfig>().cardSO,1,true,transform.tag);
    }

    public void MouseLeavesCard(GameObject sender)
    {
        for(int i=0; i<SelectedCardZoom.transform.childCount; i++)
            SelectedCardZoom.transform.GetChild(i).gameObject.SetActive(false);
    }

    public void MouseClickCard(GameObject sender)
    {
        Debug.Log($"Carte achetée : {sender.GetComponent<CardTemplateConfig>().cardSO.name}");
    }
}
