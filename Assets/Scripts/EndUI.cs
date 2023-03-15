using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndUI : SingleMonobehaviour<EndUI>
{
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text accuracyText;

    public void EndTexts()
    {
        if(ScoreManager.Instance.CalculateHitAccuracy() >= 95)
        {
            rankText.text = "S";
        }
        else if(ScoreManager.Instance.CalculateHitAccuracy() >= 90 && ScoreManager.Instance.CalculateHitAccuracy() < 95)
        {
            rankText.text = "A";
        }
        else if(ScoreManager.Instance.CalculateHitAccuracy() >= 80 && ScoreManager.Instance.CalculateHitAccuracy() < 90)
        {
            rankText.text = "B";
        }
        else if(ScoreManager.Instance.CalculateHitAccuracy() >= 70 && ScoreManager.Instance.CalculateHitAccuracy() < 80)
        {
            rankText.text = "C";
        }
        else if(ScoreManager.Instance.CalculateHitAccuracy() < 70)
        {
            rankText.text = "Failed";
        }
        scoreText.text = ScoreManager.Instance.scoreText.text;
        accuracyText.text = ScoreManager.Instance.hitAccuracyText.text;
    }
}
