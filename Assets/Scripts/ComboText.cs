using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ComboText : MonoBehaviour
{
    [SerializeField]
    private float scaleOffset = 0.25f;
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void ComboTextEffect()
    {
        rectTransform.localScale = new Vector3(1f, 1f, 1f);
        Vector3 OriginalScale = rectTransform.localScale;
        DOTween.Sequence().Append(rectTransform.DOScale(new Vector3(OriginalScale.x + scaleOffset, OriginalScale.y + scaleOffset, OriginalScale.z + scaleOffset), 0.02f).SetEase(Ease.Linear)).Append(rectTransform.DOScale(new Vector3(1f, 1f, 1f), 0.02f).SetEase(Ease.Linear));
    }
}
