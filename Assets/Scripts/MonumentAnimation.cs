using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MonumentAnimation : MonoBehaviour
{
    private Image sprite;
    [SerializeField]
    private float ymov;
    [SerializeField]
    private float duration;
    [SerializeField]
    private float startDiff;
    [SerializeField]
    private int startDir;

    IEnumerator Animation()
    {
        yield return new WaitForSeconds(startDiff);
        startDiff = 0;
        MoveAnim(startDir);
        yield return new WaitForSeconds(duration);
        MoveAnim(-startDir);
        yield return new WaitForSeconds(duration);
        StartCoroutine("Animation");
    }
    void Start()
    {
        sprite = GetComponent<Image>();
        sprite.color = new Color(1,1,1,0.3f);
        MoveAnim(-startDir/2f);
        startDiff = duration/2f;
        StartCoroutine("Animation");
    }

    private void MoveAnim(float sign)
    {
        transform.DOMoveY(transform.position.y + ymov*sign, duration).SetEase(Ease.InOutQuad);;
    }
}
