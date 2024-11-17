using UnityEngine;
using UnityEngine.UI;

public class DiceRollAnim : MonoBehaviour
{
    public Sprite actualFace;
    public void ChangeSprite()
    {
        GetComponent<Animator>().enabled = false;
        GetComponent<Image>().sprite = actualFace;
    }
}
