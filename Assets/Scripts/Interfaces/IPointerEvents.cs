using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public interface IPointerEvents
{
    public void MouseOnCard(CardSO data, int id);
    public void MouseLeavesCard();
}
