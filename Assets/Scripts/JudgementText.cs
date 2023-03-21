using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System;
using TMPro;

public class JudgementText : MonoBehaviour
{
    public List<Sprite> accuracySprites;                 //JudgementTextSO에서 가져온 JudgementTextData
    public ComboText comboNumText;
    public ComboText comboText;
    private float timer = 0;                                               // 정확도 텍스트가 초기화되는 시간

    public void Start()
    {
        DataManager.Instance.JudgementTextDataInit(accuracySprites);
    }

    public void TimerInit()
    {
        timer = 0;
    }

    public void AccuracyJudgement(double audioT, double timeStamp)
    {
        if(Math.Abs(audioT - timeStamp) <= 0.05)
        {
            ScoreManager.Instance.perfectCount++;
            JudgementTextChange(accuracySprites[0]);
        }
        else if (Math.Abs(audioT - timeStamp) > 0.05)
        {
            if (Math.Abs(audioT - timeStamp) <= 9)
            {
                ScoreManager.Instance.greatCount++;
                JudgementTextChange(accuracySprites[1]);
            }

            else if (Math.Abs(audioT - timeStamp) > 0.09 )
            {
                if (Math.Abs(audioT - timeStamp) <= 0.13f)
                {
                    ScoreManager.Instance.goodCount++;
                    JudgementTextChange(accuracySprites[2]);
                }
            }
        }
    }

    public void JudgementTextTimer()
    {
        if (this.GetComponent<SpriteRenderer>().enabled == true)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
        }

        if (timer >= 2)
        {
            this.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void JudgementTextChange(Sprite s)
    {
        this.GetComponent<SpriteRenderer>().enabled = true;
        this.GetComponent<SpriteRenderer>().sprite = s;
        comboNumText.GetComponent<TMP_Text>().enabled = true;
        comboText.GetComponent<TMP_Text>().enabled = true;
        comboNumText.ComboTextEffect();
        comboText.ComboTextEffect();
        this.GetComponent<TouchEffect>().ComboTextEffect();
    }

    public void ViewMissText()
    {
        this.GetComponent<SpriteRenderer>().enabled = true;
        comboNumText.GetComponent<TMP_Text>().enabled = false;
        comboText.GetComponent<TMP_Text>().enabled = false;
        this.GetComponent<SpriteRenderer>().sprite = accuracySprites[3];
        this.GetComponent<TouchEffect>().ComboTextEffect();
    }
}
