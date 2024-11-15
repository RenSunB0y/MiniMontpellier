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

public class HorizontalLayoutBehaviour : MonoBehaviour
{
    public GameObject Card;
    public Image SelectedCardZoom;
    [SerializeField]
    private List<CardSO> cards = new List<CardSO>();
    private int actualdCardId;
    private float _cardPosY;
    private const float MIN_SPACE = 20;
    private const int SHOW_HAND_Y = 0;
    private const int HIDE_HAND_Y = -175;
    private const float HAND_TRANS_DURATION = 0.3f;
    private const float SELECTED_CARD_MOV_COEF = 300;
    private const float SELECTED_CARD_DURATION = 0.15f;
    private const float SELECTED_CARD_SCALE_COEF = 1.5f;
    
    int count = 0;
    IEnumerator FillMain()
    {
        yield return new WaitForSeconds(0.05f);
        cards.Add(cards[0]);
        UpdateMainHand();
        if(count<17)
        {
            count++;
            StartCoroutine("FillMain");
        }
    }

    void Start()
    {
        UpdateMainHand();
        StartCoroutine("FillMain");
    }

    private void UpdateMainHand()
    {
        for(int i=0; i<transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        int id = 0;
        foreach (CardSO card in cards)
        {
            var c = Instantiate(Card, Vector3.zero, Quaternion.identity, transform);
            c.GetComponent<Image>().sprite = card.sprite;
            c.GetComponent<OnPointerEnter>().MainHand = this;
            c.GetComponent<OnPointerEnter>().id = id;
            _cardPosY = c.GetComponent<RectTransform>().position.y;
            id++;
        }
        transform.GetComponent<HorizontalLayoutGroup>().spacing = -(cards.Count * 6 + MIN_SPACE);
    }

    public void ShowHand()
    {
        DOTween.To(()=> transform.GetComponent<HorizontalLayoutGroup>().padding, x=> transform.GetComponent<HorizontalLayoutGroup>().padding = x, new RectOffset(0,0,0,SHOW_HAND_Y), HAND_TRANS_DURATION);
    }

    public void HideHand()
    {
        DOTween.To(()=> transform.GetComponent<HorizontalLayoutGroup>().padding, x=> transform.GetComponent<HorizontalLayoutGroup>().padding = x, new RectOffset(0,0,0,HIDE_HAND_Y), HAND_TRANS_DURATION);
    }
    public void MouseOnCard(int id)
    {
        for(int i=0; i<transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Image>().color = new Color(0.8f,0.8f,0.8f);
        }
        actualdCardId = id;
        Image selectedCardImage = transform.GetChild(id).GetComponent<Image>();
        selectedCardImage.color = new Color(1,1,1,0);

        SelectedCardZoom.sprite = selectedCardImage.sprite;
        SelectedCardZoom.GetComponent<RectTransform>().position = selectedCardImage.GetComponent<RectTransform>().position;
        SelectedCardZoom.rectTransform.localScale = selectedCardImage.rectTransform.localScale;
        SelectedCardZoom.transform.DOMove(new Vector3(SelectedCardZoom.GetComponent<RectTransform>().position.x, _cardPosY + SELECTED_CARD_MOV_COEF*SelectedCardZoom.GetComponent<RectTransform>().lossyScale.y),SELECTED_CARD_DURATION);
        SelectedCardZoom.transform.DOScale(SelectedCardZoom.rectTransform.localScale * SELECTED_CARD_SCALE_COEF, SELECTED_CARD_DURATION);
        SelectedCardZoom.enabled = true;
    }

    public void MouseLeavesCard()
    {
        transform.GetChild(actualdCardId).GetComponent<Image>().color = new Color(0.8f,0.8f,0.8f);
        SelectedCardZoom.enabled = false;
    }

}
