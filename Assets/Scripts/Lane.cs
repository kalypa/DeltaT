using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Melanchall.DryWetMidi.MusicTheory;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;     // �ش� ���ο��� ó���� ��ǥ�� ���̸� �����ϱ� ���� ��Ʈ �̸� ����
    public KeyCode input;                                                  // �ش� ���ο��� ó���� �Է� ��ư�� �����ϱ� ���� KeyCode ����
    public GameObject notePrefab;                                          // �ش� ���ο��� ������ ��Ʈ �������� �����ϱ� ���� ����                                     
    List<Note> notes = new List<Note>();                                   // ������ ��Ʈ�� ����Ʈ�� ��� ���� ����
    public List<double> timeStamps = new List<double>();                   // ���ο� �Ҵ�� ��ǥ���� Ÿ�ӽ����� ������ �����ϱ� ���� ����Ʈ
    public GameObject hitParticle;
    public GameObject judgementText;
    public Sprite perfect;
    public Sprite great;
    public Sprite good;
    public Sprite miss;
    int spawnIndex = 0;                                                    // ������ ��Ʈ �ε����� ����ϱ� ���� ���� 
    public int inputIndex = 0;                                             // �Է��� ��Ʈ �ε����� ����ϱ� ���� ����
    private float timer = 0;
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)       // �̵� ���Ͽ��� ������ ��ǥ ������ �̿��Ͽ� �ش� ���ο� ��ǥ�� ������ Ÿ�̹� ������ �����ϴ� �Լ�
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)                                   // �ش� ���ο��� ó���� ������ ��ǥ�� Ÿ�ӽ����� ����Ʈ�� �߰�
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());                       // ��ǥ�� �ð� ������ ��Ʈ�� �ð� ������ ��ȯ�Ͽ� Ÿ�ӽ����� ����Ʈ�� �߰�
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
        }
    }


    void Update()
    {
        JudgementTextTimer();
        if (spawnIndex < timeStamps.Count)                                                                             // �ش� ���ο� ��Ʈ�� �����ϴ� �ڵ�
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)            // ��Ʈ ���� �ð��� �Ǹ� ��Ʈ�� �����ϴ� �ڵ�
            {
                var note = Instantiate(notePrefab, transform);                                                     // ��Ʈ ���� �� ����Ʈ�� ��Ʈ�� �߰��ϴ� �ڵ�
                notes.Add(note.GetComponent<Note>());
                note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];                            // ��Ʈ�� ������ �ð��� �Ҵ��ϴ� �ڵ�
                spawnIndex++;                                                                                      // ���� ��Ʈ ���� �ε����� ������Ű�� �ڵ�
            }
        }

        if (inputIndex < timeStamps.Count)                                                                             // ��Ʈ �Է� 
        {
            double timeStamp = timeStamps[inputIndex];                                                                 // �Է��ؾ� �ϴ� ��Ʈ�� Ÿ�ӽ������� �������� �ڵ�
            double marginOfError = SongManager.Instance.marginOfError;                                                 // ��Ʈ �Է� ���� ������ �����ϴ� �ڵ�
            double audioTime = SongManager.GetAudioSourceTime() - 
                (SongManager.Instance.inputDelayInMilliseconds / 1000.0);                                              // ����� �ҽ��� Ÿ�ӽ������� ������ �Է��� �󸶳� �ʾ������� ����ϴ� �ڵ�

            if (Input.GetKeyDown(input))                                                                               // �Է�Ű�� ������ ��
            {
                if (Math.Abs(audioTime - timeStamp) < marginOfError)                                                   // �Է��� �������� ��
                {
                    timer = 0;
                    AccuracyJudgement(audioTime, timeStamp);
                    Hit();                                                                                             // ���� �߰�
                    Destroy(notes[inputIndex].gameObject);                                                             // ��Ʈ ����
                    inputIndex++;                                                                                      // �Է��ؾ� �ϴ� ���� ��Ʈ �ε����� ������Ű�� �ڵ�
                    if (FindObjectOfType<OffsetManager>() != null)                                                     // OffsetManager�� �����Ѵٸ�, �������� ����ϴ� �ڵ�
                        OffsetMeasure(Math.Abs(audioTime - timeStamp));
                }

                else if(audioTime - timeStamp >= marginOfError)                                                                                       // �Է��� �������� �ʾ��� ��
                {
                    timer = 0;
                    judgementText.GetComponent<SpriteRenderer>().enabled = true;
                    judgementText.GetComponent<SpriteRenderer>().sprite = miss;
                    print($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");        // �Է��� �󸶳� �ʾ����� ����ϴ� �ڵ�
                    if (FindObjectOfType<OffsetManager>() != null)                                                     // OffsetManager�� �����Ѵٸ�, �������� ����ϴ� �ڵ�
                        OffsetMeasure(Math.Abs(audioTime - timeStamp));
                }
            }
            if (timeStamp + marginOfError <= audioTime)            // ��Ʈ �Է� �ð��� �Ѿ�� ��
            {
                timer = 0;
                judgementText.GetComponent<SpriteRenderer>().enabled = true;
                judgementText.GetComponent<SpriteRenderer>().sprite = miss;
                Miss();                                            // ��Ʈ�� ������ �� ó���ϴ� �ڵ�
                print($"Missed {inputIndex} note");
                inputIndex++;                                      // �Է��ؾ� �ϴ� ���� ��Ʈ �ε����� ������Ŵ.
            }
        }
    }

    private void Hit()
    {
        if (FindObjectOfType<ScoreManager>() != null)        //���� Scene�� ScoreManager�� ���� ���� ����
        {
            if(hitParticle.activeSelf == false)
                hitParticle.SetActive(true);
            else
            {
                hitParticle.SetActive(false);
                hitParticle.SetActive(true);
            }
            Invoke("ParticleDisable", 0.2f);
            ScoreManager.Instance.Hit();
        }
    }
    private void ParticleDisable()
    {
        hitParticle.SetActive(false);
    }

    private void Miss()
    {
        if (FindObjectOfType<ScoreManager>() != null)        //���� Scene�� ScoreManager�� ���� ���� ����
            ScoreManager.Instance.Miss();
    }

    private void OffsetMeasure(double d)
    {
        if (FindObjectOfType<OffsetManager>() == null)
            return;
        OffsetManager.Instance.offsetList.Add(d);
    }

    void AccuracyJudgement(double audioT, double timeStamp)
    {
        if(Math.Abs(audioT - timeStamp) <= 0.05)
        {
            ScoreManager.Instance.perfectCount++;
            judgementText.GetComponent<SpriteRenderer>().enabled = true;
            judgementText.GetComponent<SpriteRenderer>().sprite = perfect;
            judgementText.GetComponent<TouchEffect>().ComboTextEffect();
        }
        else if(Math.Abs(audioT - timeStamp) > 0.05)
        {
            if (Math.Abs(audioT - timeStamp) <= 0.09)
            {
                ScoreManager.Instance.greatCount++;
                judgementText.GetComponent<SpriteRenderer>().enabled = true;
                judgementText.GetComponent<SpriteRenderer>().sprite = great;
            }
            else if(Math.Abs(audioT - timeStamp) > 0.09 && Math.Abs(audioT - timeStamp) <= 0.13)
            {
                ScoreManager.Instance.goodCount++;
                judgementText.GetComponent<SpriteRenderer>().enabled = true;
                judgementText.GetComponent<SpriteRenderer>().sprite = good;
            }
        }
    }

    void JudgementTextTimer()
    {
        if(judgementText.GetComponent <SpriteRenderer>().enabled == true)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
        }

        if(timer >= 2)
        {
            judgementText.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}