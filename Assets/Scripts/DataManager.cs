using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : SingleMonobehaviour<DataManager>
{
    public JudgementTextSO judgementTextSO;

    public void JudgementTextDataInit(List<Sprite> s )
    {
        foreach(Sprite sp in judgementTextSO.accuracyList)
        {
            s.Add(sp);
        }
    } 
}
