using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : SingleMonobehaviour<ScoreManager>
{
    public TMP_Text scoreText;
    public TMP_Text comboText;
    public TMP_Text hitAccuracyText;

    int score = 0;
    int combo = 0;
    int maxCombo = 0;
    int hitCount = 0;
    int missCount = 0;

    void Start()
    {
        UpdateUI();
    }

    public void Hit()
    {

        hitCount++;
        combo++;

        score += 100 * combo;

        if (combo > maxCombo)
            maxCombo = combo;

        UpdateUI();
    }

    public void Miss()
    {
        missCount++;
        combo = 0;

        UpdateUI();
    }

    void UpdateUI()
    {
        scoreText.text = score.ToString();
        comboText.text = combo.ToString();
        hitAccuracyText.text = CalculateHitAccuracy().ToString("F2") + "%";
    }

    float CalculateHitAccuracy()
    {
        if (hitCount + missCount == 0)
            return 0;

        return (float)hitCount / (float)(hitCount + missCount) * 100f;
    }

    public void Reset()
    {
        score = 0;
        combo = 0;
        maxCombo = 0;
        hitCount = 0;
        missCount = 0;

        UpdateUI();
    }
}
