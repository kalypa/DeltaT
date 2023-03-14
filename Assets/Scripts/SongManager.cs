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
    public double marginOfError;    // ��Ʈ�� ������ ���� �÷��̾��� �Է� ����

    public int inputDelayInMilliseconds;   // �Է� ���� �ð� (�и��� ����)

    public string fileLocation; // �̵� ������ ��ġ
    public float noteTime;  // ��Ʈ�� �����Ǵ� �ð�
    public float noteSpawnY;    // ��Ʈ�� �����Ǵ� ��ġ (������)
    public float noteTapY;  // ��Ʈ�� ���ϴ� ��ġ (������)
    public float noteDespawnY    // ��Ʈ�� ������� ��ġ (������)
    {
        get
        {
            return noteTapY - (noteSpawnY - noteTapY);
        }
    }

    public static MidiFile midiFile;    // �ҷ��� �̵� ����

    void Start()
    {
        inputDelayInMilliseconds = GameManager.Instance.offset; // GameManager���� ������ �Է� ���� �ð�
        Instance = this;
        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
        {
            StartCoroutine(ReadFromWebsite()); // ������ �̵� ������ �о���̴� �ڷ�ƾ ����
        }
        else
        {
            ReadFromFile(); // ���ÿ��� �̵� ������ �о���̴� �޼ҵ� ����
        }
    }

    private IEnumerator ReadFromWebsite()   // ������ �̵� ������ �о���̴� �ڷ�ƾ
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + fileLocation))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)    // ��Ʈ��ũ ���� ó��
            {
                Debug.LogError(www.error);
            }
            else
            {
                byte[] results = www.downloadHandler.data;
                using (var stream = new MemoryStream(results))
                {
                    midiFile = MidiFile.Read(stream);   // �ҷ��� �̵� ������ �Ľ��Ͽ� ������ ����
                    GetDataFromMidi();  // �̵� ���Ͽ��� �����͸� �����Ͽ� ���ο� �Ҵ��ϴ� �޼ҵ� ����
                }
            }
        }
    }

    private void ReadFromFile() // ���ÿ��� �̵� ������ �о���̴� �޼ҵ�
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);  // �̵� ������ �Ľ��Ͽ� ������ ����
        GetDataFromMidi();  // �̵� ���Ͽ��� �����͸� �����Ͽ� ���ο� �Ҵ��ϴ� �޼ҵ� ����
    }

    public void GetDataFromMidi()   // �̵� ���Ͽ��� �����͸� �����Ͽ� ���ο� �Ҵ��ϴ� �޼ҵ�
    {
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);    // �迭�� �̵� ���Ͽ��� ������ ��Ʈ �̺�Ʈ ������ ����
        foreach (var lane in lanes) lane.SetTimeStamps(array);  // �� ���ο� ��Ʈ �̺�Ʈ ������ �Ҵ��ϴ� �޼ҵ� ȣ��

        Invoke(nameof(StartSong), songDelayInSeconds);  // ������ �ð� �� ���� �����ϴ� �޼ҵ带 ȣ���ϴ� Invoke �Լ�
    }

    public void StartSong() // �� ����
    {
        audioSource.Play(); // AudioSource ������Ʈ�� Play �޼ҵ� ȣ��
    }

    public static double GetAudioSourceTime() // ���� ���� ����ǰ� �ִ� �ð��� ��ȯ�ϴ� �޼ҵ�
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency; //���� ����� �ҽ������� ��� �ð��� �� ������ ����ϰ�, �̸� ��ȯ
    }

}