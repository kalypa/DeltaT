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

    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;     // 해당 레인에서 처리할 음표의 높이를 제한하기 위한 노트 이름 변수
    public KeyCode input;                                                  // 해당 레인에서 처리할 입력 버튼을 설정하기 위한 KeyCode 변수
    public GameObject notePrefab;                                          // 해당 레인에서 생성할 노트 프리팹을 지정하기 위한 변수                                     
    List<Note> notes = new List<Note>();                                   // 생성한 노트를 리스트에 담기 위한 변수
    public List<double> timeStamps = new();                   // 레인에 할당된 음표들의 타임스탬프 정보를 저장하기 위한 리스트
    public List<double> endTimeStamps = new();                   // 레인에 할당된 음표들의 타임스탬프 정보를 저장하기 위한 리스트
    public List<bool> isLongNote = new();                   // 레인에 할당된 음표들이 롱노트인지에 대한 정보를 확인하기 위한 리스트
    public GameObject hitParticle;
    public JudgementText judgementText;
    int spawnIndex = 0;                                                    // 생성할 노트 인덱스를 기록하기 위한 변수 
    int endSpawnIndex = 0;                                                    // 생성할 롱노트 인덱스를 기록하기 위한 변수 
    public int inputIndex = 0;                                             // 입력할 노트 인덱스를 기록하기 위한 변수

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)       // 미디 파일에서 추출한 음표 정보를 이용하여 해당 레인에 음표가 떨어질 타이밍 정보를 저장하는 함수
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)                                   // 해당 레인에서 처리할 높이의 음표만 타임스탬프 리스트에 추가
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());                       // 음표의 시간 정보를 메트릭 시간 정보로 변환하여 타임스탬프 리스트에 추가
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
        }
    }
    public void SetEndTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)       // 미디 파일에서 추출한 음표 정보를 이용하여 해당 레인에 음표가 끝나는 타이밍 정보를 저장하는 함수
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)                                   // 해당 레인에서 처리할 높이의 음표만 타임스탬프 리스트에 추가
            {
                var metricEndTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.EndTime, SongManager.midiFile.GetTempoMap());                       // 음표의 끝나는 시간 정보를 메트릭 시간 정보로 변환하여 타임스탬프 리스트에 추가
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
        if (spawnIndex < timeStamps.Count)                                                                             // 해당 레인에 노트를 생성하는 코드
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)            // 노트 생성 시간이 되면 노트를 생성하는 코드
            {
                var note = Instantiate(notePrefab, transform);                                                         // 노트 생성 후 리스트에 노트를 추가하는 코드
                notes.Add(note.GetComponent<Note>());
                note.GetComponent<Note>().type = Note.NoteType.Normal;
                note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];                                // 노트가 생성된 시간을 할당하는 코드
                spawnIndex++;                                                                                          // 다음 노트 생성 인덱스를 증가시키는 코드
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
        if (inputIndex < timeStamps.Count)                                                                             // 노트 입력 
        {
            double timeStamp = timeStamps[inputIndex];                                                                 // 입력해야 하는 노트의 타임스탬프를 가져오는 코드
            double marginOfError = SongManager.Instance.marginOfError;                                                 // 노트 입력 가능 범위를 설정하는 코드
            double audioTime = SongManager.GetAudioSourceTime() - 
                (SongManager.Instance.inputDelayInMilliseconds / 1000.0);                                              // 오디오 소스의 타임스탬프를 가져와 입력이 얼마나 늦어졌는지 계산하는 코드

            if (Input.GetKeyDown(input))                                                                               // 입력키가 눌렸을 때
            {
                if (Math.Abs(audioTime - timeStamp) < marginOfError)                                                   // 입력이 맞춰졌을 때
                {
                    judgementText.TimerInit();
                    judgementText.AccuracyJudgement(audioTime, timeStamp);
                    Hit();                                                                                             // 점수 추가
                    Destroy(notes[inputIndex].gameObject);                                                             // 노트 삭제
                    inputIndex++;                                                                                      // 입력해야 하는 다음 노트 인덱스를 증가시키는 코드
                    if (FindObjectOfType<OffsetManager>() != null)                                                     // OffsetManager가 존재한다면, 오프셋을 계산하는 코드
                        OffsetMeasure(Math.Abs(audioTime - timeStamp));
                }

                else if(audioTime - timeStamp >= marginOfError)                                                                                       // 입력이 맞춰지지 않았을 때
                {
                    judgementText.TimerInit();
                    judgementText.ViewMissText();
                    print($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");        // 입력이 얼마나 늦었는지 출력하는 코드
                    if (FindObjectOfType<OffsetManager>() != null)                                                     // OffsetManager가 존재한다면, 오프셋을 계산하는 코드
                        OffsetMeasure(Math.Abs(audioTime - timeStamp));
                }
            }
            if (timeStamp + marginOfError <= audioTime)            // 노트 입력 시간을 넘어섰을 때
            {
                judgementText.TimerInit();
                judgementText.ViewMissText();
                Miss();                                            // 노트를 놓쳤을 때 처리하는 코드
                print($"Missed {inputIndex} note");
                inputIndex++;                                      // 입력해야 하는 다음 노트 인덱스를 증가시킴.
            }
        }
    }

    private void Hit()
    {
        if (FindObjectOfType<ScoreManager>() != null)        //현재 Scene에 ScoreManager가 있을 때만 실행
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
        if (FindObjectOfType<ScoreManager>() != null)        //현재 Scene에 ScoreManager가 있을 때만 실행
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