using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockRotate : MonoBehaviour
{
    [SerializeField]
    private GameObject secondhand;
    private float rotateSpd = -2;
    private bool isCycle = false;
    void Start()
    {
        
    }

    void Update()
    {
        if (gameObject.transform.rotation.z >= 0.1f && gameObject.transform.rotation.z < 0.15f && isCycle == false)
        {
            isCycle = true;
            secondhand.transform.eulerAngles = new Vector3(0, 0, -155.7f);
        }

        else
        {
            isCycle = false;
        }
    }
}
