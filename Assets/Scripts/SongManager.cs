using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource audioSource;
    public Lane[] lanes;
    public float songDelayInSeconds;
    public double marginOfError;    // 노트를 눌렀을 때의 플레이어의 입력 오차

    public int inputDelayInMilliseconds;   // 입력 지연 시간 (밀리초 단위)

    public string fileLocation; // 미디 파일의 위치
    public float noteTime;  // 노트가 생성되는 시간
    public float noteSpawnY;    // 노트가 생성되는 위치 (세로축)
    public float noteTapY;  // 노트를 탭하는 위치 (세로축)
    public float noteDespawnY    // 노트가 사라지는 위치 (세로축)
    {
        get
        {
            return noteTapY - (noteSpawnY - noteTapY);
        }
    }

    public static MidiFile midiFile;    // 불러온 미디 파일

    void Start()
    {
        inputDelayInMilliseconds = GameManager.Instance.offset; // GameManager에서 가져온 입력 지연 시간
        Instance = this;
        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
        {
            StartCoroutine(ReadFromWebsite()); // 웹에서 미디 파일을 읽어들이는 코루틴 실행
        }
        else
        {
            ReadFromFile(); // 로컬에서 미디 파일을 읽어들이는 메소드 실행
        }
    }

    private IEnumerator ReadFromWebsite()   // 웹에서 미디 파일을 읽어들이는 코루틴
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + fileLocation))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)    // 네트워크 오류 처리
            {
                Debug.LogError(www.error);
            }
            else
            {
                byte[] results = www.downloadHandler.data;
                using (var stream = new MemoryStream(results))
                {
                    midiFile = MidiFile.Read(stream);   // 불러온 미디 파일을 파싱하여 변수에 저장
                    GetDataFromMidi();  // 미디 파일에서 데이터를 추출하여 레인에 할당하는 메소드 실행
                }
            }
        }
    }

    private void ReadFromFile() // 로컬에서 미디 파일을 읽어들이는 메소드
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);  // 미디 파일을 파싱하여 변수에 저장
        GetDataFromMidi();  // 미디 파일에서 데이터를 추출하여 레인에 할당하는 메소드 실행
    }

    public void GetDataFromMidi()   // 미디 파일에서 데이터를 추출하여 레인에 할당하는 메소드
    {
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);    // 배열에 미디 파일에서 추출한 노트 이벤트 정보를 복사
        foreach (var lane in lanes) lane.SetTimeStamps(array);  // 각 레인에 노트 이벤트 정보를 할당하는 메소드 호출

        Invoke(nameof(StartSong), songDelayInSeconds);  // 지정한 시간 후 곡을 시작하는 메소드를 호출하는 Invoke 함수
    }

    public void StartSong() // 곡 시작
    {
        audioSource.Play(); // AudioSource 컴포넌트의 Play 메소드 호출
    }

    public static double GetAudioSourceTime() // 현재 곡이 재생되고 있는 시간을 반환하는 메소드
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency; //현재 오디오 소스에서의 재생 시간을 초 단위로 계산하고, 이를 반환
    }

}