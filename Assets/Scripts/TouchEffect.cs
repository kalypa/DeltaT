using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class TouchEffect : SingleMonobehaviour<TouchEffect>
{
    [SerializeField]
    private float scaleOffset = 0.5f;

    public void ComboTextEffect()
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 OriginalScale = transform.localScale;
        DOTween.Sequence().Append(transform.DOScale(new Vector3(OriginalScale.x + scaleOffset, OriginalScale.y + scaleOffset, OriginalScale.z), 0.02f).SetEase(Ease.Linear)).Append(transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.02f).SetEase(Ease.Linear));
    }
}
