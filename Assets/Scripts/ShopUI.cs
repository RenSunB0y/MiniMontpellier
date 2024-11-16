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
            c.GetComponent<CardTemplateConfig>().Load(card,true);
            _defaultScale = c.transform.localScale;
            id++;
        }
    }

    public float coefs;
    public float duration;
    public void MouseOnCard(GameObject sender)
    {
        SelectedCardZoom.GetComponent<CardTemplateConfig>().Load(sender.GetComponent<CardTemplateConfig>().card,true);
        SelectedCardZoom.transform.position = sender.transform.position;
        SelectedCardZoom.transform.localScale = _defaultScale;
        SelectedCardZoom.transform.DOScale(_defaultScale * coefs, duration);
        for(int i=0; i<SelectedCardZoom.transform.childCount; i++)
            SelectedCardZoom.transform.GetChild(i).gameObject.SetActive(true);
    }

    public void MouseLeavesCard(GameObject sender)
    {
        for(int i=0; i<SelectedCardZoom.transform.childCount; i++)
            SelectedCardZoom.transform.GetChild(i).gameObject.SetActive(false);
    }
}
