using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ShowHideHand : MonoBehaviour
{
    [SerializeField]
    private bool hasColor;
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
        if(hasColor)
        {
            GetComponent<Image>().raycastTarget = true;
            GetComponent<Image>().DOColor(new Color(0,0,0,83f/255f), duration);
        }
        DOTween.To(()=> showStart, x=> transform.GetComponent<HorizontalLayoutGroup>().padding = x, showTarget, duration);
        interactable = true;
    }

    public void HideHand()
    {
        if(hasColor)
        {
            GetComponent<Image>().DOColor(Color.black*0, duration);
            GetComponent<Image>().raycastTarget = false;
        }
        if(interactable)
        {
            interactable = false;
            DOTween.To(()=> hideStart, x=> transform.GetComponent<HorizontalLayoutGroup>().padding = x, hideTarget, duration);
        }
    }
}
