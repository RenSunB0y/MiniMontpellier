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

public class MainHandUI : MonoBehaviour, IPointerEvents
{
    public GameObject SelectedCardZoom;
    [SerializeField]
    private List<CardSO> cards = new List<CardSO>();
    [SerializeField]
    private Vector2 _cardPos;
    private const float MIN_SPACE = 20;
    private const float SELECTED_CARD_MOV_COEF = 300;
    private const float SELECTED_CARD_DURATION = 0.15f;
    private const float SELECTED_CARD_SCALE_COEF = 1.3f;
    
    int count = 0;  
    // IEnumerator FillMain()
    // {
    //     yield return new WaitForSeconds(0.5f);
    //     cards.Add(cards[0]);
    //     UpdateMainHand();
    //     if(count<17)
    //     {
    //         count++;
    //         StartCoroutine("FillMain");
    //     }
    // }

    void Start()
    {
        UpdateMainHand();
        // StartCoroutine("FillMain");
    }

    private void UpdateMainHand()
    {
        for(int i=0; i<transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        int id = 0;
        var deck = cards.OrderByDescending(card => card.dice.Length>0 ? card.dice[0] : int.MaxValue).ThenByDescending(card => card.dice.Length);
        foreach (CardSO card in deck) // Recup deck du joueur
        {
            var c = Instantiate(card.prefab, transform);
            c.GetComponent<CardTemplateConfig>().Load(card,false,transform.tag);
            _cardPos = c.transform.position;
            id++;
        }
        transform.GetComponent<HorizontalLayoutGroup>().spacing = -(cards.Count * 6.5f + MIN_SPACE);
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
        SelectedCardZoom.GetComponent<CardTemplateConfig>().Load(sender.GetComponent<CardTemplateConfig>().card,false,transform.tag);
    }

    public void MouseLeavesCard(GameObject sender)
    {
        for(int i=0; i<sender.transform.childCount; i++)
            sender.transform.GetChild(i).gameObject.SetActive(true);
        sender.GetComponent<CardTemplateConfig>().Load(sender.GetComponent<CardTemplateConfig>().card,false,transform.tag);
        sender.GetComponent<Image>().color = new Color(1,1,1,1);
        for(int i=0; i<SelectedCardZoom.transform.childCount; i++)
            SelectedCardZoom.transform.GetChild(i).gameObject.SetActive(false);
    }

    public void MouseClickCard(GameObject sender)
    {
        if(transform.tag == "MainHand")
            Debug.Log($"Carte du joueur sélectionnée : {sender.GetComponent<CardTemplateConfig>().card.name}");
        else if(transform.tag == "SecondaryHand")
            Debug.Log($"Carte de l'ennemi sélectionnée : {sender.GetComponent<CardTemplateConfig>().card.name}");
    }

}
