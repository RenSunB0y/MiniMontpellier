using System.Diagnostics.Tracing;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class OnPointerEnter : MonoBehaviour
{
    public float coefs;
    private Vector3 _defaultScale;

    void Start()
    {
        _defaultScale = transform.localScale;
    }
    public void PointerEnter(BaseEventData eventData)
    {
        PointerEventData t = eventData as PointerEventData;
        GameObject sender = t.pointerEnter;
        // if(GetComponent<CardTemplateConfig>().interactable)
        {
            if(GetComponent<CardTemplateConfig>().isInShop)
            {
                GameObject.FindGameObjectWithTag("Shop").GetComponent<ShopUI>().MouseOnCard(sender);
            }
            else
                GameObject.FindGameObjectWithTag("MainHand").GetComponent<MainHandUI>().MouseOnCard(sender);
        }
    }

    public void PointerExit(BaseEventData eventData)
    {
        PointerEventData t = eventData as PointerEventData;
        GameObject sender = t.pointerEnter;
        // if(GetComponent<CardTemplateConfig>().interactable)
        {
            if(GetComponent<CardTemplateConfig>().isInShop)
            {
                GameObject.FindGameObjectWithTag("Shop").GetComponent<ShopUI>().MouseLeavesCard(sender);
            }
            else
                GameObject.FindGameObjectWithTag("MainHand").GetComponent<MainHandUI>().MouseLeavesCard(sender);
        }
    }

    public void PointerClick(BaseEventData eventData)
    {
        PointerEventData t = eventData as PointerEventData;
        GameObject sender = t.pointerEnter;
        if(GetComponent<CardTemplateConfig>().interactable)
        {
            if(GetComponent<CardTemplateConfig>().isInShop)
            {
                Debug.Log(sender.GetComponent<CardTemplateConfig>().card.name);
            }

        }
    }
}
