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

public class MainHandUI : MonoBehaviour, IPointerEvents
{
    public GameObject SelectedCardZoom;
    [SerializeField]
    private List<CardSO> cards = new List<CardSO>();
    private float _cardPosY;
    private const float MIN_SPACE = 20;
    private const int SHOW_HAND_Y = 0;
    private const int HIDE_HAND_Y = -175;
    private const float HAND_TRANS_DURATION = 0.3f;
    private const float SELECTED_CARD_MOV_COEF = 300;
    private const float SELECTED_CARD_DURATION = 0.15f;
    private const float SELECTED_CARD_SCALE_COEF = 1.3f;
    
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
        HideHand();
        StartCoroutine("FillMain");
    }

    private void UpdateMainHand()
    {
        for(int i=0; i<transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        int id = 0;
        foreach (CardSO card in cards) // Recup deck du joueur
        {
            var c = Instantiate(card.prefab, transform);
            c.GetComponent<CardTemplateConfig>().Load(card,false);
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
    public void MouseOnCard(CardSO data, int id)
    {
        for(int i=0; i<transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Image>().color = new Color(0.8f,0.8f,0.8f);
        }

        Image selectedCardImage = transform.GetChild(id).GetComponent<Image>();
        selectedCardImage.color = new Color(1,1,1,0);

        SelectedCardZoom.GetComponent<CardTemplateConfig>().Load(data);
        SelectedCardZoom.transform.position = selectedCardImage.GetComponent<RectTransform>().position;
        SelectedCardZoom.transform.localScale = selectedCardImage.rectTransform.localScale;

        SelectedCardZoom.transform.DOMove(new Vector3(SelectedCardZoom.GetComponent<RectTransform>().position.x, _cardPosY + SELECTED_CARD_MOV_COEF*SelectedCardZoom.GetComponent<RectTransform>().lossyScale.y),SELECTED_CARD_DURATION);
        SelectedCardZoom.transform.DOScale(SelectedCardZoom.transform.localScale * SELECTED_CARD_SCALE_COEF, SELECTED_CARD_DURATION);

        for(int i=0; i<SelectedCardZoom.transform.childCount; i++)
            SelectedCardZoom.transform.GetChild(i).gameObject.SetActive(i!=2);
    }

    public void MouseLeavesCard()
    {
        for(int i=0; i<SelectedCardZoom.transform.childCount; i++)
            SelectedCardZoom.transform.GetChild(i).gameObject.SetActive(false);
    }

}
