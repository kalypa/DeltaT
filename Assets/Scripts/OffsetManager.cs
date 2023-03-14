using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetManager : SingleMonobehaviour<OffsetManager>
{
    public List<double> offsetList = new List<double>();

    public double GetOffset()
    {
        double sum = 0;
        foreach (double offset in offsetList)
        {
            sum += offset;
        }
        return sum / offsetList.Count;
    }
}
