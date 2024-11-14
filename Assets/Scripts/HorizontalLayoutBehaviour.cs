using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class HorizontalLayoutBehaviour : MonoBehaviour
{
    public GameObject Card;
    public Image SelectedCard;
    [SerializeField]
    private List<CardSO> cards = new List<CardSO>();
    private const float MIN_SPACE = 20;
    
    int count = 0;
    IEnumerator FillMain()
    {
        yield return new WaitForSeconds(0f);
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
        for(int i=1; i<transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        foreach (CardSO card in cards)
        {
            var c = Instantiate(Card, Vector3.zero, Quaternion.identity, transform).GetComponent<Image>().sprite = card.sprite;
        }
        transform.GetComponent<HorizontalLayoutGroup>().spacing = -(cards.Count * 6 + MIN_SPACE);
    }

    public void MouseOnCard(int id)
    {
        Debug.Log("ENTER");
        Image selectedCardImage = transform.GetChild(id).GetComponent<Image>();
        selectedCardImage.color = new Color(1,1,1,0);
        SelectedCard.sprite = selectedCardImage.sprite;
        SelectedCard.transform.position = new Vector2(selectedCardImage.transform.position.x, SelectedCard.transform.position.y);
        SelectedCard.enabled = true;
    }

    public void MouseLeavesCard(int id)
    {
        Debug.Log("EXIT");
        transform.GetChild(id).GetComponent<Image>().color = Color.white;
        SelectedCard.enabled = false;
    }
}
