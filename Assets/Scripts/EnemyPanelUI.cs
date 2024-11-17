using GameLogic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyPanelUI : MonoBehaviour
{
    public void UpdateSecondaryHand(BaseEventData eventData)
    {
        PointerEventData t = eventData as PointerEventData;
        GameObject sender = t.pointerEnter;
        var deck = GameManager.Instance.playersGameObject[int.Parse(sender.name.Split('-')[1])].GetComponent<Player>().Deck.Pile;
        GameObject.FindGameObjectWithTag("SecondaryHand").GetComponent<MainHandUI>().UpdateMainHand(deck);
    }

    public void UpdatePanel()
    {
        
    }
}
