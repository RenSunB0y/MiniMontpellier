using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour, IPointerEvents
{
    public GameObject SelectedCardZoom;
    public List<CardSO> cardsType; 
    public Dictionary <CardSO, int> DrawPile = new Dictionary<CardSO, int>(); // A enlever, recup le drawpile de la classe Pile
    private Vector3 _defaultScale;
    void Start()
    {
        Init();
    }

    void Init()
    {
        int id = 0;
        foreach(CardSO card in cardsType)
        {
            var c = Instantiate(card.prefab, transform);
            c.GetComponent<CardTemplateConfig>().Load(card);
            c.GetComponent<OnPointerEnter>().id = id;
            _defaultScale = c.transform.localScale;
            id++;
        }
    }

    public float coefs;
    public float duration;
    public void MouseOnCard(CardSO data, int id)
    {
        SelectedCardZoom.transform.position = transform.GetChild(id).transform.position;
        SelectedCardZoom.GetComponent<CardTemplateConfig>().Load(data);
        SelectedCardZoom.transform.localScale = _defaultScale;

        for(int i=0; i<SelectedCardZoom.transform.childCount; i++)
            SelectedCardZoom.transform.GetChild(i).gameObject.SetActive(true);
        SelectedCardZoom.transform.DOScale(_defaultScale * coefs, duration);
    }

    public void MouseLeavesCard()
    {
        for(int i=0; i<SelectedCardZoom.transform.childCount; i++)
            SelectedCardZoom.transform.GetChild(i).gameObject.SetActive(false);
    }
}
