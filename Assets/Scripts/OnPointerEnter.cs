using System.Diagnostics.Tracing;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class OnPointerEnter : MonoBehaviour
{
    public void PointerEnter(BaseEventData eventData)
    {
        PointerEventData t = eventData as PointerEventData;
        GameObject sender = t.pointerEnter;
        // if(GetComponent<CardTemplateConfig>().interactable)
        {
            if(GetComponent<CardTemplateConfig>().isInShop)
            {
                GameObject.FindGameObjectWithTag(GetComponent<CardTemplateConfig>().parentScriptTag).GetComponent<ShopUI>().MouseOnCard(sender);
            }
            else
                GameObject.FindGameObjectWithTag(GetComponent<CardTemplateConfig>().parentScriptTag).GetComponent<MainHandUI>().MouseOnCard(sender);
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
                GameObject.FindGameObjectWithTag(GetComponent<CardTemplateConfig>().parentScriptTag).GetComponent<ShopUI>().MouseLeavesCard(sender);
            }
            else
                GameObject.FindGameObjectWithTag(GetComponent<CardTemplateConfig>().parentScriptTag).GetComponent<MainHandUI>().MouseLeavesCard(sender);
        }
    }

    public void PointerClick(BaseEventData eventData)
    {
        PointerEventData t = eventData as PointerEventData;
        GameObject sender = t.pointerEnter;
        if(GetComponent<CardTemplateConfig>().isInShop)
            {
                GameObject.FindGameObjectWithTag(GetComponent<CardTemplateConfig>().parentScriptTag).GetComponent<ShopUI>().MouseClickCard(sender);
            }
            else
                GameObject.FindGameObjectWithTag(GetComponent<CardTemplateConfig>().parentScriptTag).GetComponent<MainHandUI>().MouseClickCard(sender);
    }
}
