using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using System.Data.Common;

public class HorizontalLayoutBehaviour : MonoBehaviour
{
    public GameObject Card;
    public Image SelectedCardZoom;
    [SerializeField]
    private List<CardSO> cards = new List<CardSO>();
    private int actualdCardId;
    private const float MIN_SPACE = 20;
    
    int count = 0;
    IEnumerator FillMain()
    {
        yield return new WaitForSeconds(0.05f);
        cards.Add(cards[0]);
        UpdateMainHand();
        if(count<7)
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
            id++;
        }
        transform.GetComponent<HorizontalLayoutGroup>().spacing = -(cards.Count * 6 + MIN_SPACE);
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
        SelectedCardZoom.transform.position = new Vector2(selectedCardImage.transform.position.x-SelectedCardZoom.transform.lossyScale.x/2f, SelectedCardZoom.transform.position.y);
        SelectedCardZoom.enabled = true;
    }

    public void MouseLeavesCard()
    {
        transform.GetChild(actualdCardId).GetComponent<Image>().color = new Color(0.8f,0.8f,0.8f);
        SelectedCardZoom.enabled = false;
    }

}
