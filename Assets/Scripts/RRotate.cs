using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RRotate : MonoBehaviour
{
    float rotateSpd = -40f;

    void Start()
    {
        
    }

    void Update()
    {
        gameObject.transform.Rotate(new Vector3(0, 0, rotateSpd) * Time.deltaTime);
    }
}
