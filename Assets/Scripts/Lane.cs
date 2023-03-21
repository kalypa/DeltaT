using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Melanchall.DryWetMidi.MusicTheory;
using Unity.VisualScripting;

public class Lane : MonoBehaviour
{

    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;     // �ش� ���ο��� ó���� ��ǥ�� ���̸� �����ϱ� ���� ��Ʈ �̸� ����
    public KeyCode input;                                                  // �ش� ���ο��� ó���� �Է� ��ư�� �����ϱ� ���� KeyCode ����
    public GameObject notePrefab;                                          // �ش� ���ο��� ������ ��Ʈ �������� �����ϱ� ���� ����                                     
    List<Note> notes = new List<Note>();                                   // ������ ��Ʈ�� ����Ʈ�� ��� ���� ����
    public List<double> timeStamps = new();                   // ���ο� �Ҵ�� ��ǥ���� Ÿ�ӽ����� ������ �����ϱ� ���� ����Ʈ
    public List<double> endTimeStamps = new();                   // ���ο� �Ҵ�� ��ǥ���� Ÿ�ӽ����� ������ �����ϱ� ���� ����Ʈ
    public List<bool> isLongNote = new();                   // ���ο� �Ҵ�� ��ǥ���� �ճ�Ʈ������ ���� ������ Ȯ���ϱ� ���� ����Ʈ
    public GameObject hitParticle;
    public JudgementText judgementText;
    int spawnIndex = 0;                                                    // ������ ��Ʈ �ε����� ����ϱ� ���� ���� 
    int endSpawnIndex = 0;                                                    // ������ �ճ�Ʈ �ε����� ����ϱ� ���� ���� 
    public int inputIndex = 0;                                             // �Է��� ��Ʈ �ε����� ����ϱ� ���� ����

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
    public void SetEndTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)       // �̵� ���Ͽ��� ������ ��ǥ ������ �̿��Ͽ� �ش� ���ο� ��ǥ�� ������ Ÿ�̹� ������ �����ϴ� �Լ�
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)                                   // �ش� ���ο��� ó���� ������ ��ǥ�� Ÿ�ӽ����� ����Ʈ�� �߰�
            {
                var metricEndTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.EndTime, SongManager.midiFile.GetTempoMap());                       // ��ǥ�� ������ �ð� ������ ��Ʈ�� �ð� ������ ��ȯ�Ͽ� Ÿ�ӽ����� ����Ʈ�� �߰�
                if(note.Length > 32)
                {
                    endTimeStamps.Add((double)metricEndTimeSpan.Minutes * 60f + metricEndTimeSpan.Seconds + (double)metricEndTimeSpan.Milliseconds / 1000f);
                    isLongNote.Add(true);
                }
                else if(note.Length == 32)
                {
                    endTimeStamps.Add((double)metricEndTimeSpan.Minutes * 60f + metricEndTimeSpan.Seconds + (double)metricEndTimeSpan.Milliseconds / 1000f);
                    isLongNote.Add(false);
                }
            }
        }
    }

    void Update()
    {
        judgementText.JudgementTextTimer();
        if (spawnIndex < timeStamps.Count)                                                                             // �ش� ���ο� ��Ʈ�� �����ϴ� �ڵ�
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)            // ��Ʈ ���� �ð��� �Ǹ� ��Ʈ�� �����ϴ� �ڵ�
            {
                var note = Instantiate(notePrefab, transform);                                                         // ��Ʈ ���� �� ����Ʈ�� ��Ʈ�� �߰��ϴ� �ڵ�
                notes.Add(note.GetComponent<Note>());
                note.GetComponent<Note>().type = Note.NoteType.Normal;
                note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];                                // ��Ʈ�� ������ �ð��� �Ҵ��ϴ� �ڵ�
                spawnIndex++;                                                                                          // ���� ��Ʈ ���� �ε����� ������Ű�� �ڵ�
            }
        }
        //if(endSpawnIndex < endTimeStamps.Count)
        //{
        //    if(SongManager.GetAudioSourceTime() >= endTimeStamps[endSpawnIndex] - SongManager.Instance.noteTime)
        //    {
        //        if (isLongNote[endSpawnIndex])
        //        {
        //            var note = Instantiate(notePrefab, transform);
        //            note.GetComponent<Note>().type = Note.NoteType.Long;
        //            notes.Add(note.GetComponent<Note>());
        //            note.GetComponent<Note>().assignedTime = (float)endTimeStamps[endSpawnIndex];
        //            endSpawnIndex++;
        //        }
        //        else
        //        {
        //            endSpawnIndex++;
        //        }
        //    }
        //}
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
                    judgementText.TimerInit();
                    judgementText.AccuracyJudgement(audioTime, timeStamp);
                    Hit();                                                                                             // ���� �߰�
                    Destroy(notes[inputIndex].gameObject);                                                             // ��Ʈ ����
                    inputIndex++;                                                                                      // �Է��ؾ� �ϴ� ���� ��Ʈ �ε����� ������Ű�� �ڵ�
                    if (FindObjectOfType<OffsetManager>() != null)                                                     // OffsetManager�� �����Ѵٸ�, �������� ����ϴ� �ڵ�
                        OffsetMeasure(Math.Abs(audioTime - timeStamp));
                }

                else if(audioTime - timeStamp >= marginOfError)                                                                                       // �Է��� �������� �ʾ��� ��
                {
                    judgementText.TimerInit();
                    judgementText.ViewMissText();
                    print($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");        // �Է��� �󸶳� �ʾ����� ����ϴ� �ڵ�
                    if (FindObjectOfType<OffsetManager>() != null)                                                     // OffsetManager�� �����Ѵٸ�, �������� ����ϴ� �ڵ�
                        OffsetMeasure(Math.Abs(audioTime - timeStamp));
                }
            }
            if (timeStamp + marginOfError <= audioTime)            // ��Ʈ �Է� �ð��� �Ѿ�� ��
            {
                judgementText.TimerInit();
                judgementText.ViewMissText();
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

    private void Miss()
    {
        if (FindObjectOfType<ScoreManager>() != null)        //���� Scene�� ScoreManager�� ���� ���� ����
            ScoreManager.Instance.Miss();
    }

    private void ParticleDisable()
    {
        hitParticle.SetActive(false);
    }

    private void OffsetMeasure(double d)
    {
        if (FindObjectOfType<OffsetManager>() == null)
            return;
        OffsetManager.Instance.offsetList.Add(d);
    }
}