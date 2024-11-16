using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour, IPointerEvents
{
    public Image SelectedCardZoom; //Carte "factice" pour animation DOTween
    public List<CardSO> cardsType; //Toutes les types de cartes
    public Dictionary <CardSO, int> DrawPile = new Dictionary<CardSO, int>(); //Dictionnaire contenant cartes + nombre exemplaire
    private Vector3 _defaultScale;
    private int actualdCardId; //id carte actuelle

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

    //Initialisation DrawPile (= pile plac�e au centre)
    void Init()
    {
        int id = 0;
        foreach(CardSO card in cardsType)
        {
            DrawPile.Add(card,card.amount);
            var c = Instantiate(card.prefab, transform);
            c.GetComponent<CardTemplateConfig>().Load(card);
            c.GetComponent<OnPointerEnter>().id = id;
            _defaultScale = c.transform.localScale;
            id++;
        }
    }

    public float coefs;
    public float duration;

    //M�thode event appel�e quand curseur EST sur carte pass�e en id (dans shop)
    public void MouseOnCard(int id)
    {
        actualdCardId = id;
        SelectedCardZoom.transform.position = transform.GetChild(id).transform.position;
        SelectedCardZoom.GetComponent<CardTemplateConfig>().Load(data);
        for(int i=0; i<SelectedCardZoom.transform.childCount; i++)
            SelectedCardZoom.transform.GetChild(i).gameObject.SetActive(true);
        SelectedCardZoom.transform.DOScale(_defaultScale * coefs, duration);
    }

    //M�thode event appel�e quand curseur n'est PLUS sur carte pass�e en id (dans shop)
    public void MouseLeavesCard()
    {
        for(int i=0; i<SelectedCardZoom.transform.childCount; i++)
            SelectedCardZoom.transform.GetChild(i).gameObject.SetActive(false);
        SelectedCardZoom.transform.localScale = _defaultScale;
    }
}
