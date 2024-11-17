using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ShowHideHand : MonoBehaviour
{
    [SerializeField]
    private RectOffset hideTarget;
    [SerializeField]
    private RectOffset hideStart;
    [SerializeField]
    private RectOffset showTarget;
    [SerializeField]
    private RectOffset showStart;
    [SerializeField]
    private float duration;
    [SerializeField]
    private bool interactable = false;

    private void Start()
    {
        transform.GetComponent<HorizontalLayoutGroup>().padding = showStart;
    }
    public void ShowHand()
    {
        DOTween.To(()=> showStart, x=> transform.GetComponent<HorizontalLayoutGroup>().padding = x, showTarget, duration);
        interactable = true;
    }

    public void HideHand()
    {
        if(interactable)
        {
            interactable = false;
            DOTween.To(()=> hideStart, x=> transform.GetComponent<HorizontalLayoutGroup>().padding = x, hideTarget, duration);
        }
    }
}
