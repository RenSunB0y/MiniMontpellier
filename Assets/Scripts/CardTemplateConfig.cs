using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardTemplateConfig : MonoBehaviour
{
    public CardSO card;
    public void Start()
    {
        Load(card);
    }
    public void Load(CardSO data)
    {
        GetComponent<Image>().sprite = data.sprite;
    }
}
