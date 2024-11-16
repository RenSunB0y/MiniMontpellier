using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class OnPointerEnter : MonoBehaviour
{
    public bool PointerOn;
    public int id;
    public float coefs;
    private Vector3 _defaultScale;

    void Start()
    {
        _defaultScale = transform.localScale;
    }
    public void PointerEnter()
    {
        PointerOn = true;
        if(transform.parent.name == "MainHand")
            transform.parent.GetComponent<MainHandUI>().MouseOnCard(GetComponent<CardTemplateConfig>().card,id);
        else if(transform.parent.name == "Shop")
        {
            transform.parent.GetComponent<ShopUI>().MouseOnCard(GetComponent<CardTemplateConfig>().card, id);
        }
    }

    public void PointerExit()
    {
        PointerOn = false;
        if(transform.parent.name == "MainHand")
            transform.parent.GetComponent<MainHandUI>().MouseLeavesCard();
        else if(transform.parent.name == "Shop")
        {
            transform.parent.GetComponent<ShopUI>().MouseLeavesCard();
        }
    }
}
