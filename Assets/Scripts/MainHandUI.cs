using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using System.Data.Common;
using System.Security.Cryptography;
using System;
using System.Linq;
using GameLogic;

public class MainHandUI : MonoBehaviour, IPointerEvents
{
    public GameObject SelectedCardZoom;
    [SerializeField]
    private Vector2 _cardPos;
    private const float MIN_SPACE = 20;
    private const float SELECTED_CARD_MOV_COEF = 300;
    private const float SELECTED_CARD_DURATION = 0.15f;
    private const float SELECTED_CARD_SCALE_COEF = 1.3f;
    int count = 0;
    public void UpdateMainHand(Dictionary<Card,int> deck)
    {
        for(int i=0; i<transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        int id = 0;
        var dec = deck.OrderByDescending(card => card.Key.SO.dice.Length>0 ? card.Key.SO.dice[0] : int.MaxValue).ThenByDescending(card => card.Key.SO.dice.Length);
        foreach (KeyValuePair<Card,int> card in GameManager.Instance.currentPlayer.GetComponent<Player>().Deck.Pile)
        {
            var c = Instantiate(card.Key.SO.prefab, transform);
            c.GetComponent<CardTemplateConfig>().Load(card.Key.SO,card.Value,false,transform.tag);
            _cardPos = c.transform.position;
            id++;
        }
        transform.GetComponent<HorizontalLayoutGroup>().spacing = -(deck.Count * 6.5f + MIN_SPACE);
    }

    public void MouseOnCard(GameObject sender)
    {
        for(int i=0; i<transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Image>().color = new Color(0.8f,0.8f,0.8f);
        }

        for(int i=0; i<sender.transform.childCount; i++)
            sender.transform.GetChild(i).gameObject.SetActive(false);
        sender.GetComponent<Image>().color = new Color(0,0,0,0);

        SelectedCardZoom.transform.position = sender.transform.position;
        SelectedCardZoom.transform.localScale = sender.transform.localScale;
        SelectedCardZoom.transform.DOMove(new Vector3(SelectedCardZoom.transform.position.x, _cardPos.y + SELECTED_CARD_MOV_COEF*SelectedCardZoom.transform.lossyScale.y),SELECTED_CARD_DURATION);
        SelectedCardZoom.transform.DOScale(SelectedCardZoom.transform.localScale * SELECTED_CARD_SCALE_COEF, SELECTED_CARD_DURATION);

        for(int i=0; i<SelectedCardZoom.transform.childCount; i++)
            SelectedCardZoom.transform.GetChild(i).gameObject.SetActive(true);
        SelectedCardZoom.GetComponent<CardTemplateConfig>().Load(sender.GetComponent<CardTemplateConfig>().cardSO,sender.GetComponent<CardTemplateConfig>().exemplaries,false,transform.tag);
    }

    public void MouseLeavesCard(GameObject sender)
    {
        for(int i=0; i<sender.transform.childCount; i++)
            sender.transform.GetChild(i).gameObject.SetActive(true);
        sender.GetComponent<CardTemplateConfig>().Load(sender.GetComponent<CardTemplateConfig>().cardSO,sender.GetComponent<CardTemplateConfig>().exemplaries,false,transform.tag);
        sender.GetComponent<Image>().color = new Color(1,1,1,1);
        for(int i=0; i<SelectedCardZoom.transform.childCount; i++)
            SelectedCardZoom.transform.GetChild(i).gameObject.SetActive(false);
    }

    public void MouseClickCard(GameObject sender)
    {
        if(transform.tag == "MainHand")
            Debug.Log($"Carte du joueur sélectionnée : {sender.GetComponent<CardTemplateConfig>().cardSO.name}");
        else if(transform.tag == "SecondaryHand")
            Debug.Log($"Carte de l'ennemi sélectionnée : {sender.GetComponent<CardTemplateConfig>().cardSO.name}");
    }

}
