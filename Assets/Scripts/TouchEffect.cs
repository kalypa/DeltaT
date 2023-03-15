using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class TouchEffect : SingleMonobehaviour<TouchEffect>
{
    [SerializeField]
    private float scaleOffset = 10f;

    public void ComboTextEffect()
    {
        DOTween.KillAll();
        transform.localScale = new Vector3(1, 1, 1);
        Vector3 OriginalScale = transform.localScale;
        DOTween.Sequence().Append(transform.DOScale(new Vector3(OriginalScale.x, OriginalScale.y, OriginalScale.z - scaleOffset), 0.11f).SetEase(Ease.Linear)).Append(transform.DOScale(new Vector3(1, 1, 1), 0.11f).SetEase(Ease.Linear));
    }
    void ActiveFalseObj()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
    }
}
