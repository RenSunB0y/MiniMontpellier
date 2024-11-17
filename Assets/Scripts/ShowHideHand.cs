using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ShowHideHand : MonoBehaviour
{
    [SerializeField]
    private RectOffset start;
    [SerializeField]
    private RectOffset target;
    [SerializeField]
    private float duration;
    [SerializeField]
    private bool startHided;

    private void Start()
    {
        if(startHided)
        HideHand();
        else
        ShowHand();
    }
    public void ShowHand()
    {
        DOTween.To(()=> transform.GetComponent<HorizontalLayoutGroup>().padding, x=> transform.GetComponent<HorizontalLayoutGroup>().padding = x, target, duration);
    }

    public void HideHand()
    {
        DOTween.To(()=> transform.GetComponent<HorizontalLayoutGroup>().padding, x=> transform.GetComponent<HorizontalLayoutGroup>().padding = x, start, duration);
    }
}
