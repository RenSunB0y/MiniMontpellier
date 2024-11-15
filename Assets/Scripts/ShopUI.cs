using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour, IPointerEvents
{
    public Image SelectedCardZoom;
    public List<CardSO> cardsType; 
    public Dictionary <CardSO, int> DrawPile = new Dictionary<CardSO, int>();
    private Vector3 _defaultScale;
    private int actualdCardId;

    // IEnumerator Scaling()
    // {
    //     yield return new WaitForSeconds(duration);
    //     for(int i=0; i<transform.childCount; i++)
    //         if(!transform.GetChild(i).transform.GetComponent<OnPointerEnter>().PointerOn)
    //             transform.GetChild(i).transform.localScale = _defaultScale;
        
    // }
    void Start()
    {
        Init();
    }

    void Init()
    {
        int id = 0;
        foreach(CardSO card in cardsType)
        {
            DrawPile.Add(card,card.amount);
            var c = Instantiate(card.prefab, transform);
            c.GetComponent<OnPointerEnter>().id = id;
            _defaultScale = c.transform.localScale;
            id++;
        }
    }

    public float coefs;
    public float duration;
    public void MouseOnCard(int id)
    {
        actualdCardId = id;
        SelectedCardZoom.rectTransform.position = transform.GetChild(id).transform.position;
        SelectedCardZoom.sprite = transform.GetChild(id).GetComponent<Image>().sprite;
        SelectedCardZoom.enabled = true;
        SelectedCardZoom.rectTransform.DOScale(_defaultScale * coefs, duration);
    }

    public void MouseLeavesCard()
    {
        SelectedCardZoom.enabled = false;
        SelectedCardZoom.rectTransform.localScale = _defaultScale;
    }
}
