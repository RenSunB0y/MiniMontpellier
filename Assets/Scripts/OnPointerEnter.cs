using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class OnPointerEnter : MonoBehaviour
{
    public HorizontalLayoutBehaviour MainHand;
    public int id;
    public void PointerEnter()
    {
        MainHand.MouseOnCard(id);
    }

    public void PointerExit()
    {
        MainHand.MouseLeavesCard();
    }
}
