using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public interface IPointerEvents
{
    public void MouseOnCard(GameObject sender);
    public void MouseLeavesCard(GameObject sender);
}
