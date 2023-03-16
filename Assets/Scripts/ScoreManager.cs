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
    public int perfectCount = 0;
    public int greatCount = 0;
    public int goodCount = 0;
    int hitCount = 0;
    public int missCount = 0;

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
        scoreText.text = "Score : " + score.ToString();
        comboText.text = "Combo : " + combo.ToString();
        hitAccuracyText.text = "Accuracy : " + CalculateHitAccuracy().ToString("F2") + "%";
    }

    public float CalculateHitAccuracy()
    {
        if (hitCount + missCount == 0)
            return 0;

        return (float)(((perfectCount * 100f) + (greatCount * 75f) + (goodCount * 40f)) / ( hitCount + missCount));
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
