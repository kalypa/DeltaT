using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class TouchEffect : MonoBehaviour
{
    [SerializeField]
    private float scaleOffset = 10f;
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.J))
        {
            DOTween.KillAll();
            transform.localScale = new Vector3(1, 1, 1);
            Vector3 OriginalScale = transform.localScale;
            DOTween.Sequence().Append(transform.DOScale(new Vector3(OriginalScale.x, OriginalScale.y, OriginalScale.z - scaleOffset)
                , 0.11f).SetEase(Ease.Linear)).Append(transform.DOScale(new Vector3(1, 1, 1), 0.11f).SetEase(Ease.Linear));
        }
    }
}
